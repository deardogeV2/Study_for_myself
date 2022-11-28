package com.hmdp.service.impl;

import cn.hutool.core.bean.BeanUtil;
import com.hmdp.dto.Result;
import com.hmdp.entity.SeckillVoucher;
import com.hmdp.entity.Voucher;
import com.hmdp.entity.VoucherOrder;
import com.hmdp.mapper.VoucherOrderMapper;
import com.hmdp.service.IVoucherOrderService;
import com.baomidou.mybatisplus.extension.service.impl.ServiceImpl;
import com.hmdp.utils.RedisIdWorker;
import com.hmdp.utils.SimpleRedisLock;
import com.hmdp.utils.UserHolder;
import lombok.extern.slf4j.Slf4j;
import lombok.var;
import org.redisson.api.RLock;
import org.redisson.api.RedissonClient;
import org.springframework.aop.framework.AopContext;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.core.io.ClassPathResource;
import org.springframework.data.redis.connection.stream.*;
import org.springframework.data.redis.core.StringRedisTemplate;
import org.springframework.data.redis.core.script.DefaultRedisScript;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import javax.annotation.PostConstruct;
import javax.annotation.Resource;
import java.time.Duration;
import java.time.LocalDateTime;
import java.time.LocalTime;
import java.time.ZoneOffset;
import java.util.Collections;
import java.util.List;
import java.util.Map;
import java.util.concurrent.*;

import static com.hmdp.Constants.RedisConstants.*;

/**
 * <p>
 * 服务实现类
 * </p>
 *
 * @author 虎哥
 * @since 2021-12-22
 */
@Slf4j
@Service
public class VoucherOrderServiceImpl extends ServiceImpl<VoucherOrderMapper, VoucherOrder> implements IVoucherOrderService {

    @Autowired
    VoucherServiceImpl voucherService;

    @Autowired
    SeckillVoucherServiceImpl seckillVoucherService;

    @Resource
    RedisIdWorker redisIdWorker;

    @Resource
    RedissonClient redissonClient;

    @Resource
    StringRedisTemplate stringRedisTemplate;

    private static final DefaultRedisScript<Long> SECKILL_SCRIPT;

    static { // 初始化脚本对象
        SECKILL_SCRIPT = new DefaultRedisScript<>();
        SECKILL_SCRIPT.setLocation(new ClassPathResource("secKill.lua"));// 设置脚本对象的内容
        SECKILL_SCRIPT.setResultType(Long.class); // 设置返回类型
    }

    // 把代理对象放在外面
    IVoucherOrderService proxy;

    @Override
    public Result addVoucher(Long voucherId) throws InterruptedException {

        // 查询该优惠券
        Voucher voucher = voucherService.getById(voucherId);

        // 获取用户信息
        Long userId = UserHolder.getUser().getId();

        // 判断类型是不是秒杀卷
        if (voucher.getType() == VOUCHER_TYPE_NORMAL) {
            // 直接创建订单
            Boolean result = addVoucherOrder(userId, voucher.getId());
            if (result) {
                return Result.fail("普通卷数据库写入失败");
            }
            return Result.ok("数据库写入成功");
        }

        // 创建锁对象 , 现在使用redisson client来实现
//        SimpleRedisLock lock = new SimpleRedisLock("order:" + userId, stringRedisTemplate);
        RLock rlock = redissonClient.getLock("order:" + userId);

        // 尝试获取分布式锁， 这里使用redisson 失败不重试所以无参就行。
        boolean isLock = rlock.tryLock();

        // 判断锁获取是否成功
        if (!isLock){
            // 获取锁失败，代表一个用户ID在进行并发，这是不允许的，未获取锁的请求直接返回失败不进行重试。
            return Result.fail("一个人只允许下一单");
        }
        // 成功之后的逻辑
        try{
            //返回订单ID, 不再使用JVM自带的互斥锁
            proxy = (IVoucherOrderService)AopContext.currentProxy();
            return proxy.createSecKillVoucherOrder(voucher, userId);
        }finally {
            rlock.unlock(); // 无论里面内容操作成功还是失败，一定要释放锁
        }

    }


    // 创建新的单线程任务
    private static final ExecutorService SECKILL_ORDER_EXECUTOR = Executors.newSingleThreadExecutor();

    @PostConstruct // Spring提供的当前类被初始化之后执行的内容
    private void init(){
//        执行内容
        // 执行我们创建的线程任务
        SECKILL_ORDER_EXECUTOR.submit(new VoucherOrderHandler()); // 把任务直接跑起来
    }

    // 执行任务
    // 不再创建阻塞队列
//    private BlockingQueue<VoucherOrder> orderTasks = new ArrayBlockingQueue<>(1024*1024);
    private class VoucherOrderHandler implements Runnable{
        // 这个任务需要类被初始化的时候直接执行.
        String queueName = "stream.orders";

        @Override
        public void run() {
            // 持续执行任务
            while(true){
                try {
//                  1、获取消息队列中的订单信息 获取消息命令XREADGROUP group g1 c1 COUNT 1 BLOCK 2000 STREAMS stream.orders >
                    List<MapRecord<String, Object, Object>> mqList = stringRedisTemplate.opsForStream().read(
                            Consumer.from("g1", "c1"),
                            StreamReadOptions.empty().count(1).block(Duration.ofSeconds(2)),
                            StreamOffset.create(queueName, ReadOffset.lastConsumed())
                    );

//                  2、没消息进入下一次循环
                    if (mqList.isEmpty()){
                        continue;
                    }
//                  3、有消息创建订单
                    // 消息解析
                    MapRecord<String, Object, Object> record = mqList.get(0); // 获取信息原始格式
                    Map<Object, Object> values = record.getValue(); // 获取键值对map
                    VoucherOrder voucherOrder = BeanUtil.fillBeanWithMap(values, new VoucherOrder(), true);// 键值对map转换为对应bean格式
                    handleVoucherOrder(voucherOrder);
//                  4、XACK确认消息 SACK stream.orders g1 id
                    stringRedisTemplate.opsForStream().acknowledge(queueName,"g1",record.getId().toString());
                    log.debug("完成了一个订单");

                } catch (Exception e) {
                    log.error("处理订单异常：",e);
                    // 异常情况的处理，预计是读取pending-list中的内容，如果有消息则进行下单，如果没有这脱离循环。
                    while(true){
                        try {
//                  1、获取获取pending-list中的消息获取消息命令XREADGROUP group g1 c1 COUNT 1 2000 STREAMS stream.orders 0
                            List<MapRecord<String, Object, Object>> WmqList = stringRedisTemplate.opsForStream().read(
                                    Consumer.from("g1", "c1"),
                                    StreamReadOptions.empty().count(1),
                                    StreamOffset.create(queueName, ReadOffset.from("0"))
                            );

//                  2、没pending-list消息结束循环
                            if (WmqList.isEmpty()){
                                break;
                            }
//                  3、有消息创建订单
                            // 消息解析
                            MapRecord<String, Object, Object> record = WmqList.get(0); // 获取信息原始格式
                            Map<Object, Object> values = record.getValue(); // 获取键值对map
                            VoucherOrder voucherOrder = BeanUtil.fillBeanWithMap(values, new VoucherOrder(), true);// 键值对map转换为对应bean格式
                            handleVoucherOrder(voucherOrder);
//                  4、XACK确认消息 SACK stream.orders g1 id
                            stringRedisTemplate.opsForStream().acknowledge(queueName,"g1",record.getId().toString());
                            log.debug("从pending-list完成了一个订单");
                        } catch (Exception EX) {
                            log.error("pending-list异常处理：",e);
                            try {
                                Thread.sleep(50);
                            } catch (InterruptedException ex) {
                                throw new RuntimeException(ex);
                            }
                            // 异常情况的处理
                        }
                    }
                }
            }
        }
    }
    // 对于阻塞队列的处理逻辑
    private void handleVoucherOrder(VoucherOrder voucherOrder){
        Long userId = voucherOrder.getUserId();
        // 创建锁对象
        RLock rlock = redissonClient.getLock(LOCK_ORDER_Z +userId);

        boolean isLock = rlock.tryLock();
        if (!isLock){
            // 锁失败
            log.error("不允许重复下单");
        }
        try{
            // 扣减库存
        boolean success = seckillVoucherService.update(). // MybatisPlus写法，单表操作方便一些。
                setSql("stock = stock -1 ").
                eq("voucher_id", voucherOrder.getVoucherId()).gt("stock", 0).
                update();
        if (!success) {
            //扣减库存失败
            log.error("扣减库存失败");
        }

        // 创建订单
        addVoucherOrder(userId, voucherOrder.getVoucherId());
        }finally {
            rlock.unlock();
        }

    }

    public Result createSecKillVoucherOrder(Voucher voucher, Long userId) {
        long orderId = redisIdWorker.nextId(VOUCHER_ORDER_PRE_KEY);
        // 执行lua脚本
        Long result = stringRedisTemplate.execute(
                SECKILL_SCRIPT,
                Collections.EMPTY_LIST,
                voucher.getId().toString(),
                userId.toString(),
                orderId+"",
                LocalDateTime.now().toEpochSecond(ZoneOffset.UTC)+""
        );
        int r = result.intValue();
        if (r!=0){ // 不具备购买资格
            if (r==1){
                return Result.fail("库存不足");
            }
            if (r==2){
                return Result.fail("重复下单");
            }
            if (r==3){
                return Result.fail("活动还没开始");
            }
            if (r==4){
                return Result.fail("活动已经结束");
            }
        }
        // 保存信息至阻塞队列
//        orderTasks.add(newVoucherOrder); //  不再使用阻塞队列
        return Result.ok(orderId);
    }

//    @Transactional // 因为包含多个表的操作，最好还是加上事务管理。
//    public Result createSecKillVoucherOrder(Voucher voucher, Long userId) {
//
//        // 判断是否已存在
//        Integer userOrders = userOrders(userId, voucher.getId());
//
//        // 秒杀卷提前判断是否已经买过
//        if (userOrders > 0) {
//            return Result.fail("当前用户已经买过了");
//        }
//
//        // 下面是秒杀卷的逻辑
//        Long sqlVoucherId = voucher.getId();
//        // 判断秒杀是否开始
//        SeckillVoucher seckillVoucher = seckillVoucherService.getById(sqlVoucherId);
//        if (seckillVoucher.getBeginTime().isAfter(LocalDateTime.now())) {
//            // 秒杀还未开始
//            return Result.fail("秒杀还未开始呢");
//        }
//
//        // 判断秒杀是否已经结束
//        if (seckillVoucher.getEndTime().isBefore(LocalDateTime.now())) {
//            //秒杀已经结束
//            return Result.fail("秒杀已经结束");
//        }
//
//        //判断库存是否充足
//        if (seckillVoucher.getStock() < 1) {
//            //库存不足
//            return Result.fail("库存不足");
//        }
//        // 扣减库存
//        boolean success = seckillVoucherService.update(). // MybatisPlus写法，单表操作方便一些。
//                setSql("stock = stock -1 ").
//                eq("voucher_id", sqlVoucherId).gt("stock", 0).
//                update();
//        if (!success) {
//            //扣减库存失败
//            return Result.fail("扣减库存失败");
//        }
//
//        // 创建订单
//        addVoucherOrder(userId, voucher.getId());
//        return Result.ok("订单创建成功");
//
//    }

    public Boolean addVoucherOrder(Long userId, Long voucherId) {
        VoucherOrder newVoucherOrder = new VoucherOrder();
        newVoucherOrder.setId(redisIdWorker.nextId(VOUCHER_ORDER_PRE_KEY)); // 订单ID
        newVoucherOrder.setUserId(userId); // 用户ID
        newVoucherOrder.setVoucherId(voucherId); // 代金券ID
        // 添加加入Redis内容
        return save(newVoucherOrder);
    }

    /**
     * 查询该用户对当前优惠券下单数
     *
     * @param UserId
     * @param voucherId
     * @return
     */
    private Integer userOrders(Long UserId, Long voucherId) {
        return query().eq("user_id", UserId).eq("voucher_id", voucherId).count();
    }
}
