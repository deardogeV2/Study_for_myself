package com.example.LearnForDubbo.consumers;

import org.apache.dubbo.config.annotation.DubboReference;
import org.apache.dubbo.config.spring.context.annotation.EnableDubbo;
import org.springframework.boot.SpringApplication;
import org.springframework.boot.autoconfigure.SpringBootApplication;

@SpringBootApplication
@EnableDubbo
public class consumersApplication {

    public static void main(String[] args) {

        SpringApplication.run(consumersApplication.class, args);
    }
}
