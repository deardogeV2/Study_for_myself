package com.example.WFW_order.dao;

import com.example.WFW_order.domain.Order;
import org.apache.ibatis.annotations.Mapper;

import java.util.List;

@Mapper
public interface OrderDao {
    public List<Order> selectAll();

    public Order selectOneByOrderId(Integer id);
}
