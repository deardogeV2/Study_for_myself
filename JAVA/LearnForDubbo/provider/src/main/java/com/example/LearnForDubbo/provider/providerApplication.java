package com.example.LearnForDubbo.provider;

import org.apache.dubbo.config.spring.context.annotation.EnableDubbo;
import org.springframework.boot.SpringApplication;
import org.springframework.boot.autoconfigure.SpringBootApplication;

@SpringBootApplication
@EnableDubbo
public class providerApplication {
    public static void main(String[] args) {
        SpringApplication.run(providerApplication.class, args);
    }
}


