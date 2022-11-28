package com.hmdp.utils;

import cn.hutool.core.util.BooleanUtil;
import cn.hutool.core.util.StrUtil;
import cn.hutool.json.JSONObject;
import cn.hutool.json.JSONUtil;
import com.hmdp.entity.Shop;
import lombok.extern.slf4j.Slf4j;
import org.springframework.data.redis.core.StringRedisTemplate;
import org.springframework.stereotype.Component;

import javax.annotation.Resource;
import java.time.LocalDateTime;
import java.util.concurrent.TimeUnit;
import java.util.function.Function;

import static com.hmdp.Constants.RedisConstants.CACHE_SHOP_Z;
import static com.hmdp.Constants.RedisConstants.LOCK_KEY_Z;

@Slf4j
@Component
public class CacheClient {
    // 缓存工具类

    @Resource
    private StringRedisTemplate stringRedisTemplate;

    /**
     * 缓存写入方法
     * @param key
     * @param value
     * @param time
     * @param unit
     */
    public void set(String key, Object value, Long time, TimeUnit unit) {
        stringRedisTemplate.opsForValue().set(key, JSONUtil.toJsonStr(value), time, unit);
    }

    /**
     * 逻辑缓存写入方法
     * @param key
     * @param value
     * @param time
     * @param unit
     */
    public void setWithLogic(String key, Object value, Long time, TimeUnit unit) {
        // 写入逻辑过期
        RedisData redisData = new RedisData();
        redisData.setData(value);
        redisData.setExpireTime(LocalDateTime.now().plusSeconds(unit.toSeconds(time)));

        stringRedisTemplate.opsForValue().set(key, JSONUtil.toJsonStr(redisData));
    }

    /**
     * 缓存击穿查询工具
     * @param keyPrefix
     * @param id
     * @param time
     * @param unit
     * @param type
     * @param dbFallback
     * @return
     * @param <R>
     * @param <ID>
     */
    public <R, ID> R queryWithPassThrough(
            String keyPrefix, ID id, Class<R> type, Function<ID, R> dbFallback, Long time, TimeUnit unit) {
        String key = keyPrefix + id.toString();

        // 1、从redis查询商品缓存，这回我们使用string来实现
        String json = stringRedisTemplate.opsForValue().get(key);
        // 2、 判断是否存在
        if (StrUtil.isNotBlank(json)) {
            // 3、存在-直接返回
            return JSONUtil.toBean(json, type);
        }
        // 添加空书卷缓存的判断逻辑
        if (json != null) {
            return null;
        }
        // 4、使用泛型让调用方传入查询数据库的方法再进行调用。
        R r = dbFallback.apply(id);

        if (r == null) {
            // 空对象缓存
            stringRedisTemplate.opsForValue().set(key, "", 3, TimeUnit.MINUTES);
            return null;
        }
        // 5、数据写入Redis
        String rJson = JSONUtil.toJsonStr(r);
        this.set(key,rJson,time,unit);

        return r;
    }


    public <R, ID> R queryWithLogicExpire(String keyPrefix, ID id,String lockKey,  Class<R> type, Function<ID, R> dbFallback, Long time, TimeUnit unit) throws InterruptedException {
        String key = keyPrefix + id.toString();

        // 1、从redis查询商品缓存，这回我们使用string来实现
        String cacheShopJson = stringRedisTemplate.opsForValue().get(key);

        // 2、 判断是否存在
        if (StrUtil.isBlank(cacheShopJson)) {
            R Objectr;
            // 3、如果缓存不存在，则走缓存逻辑
            boolean isLock = tryLock(lockKey + id);
            while (true) {  // 通过while的形式来实现锁
                if (isLock) {
                    //获取锁之后再次判断一下有没有缓存有没有数据
                    cacheShopJson = stringRedisTemplate.opsForValue().get(key);
                    if (cacheShopJson!=null){
                        // 缓存此刻缓存突然有了数据，放弃锁，往下走
                        unLock(lockKey + id);
                        break;
                    }
                    // 此时缓存任然没有数据，进入互斥锁更新redis逻辑
                    Objectr = dbFallback.apply(id);
                    if (Objectr == null) {
                        // 如果真的数据库都没有，只能返回报错
                        this.setWithLogic(key,"",3L,TimeUnit.MINUTES);
                        return null;
                    }

                    this.setWithLogic(key, Objectr,time,unit);
                    // 释放互斥锁
                    unLock(lockKey + id);
                    // 6、数据返回
                    return Objectr;
                } else {
                    Thread.sleep(50);
                    isLock = tryLock(lockKey + id);
                }
            }
        }

        // 3、缓存存在，判断数据是否过期
        RedisData redisData = JSONUtil.toBean(cacheShopJson, RedisData.class);
        // 这里需要注意，多一层的redisData又要使用一次Json类型转实例类型。和上面这个JSON字符串类型转实例类型不一样。
        R r = JSONUtil.toBean((JSONObject) redisData.getData(),type);
        if (redisData.getExpireTime().isAfter(LocalDateTime.now())) {
            return r;
        } else { // 缓存已经过期
            // 缓存互斥锁重建
            // 缓存重建内容添加互斥锁,实在不想递归，所以就先互斥了。
            boolean isLock = tryLock(lockKey + id);
            while (true) {  // 通过while的形式来实现锁
                if (isLock) {
                    //获取锁之后再次判断一下有没有缓存有没有数据
                    cacheShopJson = stringRedisTemplate.opsForValue().get(key);
                    redisData = JSONUtil.toBean(cacheShopJson, RedisData.class);
                    R Objectr = JSONUtil.toBean((JSONObject) redisData.getData(), type);

                    if (redisData != null && redisData.getExpireTime().isAfter(LocalDateTime.now())) {
                        // 释放互斥锁
                        unLock(lockKey + id);
                        // 返回可用数据
                        return Objectr;
                    }
                    // 4、数据任然过期，进入互斥锁更新redis逻辑
                    Objectr = dbFallback.apply(id);
                    if (Objectr == null) {
                        // 空对象缓存 空对象缓存时间只有3分钟
                        this.setWithLogic(key,"",3L,TimeUnit.MINUTES);
                        return null;
                    }
                    // 5、数据写入Redis，但此时我们不再计算时间，并且写入的是redisData

                    this.setWithLogic(key, Objectr,time,unit);

                    // 释放互斥锁
                    unLock(lockKey + id);
                    // 6、数据返回
                    return Objectr;
                } else {
                    Thread.sleep(50);
                    isLock = tryLock(lockKey + id);
                }
            }
        }
    }

    private boolean tryLock(String key) {
        Boolean flag = stringRedisTemplate.opsForValue().setIfAbsent(key, "1", 10, TimeUnit.SECONDS); // 添加锁
        return BooleanUtil.isTrue(flag);
    }

    // 删除锁
    private void unLock(String key) {
        stringRedisTemplate.delete(key);
    }

}
