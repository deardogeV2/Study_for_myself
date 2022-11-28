package com.hmdp.service;

import com.hmdp.dto.Result;
import com.hmdp.entity.Follow;
import com.baomidou.mybatisplus.extension.service.IService;

/**
 * <p>
 *  服务类
 * </p>
 *
 * @author 虎哥
 * @since 2021-12-22
 */
public interface IFollowService extends IService<Follow> {

    Result isFollow(Long id);

    Result follow(Long followUserId, Boolean isFollow);

    /**
     * 查看用户和登录用户的共同关注用户列表
     * @param lockUserId
     * @return
     */
    Result followCommonsTogether(Long lockUserId);
}
