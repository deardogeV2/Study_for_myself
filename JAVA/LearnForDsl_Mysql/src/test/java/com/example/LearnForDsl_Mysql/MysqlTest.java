package com.example.LearnForDsl_Mysql;

import com.alibaba.fastjson.JSON;
import com.example.LearnForDsl_Mysql.domain.HotelInfo;
import com.example.LearnForDsl_Mysql.service.HotelServiceImpl;
import org.junit.Test;
import org.junit.runner.RunWith;
import org.springframework.amqp.rabbit.core.RabbitTemplate;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.boot.test.context.SpringBootTest;
import org.springframework.test.context.junit4.SpringRunner;


@SpringBootTest
@RunWith(SpringRunner.class)
public class MysqlTest {

    @Autowired
    HotelServiceImpl hotelService;

    @Autowired
    private RabbitTemplate rabbitTemplate;

    @Test
    public void testOne(){
        HotelInfo hotelInfo = hotelService.selectOneById(1);
        System.out.println(hotelInfo);
    }

    @Test
    public void testTwo(){
        HotelInfo testOne = new HotelInfo();
        testOne.setId(7);
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
    }

    @Test
    public void testThree(){
        HotelInfo hotelInfo = hotelService.selectOneById(3);
        hotelInfo.setPrice(hotelInfo.getPrice()+20);
        Integer result = hotelService.updateOne(hotelInfo);

        if(result!=0){
            System.out.println("更新成功");
        }else {
            System.out.println("更新失败");
        }
    }

    @Test
    public void testSimpleQueue(){
        String queueName = "simple.queue";
        HotelInfo hotelInfo = hotelService.selectOneById(1);
        String message = JSON.toJSONString(hotelInfo);
        rabbitTemplate.convertAndSend(queueName,message);
    }
}
