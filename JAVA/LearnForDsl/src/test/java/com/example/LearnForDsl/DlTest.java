package com.example.LearnForDsl;

import lombok.extern.slf4j.Slf4j;
import org.junit.jupiter.api.Test;
import org.springframework.amqp.core.Message;
import org.springframework.amqp.core.MessageBuilder;
import org.springframework.amqp.rabbit.core.RabbitTemplate;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.boot.test.context.SpringBootTest;

import java.nio.charset.StandardCharsets;

@SpringBootTest
@Slf4j
public class DlTest {

    @Autowired
    RabbitTemplate rabbitTemplate;

    @Test
    public void testTTL(){
        // 创建消息
        Message message = MessageBuilder.withBody("hello, ttl message".getBytes(StandardCharsets.UTF_8)).
                setExpiration("5000") // 这里实际上就给消息进行限时了
                        .build();
        rabbitTemplate.convertAndSend("ttl.direct","ttl",message);
        log.info("消息发送成功");
    }


}
