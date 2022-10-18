package com.example.LearnForDsl.Listener;

import com.alibaba.fastjson.JSON;
import com.example.LearnForDsl.domain.HotelDoc;
import com.example.LearnForDsl.domain.HotelInfo;
import com.example.LearnForDsl.service.HotelServiceImpl;
import org.springframework.amqp.rabbit.annotation.RabbitListener;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Component;

@Component
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

}
