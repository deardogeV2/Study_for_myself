package com.test;

import com.baomidou.mybatisplus.core.conditions.query.LambdaQueryWrapper;
import com.baomidou.mybatisplus.core.conditions.query.QueryWrapper;
import com.baomidou.mybatisplus.core.metadata.IPage;
import com.baomidou.mybatisplus.extension.plugins.pagination.Page;
import com.test.dao.ItemDao;
import com.test.domain.Items;
import com.test.domain.query.ItemsQuery;
import com.test.service.impl.ItemServiceImpl;
import org.apache.dubbo.config.annotation.Reference;
import org.junit.jupiter.api.Test;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.boot.test.context.SpringBootTest;

import java.sql.Wrapper;
import java.util.ArrayList;
import java.util.List;
import java.util.Objects;

@SpringBootTest
class MybatisPlus01QuickstartApplicationTests {
//    @Autowired 在表现层，原本是使用Autowired进行本地自动住哪个配的
// 现在使用Reference进行远程装配

//    使用这个远程注解做了哪些事呢？
//    1.zookeeper注册中心获取userService的访问url，就是你设置在Service那个机器配置文件里面的url
//    2.进行远程调用RPC
//    3.将结果封装为一个代理对象，给到这个变量赋值。
    @Reference
    ItemServiceImpl itemService;

    @Test
    void contextLoads() { // 需要配置拦截器，MybatisPlus需要开启这个功能

        LambdaQueryWrapper<Items> wrapper = new LambdaQueryWrapper<Items>(); // 条件对象，注意这里需要指定泛型为domain类
        ItemsQuery itemsQuery = new ItemsQuery();

        // A用户
        Items newOne1 = itemService.itemDao.selectById(8); // A用户拿到的消息：version = 1

        // B用户
        Items newOne2 = itemService.itemDao.selectById(8); // B用户拿到的数据：version = 1

        newOne2.setItemName("测试乐观锁2");
        itemService.itemDao.updateById(newOne2);            // B用户修改完成后：version = 2 而version 1+1 = 2，判断条件成功，这条语句会执行

        newOne1.setItemName("测试乐观锁3");      // B用户修改完成后：version = 2这个判断条件一定会失败，所以这条操作会失败！
        itemService.itemDao.updateById(newOne1);
    }
}
