package com.test.dao;

import com.test.domain.ItemInfo;
import org.apache.ibatis.annotations.Mapper;

@Mapper // 标识它是一个Mybatis自动代理的接口
public interface ItemDao {
    public ItemInfo getById(Integer id);
}
