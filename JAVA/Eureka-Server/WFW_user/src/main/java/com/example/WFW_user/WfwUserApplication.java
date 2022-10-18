package com.example.WFW_user;

import org.springframework.boot.SpringApplication;
import org.springframework.boot.autoconfigure.SpringBootApplication;
import org.springframework.cloud.client.loadbalancer.LoadBalanced;
import org.springframework.context.annotation.Bean;
import org.springframework.web.client.RestTemplate;

//@EnableEurekaClient // 需要设置为eureka的客户端
@SpringBootApplication
public class WfwUserApplication {

	public static void main(String[] args) {
		SpringApplication.run(WfwUserApplication.class, args);
	}

	@Bean
	@LoadBalanced // 对于创建的RestTemplate添加负载均衡注解
	public RestTemplate restTemplate(){
		return new RestTemplate();
	}

}
