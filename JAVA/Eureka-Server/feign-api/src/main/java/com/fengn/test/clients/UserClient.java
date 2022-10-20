package com.fengn.test.clients;

import com.fengn.test.domain.User;
import com.fengn.test.fallbackFactory.UserClientFallbackFactory;
import org.springframework.cloud.openfeign.FeignClient;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PathVariable;

@FeignClient(value = "userService" , fallbackFactory = UserClientFallbackFactory.class)
public interface UserClient {

    // 这里定义了调用user服务的rpc接口链接

    @GetMapping("/user/{id}")
    User findByid(@PathVariable("id") Integer id) ;

}
