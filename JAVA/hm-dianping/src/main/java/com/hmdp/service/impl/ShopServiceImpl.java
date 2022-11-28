package com.hmdp.service.impl;

import cn.hutool.core.util.BooleanUtil;
import cn.hutool.core.util.StrUtil;
import cn.hutool.json.JSONObject;
import cn.hutool.json.JSONUtil;
import com.baomidou.mybatisplus.extension.conditions.query.QueryChainWrapper;
import com.baomidou.mybatisplus.extension.plugins.pagination.Page;
import com.hmdp.dto.Result;
import com.hmdp.entity.Shop;
import com.hmdp.mapper.ShopMapper;
import com.hmdp.service.IShopService;
import com.baomidou.mybatisplus.extension.service.impl.ServiceImpl;
import com.hmdp.utils.CacheClient;
import com.hmdp.utils.RedisData;
import com.hmdp.utils.SystemConstants;
import io.lettuce.core.GeoSearch;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.data.geo.Distance;
import org.springframework.data.geo.GeoResult;
import org.springframework.data.geo.GeoResults;
import org.springframework.data.redis.connection.RedisGeoCommands;
import org.springframework.data.redis.core.StringRedisTemplate;
import org.springframework.data.redis.domain.geo.GeoReference;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import javax.annotation.Resource;

import java.sql.Time;
import java.time.LocalDateTime;
import java.util.*;
import java.util.concurrent.TimeUnit;
import java.util.stream.Stream;

import static com.hmdp.Constants.RedisConstants.*;

/**
 * <p>
 * 服务实现类
 * </p>
 *
 * @author 虎哥
 * @since 2021-12-22
 */
@Service
public class ShopServiceImpl extends ServiceImpl<ShopMapper, Shop> implements IShopService {

    @Resource
    StringRedisTemplate stringRedisTemplate;

    @Autowired
    CacheClient cacheClient;

    @Override
    public Result queryById(Long id) throws InterruptedException {
        Shop shop = queryWithLogicExpire(id);
        if (shop == null) return Result.fail("服务器获取错误");
        return Result.ok(shop);
    }

    /**
     * 通过逻辑删除来解决缓存击穿问题
     *
     * @param id
     * @return
     */
    private Shop queryWithLogicExpire(Long id) throws InterruptedException {
        Shop shop = cacheClient.queryWithLogicExpire(CACHE_SHOP_Z, id, LOCK_KEY_Z, Shop.class, this::getById, 1L, TimeUnit.HOURS);
        return shop;
    }

    /**
     * 这是缓存穿透的实现存留
     */
    private Shop queryCacheThrouth(Long id) {
        Shop shop = cacheClient.queryWithPassThrough(CACHE_SHOP_Z, id, Shop.class, this::getById, 5L, TimeUnit.MINUTES);
        return shop;
    }

    @Override
    @Transactional // 事务管理注解，其中任何一步出错都会回滚之前的响应操作。
    public Result updateShop(Shop shop) {
        // 数据简单判断
        if (shop == null || shop.getId() == null) {
            return Result.fail("传入数据错误");
        }

        // 数据库更新
        Boolean result = updateById(shop);
        if (!result) {
            return Result.fail("更新数据库失败");
        }

        // 缓存，其实是缓存删除
        String key = CACHE_SHOP_Z + shop.getId();
        stringRedisTemplate.delete(key);

        return Result.ok();
    }

    @Override
    public Result queryShopByType(Integer typeId, Integer current, Double x, Double y) {

//        1、判断是否需要根据坐标查询
        if (x == null || y == null) {
            // 不需要坐标查询，根据类型分页查询数据库
            Page<Shop> page = query()
                    .eq("type_id", typeId)
                    .page(new Page<>(current, SystemConstants.DEFAULT_PAGE_SIZE));
            return Result.ok(page);
        }

//        2、计算分页参数 起始和结束点
        int from = (current - 1) * SystemConstants.DEFAULT_PAGE_SIZE;
        int end = current * SystemConstants.DEFAULT_PAGE_SIZE;

        String key = SHOP_GEO_KEY_Z + typeId;

//        3、查询redis、按照距离排序分页，结果shopId、distance GEOSEARCH BYLONLAT x y BYADIUS 10 WITHDISTANCE
        GeoResults<RedisGeoCommands.GeoLocation<String>> results = stringRedisTemplate.opsForGeo()
                .search(key,
                        GeoReference.fromCoordinate(x, y),
                        new Distance(5000), // 5公里以内,单位为米
                        RedisGeoCommands.
                                GeoSearchCommandArgs.
                                newGeoSearchArgs().
                                includeDistance().
                                limit(end)
                );
        if (results == null) return Result.ok(null);
        // 先获取数据结果
        List<GeoResult<RedisGeoCommands.GeoLocation<String>>> list = results.getContent();
        if (list.size()<from){
            return Result.ok(Collections.emptyList());
        }

        // 因为search方法只能获取从0到指定结束的数据（无法自动分页），所以我们只能自己来做逻辑分页
        Stream<GeoResult<RedisGeoCommands.GeoLocation<String>>> shopIdList = list.stream().skip(from);

        List<Long> ids = new ArrayList<>(end - from);
        // 为了方便之后的装配SHOP距离内容，先把查出来的id和距离数据存储在Map中
        Map<String, Distance> distanceMap = new HashMap<>(end - from);
        shopIdList.forEach(result -> {
            // 获取店铺ID
            String shopIdStr = result.getContent().getName();
            ids.add(Long.valueOf(shopIdStr));
            // 获取距离
            Distance distance = result.getDistance();
            distanceMap.put(shopIdStr, distance);
        });

        // 根据查出来的shopID 数据库查到对应的店铺信息。
        String idsStr = StrUtil.join(",", ids);
        List<Shop> shops = query().in("id", ids).last("ORDER BY FIELD(id," + idsStr + ")").list();

        shops.forEach(shop -> {
            shop.setDistance(distanceMap.get(shop.getId().toString()).getValue());
        });
        return Result.ok(shops);
    }

    // 获取锁
    private boolean tryLock(String key) {
        Boolean flag = stringRedisTemplate.opsForValue().setIfAbsent(key, "1", 10, TimeUnit.SECONDS); // 添加锁
        return BooleanUtil.isTrue(flag);
    }

    // 删除锁
    private void unLock(String key) {
        stringRedisTemplate.delete(key);
    }
}
