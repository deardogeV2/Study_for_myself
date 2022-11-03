package com.example.LearnForDubbo.consumers;

import org.apache.dubbo.config.annotation.DubboReference;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.RestController;
import service.providerServerRPC;

@RestController
public class controller {

//    @DubboReference
    @Autowired // 如果你要使用这个注解，你得自己把providerServerRPC 声明为一个Bean。
    // 而且注解开发不好声明这个bean所以最好是声明在配置XML中
    private providerServerRPC providerServer;

    @GetMapping("/controller")
    public String test() throws InterruptedException {
        System.out.println("准备获取提供者数据");

        String out = providerServer.TestSendMsg();

        System.out.println("获取完成服务结束");
        return out;
    }
}
