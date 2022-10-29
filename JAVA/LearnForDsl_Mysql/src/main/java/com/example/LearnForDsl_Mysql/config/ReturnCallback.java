package com.example.LearnForDsl_Mysql.config;

import lombok.extern.slf4j.Slf4j;
import org.springframework.amqp.core.ReturnedMessage;
import org.springframework.amqp.rabbit.core.RabbitTemplate;

@Slf4j
public class ReturnCallback implements RabbitTemplate.ReturnsCallback {
    /**
     * Returned message callback.
     *
     * @param returned the returned message and metadata.
     */
    @Override
    public void returnedMessage(ReturnedMessage returned) {
        if (returned.getMessage().getMessageProperties().getReceivedDelay()>0){ // 判断一下即可
            log.info("延迟消息而已");
            return;
        }
        log.info("糟糕了。消息没有到达队列！情况如下。");
        log.info("消息主体: {}", returned.getMessage());
        log.info("回复编码: {}", returned.getReplyCode());
        log.info("回复内容: {}", returned.getReplyText());
        log.info("交换器: {}", returned.getExchange());
        log.info("路由键: {}", returned.getRoutingKey());
    }
}
