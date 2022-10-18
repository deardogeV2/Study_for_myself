package com.fengn.test.config;

import feign.Logger;
import org.springframework.context.annotation.Bean;

public class FeignClientConfiguration {
    @Bean
    public Logger.Level feignLogLevel(){
        return Logger.Level.FULL;
    }
}
