package com.example.LearnForDsl.config;

import org.springframework.amqp.core.*;
import org.springframework.amqp.rabbit.connection.CachingConnectionFactory;
import org.springframework.amqp.rabbit.core.RabbitAdmin;
import org.springframework.amqp.rabbit.core.RabbitTemplate;
import org.springframework.context.annotation.Bean;
import org.springframework.context.annotation.Configuration;

@Configuration
public class RabbitConfig {
    @Bean //通过config配置bean的形式来创建 交换机
    public DirectExchange ttlExchange(){
        return new DirectExchange("ttl.direct");
    }

    @Bean // 通过config配置bean的形式来创建 队列
    public Queue ttlQueue(){ // 正常的TTL队列 创建
        return QueueBuilder.durable("ttl.queue") //设置队列名称
                .ttl(10000) // 设置超时时间
                .deadLetterExchange("dl.direct") // 设置死信交换机
                .deadLetterRoutingKey("dl")// 指定死信交换机的routingKey
                .lazy() // 这里是声明惰性队列
                .build();
    }

    @Bean
    public Binding simpleBinding(){ // 通过config配置Bean的形式来创建绑定关系
        return BindingBuilder.bind(ttlQueue()).to(ttlExchange()).with("ttl");
    }
}
