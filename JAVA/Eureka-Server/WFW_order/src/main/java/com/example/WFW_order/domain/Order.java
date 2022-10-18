package com.example.WFW_order.domain;

import com.fengn.test.domain.User;
import lombok.Data;

@Data
public class Order {
    private Integer orderId;
    private String itemName;
    private Integer number;

    private Integer userId;
    private User user;
}
