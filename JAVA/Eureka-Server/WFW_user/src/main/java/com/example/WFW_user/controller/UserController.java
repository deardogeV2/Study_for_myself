package com.example.WFW_user.controller;

import com.example.WFW_user.dao.UserDao;
import com.example.WFW_user.domain.User;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PathVariable;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RestController;

@RestController()
@RequestMapping("/user")
public class UserController{

    @Autowired
    UserDao userDao;

    @GetMapping("/{id}")
    public User queryById(@PathVariable("id") Integer id) throws InterruptedException {
        if (id == 999){
            Thread.sleep(5000);
            return userDao.getOneById(999);
        }
        return userDao.getOneById(id);
    }

}