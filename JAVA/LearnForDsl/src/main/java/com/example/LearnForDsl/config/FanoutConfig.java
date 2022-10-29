package com.example.LearnForDsl.config;

import org.springframework.amqp.core.Binding;
import org.springframework.amqp.core.BindingBuilder;
import org.springframework.amqp.core.FanoutExchange;
import org.springframework.amqp.core.Queue;
import org.springframework.context.annotation.Bean;
import org.springframework.context.annotation.Configuration;

//@Configuration
//public class FanoutConfig {
//
//    //声明交换机FanoutExchange
//    @Bean
//    public FanoutExchange fanoutExchange(){
//        return new FanoutExchange("zb.fanout");
//    }
//
//    @Bean
//    public Queue fanoutQueue1(){
//        return new Queue("fanout.quest1");
//    }
//
//    @Bean
//    public Binding bindingQueue1(Queue fanoutQueue1,FanoutExchange fanoutExchange){
//        return BindingBuilder.bind(fanoutQueue1).to(fanoutExchange);
//    }
//}
