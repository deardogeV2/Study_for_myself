package com.example.LearnForDsl_Mysql.config;

import com.rabbitmq.client.AMQP;
import lombok.extern.slf4j.Slf4j;
import org.springframework.amqp.core.FanoutExchange;
import org.springframework.amqp.core.Queue;
import org.springframework.amqp.rabbit.connection.CachingConnectionFactory;
import org.springframework.amqp.rabbit.core.RabbitAdmin;
import org.springframework.amqp.rabbit.core.RabbitTemplate;
import org.springframework.beans.BeansException;
import org.springframework.beans.factory.InitializingBean;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.context.ApplicationContext;
import org.springframework.context.ApplicationContextAware;
import org.springframework.context.annotation.Bean;
import org.springframework.context.annotation.Configuration;

import java.util.Map;

@Configuration
public class RabbitConfig implements InitializingBean {

    @Autowired
    private RabbitTemplate rabbitTemplate;

    /**
     * bean 初始化后执行
     */
    @Override
    public void afterPropertiesSet() {
        // 设置消息确认回调类
        rabbitTemplate.setConfirmCallback(new MsgSendConfirmCallBack());
        // 设置消息回退回掉类
        rabbitTemplate.setReturnsCallback(new ReturnCallback());
    }
}
