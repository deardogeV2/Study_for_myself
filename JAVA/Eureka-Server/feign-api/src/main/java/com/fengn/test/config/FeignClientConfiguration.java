package com.fengn.test.config;

import com.fengn.test.fallbackFactory.UserClientFallbackFactory;
import feign.Logger;
import org.springframework.context.annotation.Bean;
import org.springframework.context.annotation.Configuration;
import org.springframework.stereotype.Component;

public class FeignClientConfiguration {
    @Bean
    public Logger.Level feignLogLevel(){
        return Logger.Level.FULL;
    }

    @Bean // 这里
    public UserClientFallbackFactory userClientFallbackFactory(){return new UserClientFallbackFactory();}
}
