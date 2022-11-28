package com.hmdp.service.impl;

import cn.hutool.core.util.StrUtil;
import cn.hutool.json.JSONUtil;
import com.hmdp.dto.Result;
import com.hmdp.entity.ShopType;
import com.hmdp.mapper.ShopTypeMapper;
import com.hmdp.service.IShopTypeService;
import com.baomidou.mybatisplus.extension.service.impl.ServiceImpl;
import com.hmdp.utils.UserHolder;
import javafx.scene.input.DataFormat;
import org.springframework.data.redis.core.StringRedisTemplate;
import org.springframework.stereotype.Service;

import javax.annotation.Resource;
import java.time.LocalDateTime;
import java.time.format.DateTimeFormatter;
import java.util.List;
import java.util.concurrent.TimeUnit;

import static com.hmdp.Constants.RedisConstants.SHOP_TYPE_LIST_Z;

/**
 * <p>
 *  服务实现类
 * </p>
 *
 * @author 虎哥
 * @since 2021-12-22
 */
@Service
public class ShopTypeServiceImpl extends ServiceImpl<ShopTypeMapper, ShopType> implements IShopTypeService {

    @Resource
    StringRedisTemplate stringRedisTemplate;

    @Override
    public Result getShopTypeList() {

        // 每次获取商品类型接口请求的时候上传一次数据给到HLL
        String uvKey = "UV:" + LocalDateTime.now().format(DateTimeFormatter.ofPattern("yyyy/MM"));
        String uvValue = UserHolder.getUser().getId().toString();
        stringRedisTemplate.opsForHyperLogLog().add(uvKey,uvValue);

        String shopTypeCache = stringRedisTemplate.opsForValue().get(SHOP_TYPE_LIST_Z);
        if (StrUtil.isNotBlank(shopTypeCache)){
            List<ShopType> shopType = JSONUtil.toList(shopTypeCache,ShopType.class);
            return Result.ok(shopType);
        }
        List<ShopType> typeList = query().orderByAsc("sort").list();

        if (typeList == null || typeList.isEmpty()){
            return Result.fail("返回为空");
        }
        String newCache = JSONUtil.toJsonStr(typeList);
        stringRedisTemplate.opsForValue().set(SHOP_TYPE_LIST_Z,newCache,30, TimeUnit.MINUTES);

        return Result.ok(typeList);
    }
}
