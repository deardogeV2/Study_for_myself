//package com.example.WFW_order.mq;
//
//import org.springframework.beans.factory.annotation.Autowired;
//
//public class SendMQTest {
//
//    @Autowired
//    private RabbitTemplate rabbitTemplate;
//
//    private void testSimpleQueue(){
//        String queueName = "simple.queue";
//        String message = "hello, Spring amqp!";
//
//        rabbitTemplate.convertAndSend(queueName,message);
//    }
//
//}
