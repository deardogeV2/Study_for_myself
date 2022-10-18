package com.example.WFW_order.controller;

import com.example.WFW_order.domain.Order;
import com.example.WFW_order.service.OrderServiceImpl;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.web.bind.annotation.*;

@RestController
@RequestMapping("order")
public class orderController {
    @Autowired
    private OrderServiceImpl orderService;

    @GetMapping("/{orderId}")
    public Order queryOrderByUserId(@PathVariable("orderId") Integer orderId , @RequestHeader(value = "Truth", required = false) String truth) {

        System.out.println(truth);
        // 根据id查询订单并返回
        return orderService.selectOneById(orderId);
    }

    @GetMapping("/test")
    public void test(){
        System.out.println(orderService.now());
    }
}
