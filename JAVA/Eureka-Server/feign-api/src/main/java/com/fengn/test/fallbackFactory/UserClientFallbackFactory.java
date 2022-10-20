package com.fengn.test.fallbackFactory;

import com.fengn.test.clients.UserClient;
import com.fengn.test.domain.User;
import feign.hystrix.FallbackFactory;
import lombok.extern.slf4j.Slf4j;

@Slf4j
public class UserClientFallbackFactory implements FallbackFactory<UserClient> {
    // 这里返回的也是UserClient，实际上就是返回一个新的UserClient接口
    @Override
    public UserClient create(Throwable throwable) {
        return new UserClient() { // 这里实际上就是当失败之后重新调用这个新UserClient中的兜底方法
            @Override
            public User findByid(Integer id) { // 兜底方法的定义，返回值保持一致。
                System.out.println("查询用户ID失败了！！" + throwable);
                return new User();
            }
        };
    }
}
