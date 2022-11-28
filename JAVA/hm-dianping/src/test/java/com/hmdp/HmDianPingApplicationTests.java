package com.hmdp;

import cn.hutool.json.JSONUtil;
import com.hmdp.entity.Shop;
import com.hmdp.service.IShopService;
import com.hmdp.service.impl.ShopServiceImpl;
import com.hmdp.utils.RedisData;
import com.hmdp.utils.RedisIdWorker;
import org.junit.jupiter.api.Test;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.boot.test.context.SpringBootTest;
import org.springframework.data.geo.Point;
import org.springframework.data.redis.connection.RedisGeoCommands;
import org.springframework.data.redis.core.StringRedisTemplate;

import javax.annotation.Resource;
import java.time.LocalDateTime;
import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;
import java.util.Map;
import java.util.concurrent.CountDownLatch;
import java.util.concurrent.ExecutorService;
import java.util.concurrent.Executors;
import java.util.stream.Collectors;

import static com.hmdp.Constants.RedisConstants.CACHE_SHOP_Z;

@SpringBootTest
class HmDianPingApplicationTests {

    @Autowired
    ShopServiceImpl shopService;

    @Resource
    StringRedisTemplate stringRedisTemplate;

    @Resource
    private RedisIdWorker redisIdWorker;

    private ExecutorService es = Executors.newFixedThreadPool(500);

    @Test
    void testIdWork() throws InterruptedException {
        CountDownLatch countDownLatch = new CountDownLatch(300);
        Runnable task = () -> {
            for (int i = 0; i < 100; i++) {
                long id = redisIdWorker.nextId("order");
                System.out.println(id);
            }
            countDownLatch.countDown();
        };

        long begin = System.currentTimeMillis();
        for (int i = 0; i < 300; i++) {
            es.submit(task);
        }
        countDownLatch.await();
        long over = System.currentTimeMillis();
        System.out.println("time=" + (over - begin));
    }

    @Test
    public void saveShop2Redis() throws InterruptedException {
        Long id = 1L;

        Shop shop = shopService.getById(id);

        RedisData redisData = new RedisData();
        redisData.setData(shop);
        redisData.setExpireTime(LocalDateTime.now().plusSeconds(3600));

        stringRedisTemplate.opsForValue().set(CACHE_SHOP_Z + id, JSONUtil.toJsonStr(redisData));
    }

    @Test
    public void loadShopData() {
        // 查询店铺信息
        List<Shop> list = shopService.list();
        Map<Long, List<Shop>> shopMaps = new HashMap<>();

        // 使用 collect 收集里面的分组方法
        shopMaps = list.stream().collect(Collectors.groupingBy(Shop::getTypeId));

        // 分批完成写入Redis
        for (Map.Entry<Long, List<Shop>> shopmap : shopMaps.entrySet()) {

            Long key = shopmap.getKey(); // key就是类型ID

            String typeKey = "GEO:shop:type:" + key;

            List<Shop> value = shopmap.getValue(); // value就是店铺ID

//            for (Shop shop:value){ // 写入
//                stringRedisTemplate.opsForGeo().add(typeKey,new Point(shop.getX(),shop.getY()),shop.getId().toString());
//            }

            // 这里把所有的店铺转换成 RedisGeoCommands.GeoLocation<String> 列表 方便一次性传入。节约时间。
            List<RedisGeoCommands.GeoLocation<String>> locations = new ArrayList<>(value.size());
            for (Shop shop:value){
                locations.add(new RedisGeoCommands.GeoLocation<>(
                        shop.getId().toString(),
                        new Point(shop.getX(),
                                shop.getY())));
            }
            // 把数据一次性全部导入
            stringRedisTemplate.opsForGeo().add(typeKey,locations);
        }
    }


}
