package com.test.service.impl;

import com.test.service.UserService;
import org.apache.dubbo.config.annotation.Service;

//@Service // 使用这个标识它是一个服务bean类
// 现在我们不需要放在Spring的容器中了，所以也不需要声明是bean类了


@Service // 换成另一个Service，但是这个Service是Dubbo的Service，将这个类提供的方法(服务)，对外发布。将访问的地址、ip、端口放到注册中心。
public class UserServiceImpl implements UserService {
    @Override
    public void testService() {
        System.out.println("测试服务实现类");
    }
}
