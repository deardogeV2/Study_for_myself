package com.example.LearnForDsl_Mysql.service;

import com.example.LearnForDsl_Mysql.dao.HotelDao;
import com.example.LearnForDsl_Mysql.domain.HotelInfo;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import java.util.List;

@Service
public class HotelServiceImpl {
    @Autowired
    HotelDao hotelDao;

    public List<HotelInfo> selectAll(){
        return hotelDao.selectAll();
    }

    public HotelInfo selectOneById(int id){
        return hotelDao.selectOneById(id);
    }

    public Integer insertOne(HotelInfo hotelInfo){return hotelDao.insertOne(hotelInfo);};

    public Integer updateOne(HotelInfo hotelInfo){return hotelDao.updateOne(hotelInfo);};

    public Integer deleteOne(Integer id){return hotelDao.deleteOne(id);};


}
