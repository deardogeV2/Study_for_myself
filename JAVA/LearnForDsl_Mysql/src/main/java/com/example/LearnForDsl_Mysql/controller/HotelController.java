package com.example.LearnForDsl_Mysql.controller;

import com.alibaba.fastjson.JSON;
import com.example.LearnForDsl_Mysql.domain.HotelInfo;
import com.example.LearnForDsl_Mysql.service.HotelServiceImpl;
import org.springframework.amqp.rabbit.core.RabbitTemplate;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Controller;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RestController;

@RestController
@RequestMapping("hotel")
public class HotelController {

    @Autowired
    HotelServiceImpl hotelService;

    @Autowired
    private RabbitTemplate rabbitTemplate;

    @GetMapping("/testAdd")
    public Integer addOne(){
        HotelInfo testOne = new HotelInfo();
        testOne.setId(8);
        testOne.setName("汉庭酒店（武汉光谷高农生物科技园店）");
        testOne.setAddress("湖北省武汉市江夏区高新二路东湖技术开发区");
        testOne.setPrice(87);
        testOne.setBrand("汉庭");
        testOne.setCity("武汉");
        testOne.setStarName("3星");
        testOne.setBusiness("光谷");
        testOne.setLatitude("22.22");
        testOne.setLongitute("22.22");
        testOne.setPic("https://www.baidu.com");

        Integer result = hotelService.insertOne(testOne);

        if (result!=0){
            System.out.println("已插入");
        }else {
            System.out.println("插入失败");
        }

        HotelInfo newOne = hotelService.selectOneById(8);

        if (newOne == null){return 0;}

        String queueName = "simple.addOne";
        String message = JSON.toJSONString(newOne);
        rabbitTemplate.convertAndSend(queueName,message);

        System.out.println("发送已完成");

        return 1;
    }

}
