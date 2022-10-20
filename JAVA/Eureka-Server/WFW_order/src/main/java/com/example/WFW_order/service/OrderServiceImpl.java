package com.example.WFW_order.service;

import com.example.WFW_order.dao.OrderDao;
import com.example.WFW_order.domain.Order;
import com.fengn.test.clients.UserClient;
import com.fengn.test.domain.User;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.context.annotation.Lazy;
import org.springframework.stereotype.Service;
import org.springframework.web.bind.annotation.RequestHeader;

import java.time.LocalDateTime;
import java.time.format.DateTimeFormatter;

@Service
//@RefreshScope
public class OrderServiceImpl {

    @Autowired
    OrderDao orderDao;

//    暂时不使用RestTemplate
//    @Autowired
//    RestTemplate restTemplate;

    @Autowired
    UserClient userClient;

    @Autowired
    com.example.WFW_order.domain.orderConfig orderConfig;

    //    @Value("${pattern.dateformat}")
    private String dateformat;

    public String now() {
        return LocalDateTime.now().format(DateTimeFormatter.ofPattern(orderConfig.getDateformat()));
    }

    public Order selectOneById(Integer id) { // 不需要必传

        // 查询订单
        Order order = orderDao.selectOneByOrderId(id);

//        // 使用RestTemplate【之前的硬编码方式】
//        // 现在改为负载均衡的方式
//        String url = "http://userService/user/" + order.getUserId();
        // 发送请求 使用Feign方式
//        User user = restTemplate.getForObject(url, User.class);
        User user = userClient.findByid(order.getUserId());
        // 封装User
        order.setUser(user);
        // 返回数据
        return order;
    }
}
