package com.example.WFW_order;

import com.fengn.test.clients.UserClient;
import com.fengn.test.config.FeignClientConfiguration;
import org.springframework.boot.SpringApplication;
import org.springframework.boot.autoconfigure.SpringBootApplication;
import org.springframework.cloud.client.loadbalancer.LoadBalanced;
import org.springframework.cloud.openfeign.EnableFeignClients;
import org.springframework.context.annotation.Bean;
import org.springframework.web.client.RestTemplate;

//@EnableEurekaClient
@SpringBootApplication
@EnableFeignClients(clients = {UserClient.class}, defaultConfiguration = FeignClientConfiguration.class)
public class WfwOrderApplication {

    public static void main(String[] args) {
        SpringApplication.run(WfwOrderApplication.class, args);
    }

    @Bean
    @LoadBalanced // 对于创建的RestTemplate添加负载均衡注解
    public RestTemplate restTemplate() {
        return new RestTemplate();
    }

}
