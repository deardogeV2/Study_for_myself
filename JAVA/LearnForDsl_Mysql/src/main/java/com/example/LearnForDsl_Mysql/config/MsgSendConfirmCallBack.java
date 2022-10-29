package com.example.LearnForDsl_Mysql.config;

import lombok.extern.slf4j.Slf4j;
import org.springframework.amqp.rabbit.connection.CorrelationData;
import org.springframework.amqp.rabbit.core.RabbitTemplate;

@Slf4j
public class MsgSendConfirmCallBack implements RabbitTemplate.ConfirmCallback {

    /**
     *
     * @param correlationData 相关配置信息
     * @param ack   exchange交换机 是否成功收到了消息。true 成功，false代表失败
     * @param cause 失败原因
     */
    @Override
    public void confirm(CorrelationData correlationData, boolean ack, String cause) {
        log.info("MsgSendConfirmCallBack , 回调id: {}", correlationData);
        if(ack) {
            log.info("消息发送成功");
        }else {
            log.info("消息发送失败: {}", cause);
        }
    }
}
