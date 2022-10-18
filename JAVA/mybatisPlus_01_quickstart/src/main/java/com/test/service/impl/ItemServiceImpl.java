package com.test.service.impl;

import com.test.dao.ItemDao;
import com.test.service.ItemService;
import org.springframework.beans.factory.annotation.Autowired;

public class ItemServiceImpl implements ItemService {

    @Autowired
    public ItemDao itemDao;

    @Override
    public void testService() {
        System.out.println("测试");
    }
}
