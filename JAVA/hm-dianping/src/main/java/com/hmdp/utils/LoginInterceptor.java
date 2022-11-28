package com.hmdp.utils;

import cn.hutool.core.bean.BeanUtil;
import cn.hutool.core.util.StrUtil;
import com.hmdp.dto.UserDTO;
import com.hmdp.entity.User;
import lombok.extern.slf4j.Slf4j;
import org.springframework.data.redis.core.StringRedisTemplate;
import org.springframework.web.servlet.HandlerInterceptor;

import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpServletResponse;
import javax.servlet.http.HttpSession;
import java.util.Map;
import java.util.concurrent.TimeUnit;


@Slf4j
public class LoginInterceptor implements HandlerInterceptor {
    // 拦截器需要去实现接口HandlerInterceptor

    // 注入StringRedisTemplate，因为不是Spring的配置类，所以不能使用@Autowired注入
    // 我们写构造方法注入，让它在被创建的时候进行初始化

    @Override
    public void afterCompletion(HttpServletRequest request, HttpServletResponse response, Object handler, Exception ex) throws Exception {
        // 传输后的操作
        // 移除用户
        UserHolder.removeUser();
    }

    @Override
    public boolean preHandle(HttpServletRequest request, HttpServletResponse response, Object handler) throws Exception {
        // 判断是否有用户就行了
        if(UserHolder.getUser()==null){
            response.setStatus(401);
            log.debug("有一个未登录的请求被拦截了。");
            return false;
        }
        //6、放行
        return true;
    }
}
