package com.example.WFW_user.dao;

import com.example.WFW_user.domain.User;
import org.apache.ibatis.annotations.Mapper;

@Mapper
public interface UserDao {
    public User getOneById(Integer id);
}
