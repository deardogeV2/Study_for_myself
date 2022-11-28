package com.hmdp.service.impl;

import cn.hutool.core.bean.BeanUtil;
import com.baomidou.mybatisplus.core.conditions.query.QueryWrapper;
import com.hmdp.dto.Result;
import com.hmdp.dto.UserDTO;
import com.hmdp.entity.Follow;
import com.hmdp.mapper.FollowMapper;
import com.hmdp.service.IFollowService;
import com.baomidou.mybatisplus.extension.service.impl.ServiceImpl;
import com.hmdp.service.IUserService;
import com.hmdp.utils.UserHolder;
import org.springframework.data.redis.core.StringRedisTemplate;
import org.springframework.stereotype.Service;

import javax.annotation.Resource;
import java.util.Collections;
import java.util.List;
import java.util.Set;
import java.util.stream.Collectors;

/**
 * <p>
 * 服务实现类
 * </p>
 *
 * @author 虎哥
 * @since 2021-12-22
 */
@Service
public class FollowServiceImpl extends ServiceImpl<FollowMapper, Follow> implements IFollowService {

    @Resource
    StringRedisTemplate stringRedisTemplate;
    @Resource
    IUserService userService;


    @Override
    public Result isFollow(Long followUserId) {
        // 1、获取登录用户
        Long userId = UserHolder.getUser().getId();
        // 2、判断当前用户和关注用户是否有关注关系。
        Integer count = query().eq("user_id", userId).eq("follow_user_id", followUserId).count();
        return Result.ok(count>0);
    }

    @Override
    public Result follow(Long followUserId, Boolean isFollow) {
        // 1、获取登录用户
        Long userId = UserHolder.getUser().getId();
        String key = "follows:" + userId;

        // 2、判断关注还是取关
        if (isFollow) {
            // 关注
            Follow follow = new Follow();
            follow.setUserId(userId);
            follow.setFollowUserId(followUserId);

            boolean isScuess = save(follow);
            if (isScuess){
                // 同步Redis数据修改
                stringRedisTemplate.opsForSet().add(key,followUserId.toString());
            }
        } else {
            // 取关
            boolean isScuess = remove(new QueryWrapper<Follow>().
                    eq("user_id", userId).
                    eq("follow_user_id", followUserId));
            if (isScuess){
                stringRedisTemplate.opsForSet().remove(key,followUserId.toString());
            }
        }
        return Result.ok();
    }

    @Override
    public Result followCommonsTogether(Long lockUserId) {
        // 获取当前用户
        Long userId = UserHolder.getUser().getId();
        String key1 = "follows:"+userId;
        String key2 = "follows:"+lockUserId;

        // 求交集
        Set<String> followsTogetherUserIds = stringRedisTemplate.opsForSet().intersect(key1, key2);
        if (followsTogetherUserIds==null || followsTogetherUserIds.isEmpty()){
            return Result.ok(Collections.emptyList());
        }
        // 解析ID 通过stream流 map映射为Long型之后装在List中
        List<Long> userIds = followsTogetherUserIds.stream().map(Long::valueOf).collect(Collectors.toList());
        // 查询
        List<UserDTO> userDtoList = userService.listByIds(userIds).stream().map(user -> BeanUtil.copyProperties(user, UserDTO.class)).collect(Collectors.toList());

        return Result.ok(userDtoList);
    }
}
