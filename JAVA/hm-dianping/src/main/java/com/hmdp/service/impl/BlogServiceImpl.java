package com.hmdp.service.impl;

import cn.hutool.core.bean.BeanUtil;
import cn.hutool.core.util.BooleanUtil;
import cn.hutool.core.util.StrUtil;
import com.baomidou.mybatisplus.extension.plugins.pagination.Page;
import com.hmdp.dto.Result;
import com.hmdp.dto.ScrollResult;
import com.hmdp.dto.UserDTO;
import com.hmdp.entity.Blog;
import com.hmdp.entity.Follow;
import com.hmdp.entity.User;
import com.hmdp.mapper.BlogMapper;
import com.hmdp.service.IBlogService;
import com.baomidou.mybatisplus.extension.service.impl.ServiceImpl;
import com.hmdp.service.IFollowService;
import com.hmdp.utils.SystemConstants;
import com.hmdp.utils.UserHolder;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.data.redis.core.StringRedisTemplate;
import org.springframework.data.redis.core.ZSetOperations;
import org.springframework.stereotype.Service;

import javax.annotation.Resource;
import java.util.ArrayList;
import java.util.Collections;
import java.util.List;
import java.util.Set;
import java.util.stream.Collectors;

import static com.hmdp.Constants.RedisConstants.FEED_KEY_Z;

/**
 * <p>
 * 服务实现类
 * </p>
 *
 * @author 虎哥
 * @since 2021-12-22
 */
@Service
public class BlogServiceImpl extends ServiceImpl<BlogMapper, Blog> implements IBlogService {

    @Autowired
    UserServiceImpl userService;

    @Resource
    StringRedisTemplate stringRedisTemplate;

    @Override
    public Result queryBlogById(Long id) {

        Blog blog = getById(id);
        if (blog == null) {
            return Result.fail("笔记不存在");
        }
        // 查询blog的作者信息
        queryBlogUser(blog);

        // 查询blog是否被点赞，redis
        isBlogLiked(blog);
        return Result.ok(blog);
    }

    private void isBlogLiked(Blog blog) {
        UserDTO user = UserHolder.getUser();
        if (user == null) {
            return;
        }
        Long userId = UserHolder.getUser().getId();
        String key = "blog:liked:" + blog.getId();
        Double isLiked = stringRedisTemplate.opsForZSet().score(key, userId.toString());
        blog.setIsLike(isLiked != null);
    }

    @Override
    public Result queryHotBlog(Integer current) {
        // 根据用户查询
        Page<Blog> page = query()
                .orderByDesc("liked")
                .page(new Page<>(current, SystemConstants.MAX_PAGE_SIZE));
        // 获取当前页数据
        List<Blog> records = page.getRecords();
        // 查询用户
        records.forEach(blog -> {
            this.queryBlogUser(blog);
            this.isBlogLiked(blog);
        });
        return Result.ok(records);
    }

    /**
     * 当前用户是否已经点赞
     *
     * @param id
     * @return
     */
    @Override
    public Result likeBlog(Long id) {
        // 1、 获取登录用户
        Long userId = UserHolder.getUser().getId();
        String key = "blog:liked:" + id;
        Double isMember = stringRedisTemplate.opsForZSet().score(key, userId.toString());
        // 2、 判断当前登录用户是否已经点赞
        if (isMember == null) {
            // 3、未点赞，可以点赞
            // 3.1 数据库点赞数+1
            boolean isSuccess = update().setSql("liked = liked + 1").eq("id", id).update();
            // 3.2 把用户添加Redis的set集合中
            if (isSuccess) {
                // 改为使用sortSet
                stringRedisTemplate.opsForZSet().add(key, userId.toString(), System.currentTimeMillis());
            }
        } else {
            // 4、已点赞，取消点赞
            // 4.1 数据库点赞数-1
            boolean isSuccess = update().setSql("liked = liked - 1").eq("id", id).update();
            // 4.2 把用户从Redis的set集合中移除
            if (isSuccess) {
                stringRedisTemplate.opsForZSet().remove(key, userId.toString());
            }
        }
        return Result.ok();
    }

    @Override
    public Result queryBlogLikesById(Long id) {
        // 1、查询top5的点赞用户 zrange key 0 0
        String key = "blog:liked:" + id;
        Set<String> Top5Id = stringRedisTemplate.opsForZSet().range(key, 0, 4);

        if (Top5Id.isEmpty()) {
            return Result.ok(Collections.emptyList());
        }

        // 2、通过用户ID去封装用户类型拼装成List类型
        // 这里使用了map映射的方法 还有collect的封装方法，具体实现可以下来在学习。
        List<Long> ids = Top5Id.stream().map(Long::valueOf).collect(Collectors.toList());
        String idStr = StrUtil.join(",", ids);
        // 这里不能直接使用Mybatis，因为默认是使用in的操作，而in的操作是不关乎顺序的，所以我们必须重写 order by field(id, x,y)
        List<UserDTO> userDTOS = userService.
                query().
                in("id", ids).last("ORDER BY FIELD(id," + idStr + ")").
                list().
                stream().
                map(user ->
                        BeanUtil.copyProperties(user, UserDTO.class)).
                collect(Collectors.toList());

        // 因为返回的类型是UserDTO所以需要转换
        return Result.ok(userDTOS);
    }

    /**
     * 根据用户Id分页查询用户的笔记
     *
     * @param userId
     * @param current
     * @return
     */
    @Override
    public Result queryBlogByUserId(Long userId, Integer current) {
        // 根据用户查询笔记-mybatisPlus实现
        Page<Blog> page = query().eq("user_id", userId).page(new Page<>(current, SystemConstants.MAX_PAGE_SIZE));

        // 获取当前页数据
        List<Blog> records = page.getRecords();
        return Result.ok(records);
    }

    @Resource
    IFollowService followService;

    @Override
    public Result saveBlog(Blog blog) {
        // 获取登录用户
        UserDTO user = UserHolder.getUser();
        blog.setUserId(user.getId());
        // 保存探店博文
        boolean isSuccess = save(blog);
        if (!isSuccess) {
            return Result.fail("笔记发布失败");
        }
        // 查询作者的粉丝
        List<Follow> users = followService.query().eq("follow_user_id", user.getId().toString()).list();
        //推送消息
        for (Follow follow : users) {
            // 获取粉丝ID
            Long fansId = follow.getUserId();
            String key = FEED_KEY_Z + fansId;
            stringRedisTemplate.opsForZSet().add(key, blog.getId().toString(), System.currentTimeMillis());
        }
        // 返回id
        return Result.ok(blog.getId());
    }

    @Resource
    IBlogService blogService;
    @Override
    public Result queryFollowComments(Long max, Integer offset) {
        // 查询个人收件箱里面的所有笔记
        UserDTO user = UserHolder.getUser();
        // 查询收件箱
        String key = FEED_KEY_Z + user.getId().toString();
        Set<ZSetOperations.TypedTuple<String>> typedTuples = stringRedisTemplate.opsForZSet().reverseRangeByScoreWithScores(
                key, 0L,
                max,
                offset,
                3); //就先一次查3条把。
        if (typedTuples == null || typedTuples.isEmpty()) {
            return Result.ok();
        }

        // 解析数据：blogId、最小时间戳scope、最小时间戳的个数
        List<Long> blogIds = new ArrayList<>(typedTuples.size());
        long minTime = 0;
        int nextOffset = 1;

        for (ZSetOperations.TypedTuple<String> typedTuple : typedTuples) { // 循环取内容
            blogIds.add(Long.valueOf(typedTuple.getValue()));

            long time = typedTuple.getScore().longValue();
            if (minTime == time) {
                nextOffset++;
            } else {
                minTime = time;
                nextOffset = 1;
            }
        }
        // 根据去数据库查询内容Blog集合，还是要考虑mybatisPlus默认的in方法在数据库中无序查找的情况。
        String idStr = StrUtil.join(",", blogIds);
        List<Blog> blogs = blogService.query().in("id", blogIds).last("ORDER BY FIELD(id," + idStr + ")").list();
        for (Blog blog:blogs){
            // 查询blog的作者信息
            queryBlogUser(blog);

            // 查询blog是否被点赞，redis
            isBlogLiked(blog);
        }

        // 封装并返回
        ScrollResult scrollResult = new ScrollResult();
        scrollResult.setList(blogs);
        scrollResult.setMinTime(minTime);
        scrollResult.setOffset(nextOffset);

        return Result.ok(scrollResult);
    }

    /**
     * 根据blog直接查询作者用户。
     *
     * @param blog
     */
    private void queryBlogUser(Blog blog) {
        Long userId = blog.getUserId();
        User user = userService.getById(userId);
        blog.setName(user.getNickName());
        blog.setIcon(user.getIcon());
    }
}
