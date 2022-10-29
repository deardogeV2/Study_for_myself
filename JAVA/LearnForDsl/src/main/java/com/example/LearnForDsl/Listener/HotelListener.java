package com.example.LearnForDsl.Listener;

import com.alibaba.fastjson.JSON;
import com.example.LearnForDsl.domain.HotelDoc;
import com.example.LearnForDsl.domain.HotelInfo;
import com.example.LearnForDsl.service.HotelServiceImpl;
import lombok.extern.slf4j.Slf4j;
import org.springframework.amqp.core.MessageProperties;
import org.springframework.amqp.rabbit.annotation.*;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.context.annotation.Lazy;
import org.springframework.stereotype.Component;
import com.rabbitmq.client.Channel;
import org.springframework.amqp.core.Message;

import java.io.IOException;

@Component
@Slf4j
public class HotelListener {

    @Autowired
    HotelServiceImpl hotelService;

    @RabbitListener(queues = "simple.addOne")
    public void listenAddOneHotelDoc(String msg){
        HotelInfo hotelInfo = JSON.parseObject(msg,HotelInfo.class);

        HotelDoc hotelDoc = new HotelDoc(hotelInfo);

        System.out.println("收到消息【"+hotelDoc+"】");

        // 具体操作
        hotelService.addOneByMq(msg);
    }

    @RabbitListener(queues = "fanout.quest1")
    public void listenQueue(String msg){
        System.out.println("收到了消息: "+ msg);
    }

    @RabbitListener(queues = "fanout.testACK")
    public void testACK(Message message, Channel channel) throws IOException {
        // 获取对应消息
        MessageProperties messageProperties = message.getMessageProperties();
        // 消息打印
        log.info(messageProperties.toString());
        try {
            log.info(message.toString());
            log.info(new String(message.getBody()));
            // 故意写个错误，假设就是运行中报错了
            int a = 1/0;
            channel.basicAck(messageProperties.getDeliveryTag(), false);
        } catch (Exception e) {
            // 当前的消息是否重新投递的消息,也就是该消息是重新回到队列里的消息
            if (messageProperties.getRedelivered()) {
                log.info("消息已重复处理失败,拒绝再次接收...");
                // 拒绝消息
                channel.basicReject(messageProperties.getDeliveryTag(), false);
            } else {
                log.info("消息即将再次返回队列处理...");
                channel.basicNack(messageProperties.getDeliveryTag(), false, true);
            }
        }
    }


    @RabbitListener(bindings = @QueueBinding(
            value = @Queue(
                    name = "dl.queue",
                    durable = "true",
                    arguments = @Argument(
                            name = "x-queue-mode",
                            value = "lazy")),
            exchange = @Exchange(
                    name = "dl.direct"),
            key = "dl"))
    public void testDlQueue(String msg){
        log.info("接收到 dl.queue的延迟消息：{}",msg);
    }
}
