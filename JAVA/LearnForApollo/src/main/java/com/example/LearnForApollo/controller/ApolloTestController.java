package com.example.LearnForApollo.controller;

import com.ctrip.framework.apollo.Config;
import com.ctrip.framework.apollo.ConfigService;
import org.springframework.stereotype.Controller;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RestController;

@RestController
@RequestMapping("Apollo")
public class ApolloTestController {

    @GetMapping("testGetProperties")
    public void testGetProperties() throws InterruptedException {

        Config config = ConfigService.getAppConfig();
        // 获取配置信息，第一个参数：配置的key，第二个参数
        String value = config.getProperty("app.name",null);

        String getProperties = "";
        Integer times = 0;
        while(true){
            Thread.sleep(5000);
            System.out.println(getProperties);
            System.out.println("这是第"+times+"次输出");
            times+=1;
        }
    }

}
