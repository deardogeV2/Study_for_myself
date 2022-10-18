package com.test.dao;

import com.baomidou.mybatisplus.core.mapper.BaseMapper;
import com.test.domain.Items;
import org.apache.ibatis.annotations.Mapper;

@Mapper
public interface ItemDao extends BaseMapper<Items> {
}
