package com.hmdp.service;

import com.hmdp.dto.Result;
import com.hmdp.entity.Blog;
import com.baomidou.mybatisplus.extension.service.IService;

/**
 * <p>
 *  服务类
 * </p>
 *
 * @author 虎哥
 * @since 2021-12-22
 */
public interface IBlogService extends IService<Blog> {

    Result queryBlogById(Long id);

    Result queryHotBlog(Integer current);

    Result likeBlog(Long id);

    Result queryBlogLikesById(Long id);

    Result queryBlogByUserId(Long userId, Integer current);

    /**
     * 保存一份新的笔记
     * @param blog
     * @return
     */
    Result saveBlog(Blog blog);

    /**
     * 分页查询关注列表关注用户最新发布的笔记
     * @param max
     * @param offset
     * @return
     */
    Result queryFollowComments(Long max, Integer offset);
}
