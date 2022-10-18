package com.example.LearnForDsl_Mysql.dao;

import com.example.LearnForDsl_Mysql.domain.HotelDoc;
import com.example.LearnForDsl_Mysql.domain.HotelInfo;
import org.apache.ibatis.annotations.Mapper;

import java.util.List;

@Mapper
public interface HotelDao {
    public List<HotelInfo> selectAll();

    public HotelInfo selectOneById(Integer id);

    public Integer insertOne(HotelInfo hotelInfo);

    public Integer updateOne(HotelInfo hotelInfo);

    public Integer deleteOne(Integer id);


}
