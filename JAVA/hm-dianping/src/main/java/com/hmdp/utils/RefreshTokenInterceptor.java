package com.hmdp.utils;

import cn.hutool.core.bean.BeanUtil;
import cn.hutool.core.util.StrUtil;
import com.hmdp.dto.UserDTO;
import org.springframework.data.redis.core.StringRedisTemplate;
import org.springframework.web.servlet.HandlerInterceptor;

import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpServletResponse;
import java.util.Map;
import java.util.concurrent.TimeUnit;

public class RefreshTokenInterceptor implements HandlerInterceptor {
    // 拦截器需要去实现接口HandlerInterceptor

    // 注入StringRedisTemplate，因为不是Spring的配置类，所以不能使用@Autowired注入
    // 我们写构造方法注入，让它在被创建的时候进行初始化
    private StringRedisTemplate stringRedisTemplate;
    public RefreshTokenInterceptor(StringRedisTemplate TstringRedisTemplate){
        this.stringRedisTemplate = TstringRedisTemplate;
    }

    @Override
    public void afterCompletion(HttpServletRequest request, HttpServletResponse response, Object handler, Exception ex) throws Exception {
        UserHolder.removeUser();
    }

    @Override
    public boolean preHandle(HttpServletRequest request, HttpServletResponse response, Object handler) throws Exception {

        // 1、获取token查看是否有token。
        String allToken = request.getHeader("authorization");
        if(StrUtil.isBlank(allToken)){
            // 没有token直接放行
            return true;}

        // 2、基于token，去redis中获取用户（注意格式装换）
        Map<Object, Object> userMap = stringRedisTemplate.opsForHash().entries(allToken); // 合格方法获取完整的Map


        // 3、判断用户是否存在
        if (userMap == null){
            // 没有用户直接放行
            return true;
        }
        // 使用工具把MAP类型转回UserDTO类型
        UserDTO user = BeanUtil.fillBeanWithMap(userMap,new UserDTO(),false);

        // 5、存在，保存用户信息到ThreadLocal，ThreadLocal是一个线程中的一个静态变量。这里直接实现
        UserHolder.saveUser(user);

        // 5、添加刷新token有效期动作
        stringRedisTemplate.expire(allToken,30, TimeUnit.MINUTES);

        //6、放行
        return true;
    }
}
