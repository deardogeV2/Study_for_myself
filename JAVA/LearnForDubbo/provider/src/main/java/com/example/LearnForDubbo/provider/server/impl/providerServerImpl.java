package com.example.LearnForDubbo.provider.server.impl;

import org.apache.dubbo.config.annotation.Service;
import org.springframework.context.annotation.ImportResource;
import service.providerServerRPC;

@Service
public class providerServerImpl implements providerServerRPC {

    public String TestSendMsg() throws InterruptedException {
        return "这是从服务提供者返回的数据（延时1秒）";
    }
}
