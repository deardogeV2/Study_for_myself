package com.example.LearnForDsl.service;

import com.alibaba.fastjson.JSON;
import com.example.LearnForDsl.domain.HotelDoc;
import com.example.LearnForDsl.domain.HotelInfo;
import org.elasticsearch.action.index.IndexRequest;
import org.elasticsearch.action.search.SearchRequest;
import org.elasticsearch.client.RequestOptions;
import org.elasticsearch.client.RestHighLevelClient;
import org.elasticsearch.index.query.QueryBuilder;
import org.elasticsearch.index.query.QueryBuilders;
import org.elasticsearch.xcontent.XContentType;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import java.util.List;

@Service
public class HotelServiceImpl {

    @Autowired
    RestHighLevelClient client;


    public void addDoc(HotelDoc hotelDoc){

        //2、准备Request对象
        IndexRequest request = new IndexRequest("hotel").id(hotelDoc.getId().toString());
        //3、准备JSON文档【需要用到fastjson】
        request.source(JSON.toJSONString(hotelDoc), XContentType.JSON);
        //4、发起请求
        // 【这里是因为Springboot版本低了没有做返回处理，所以先用try包起来。但实际上请求是已经获得200返回了的。数据已经插入。】
        try{
            client.index(request, RequestOptions.DEFAULT);
        }catch (Exception e){
            System.out.println("看看是不是返回信息嘛"+e.toString());
        }
    }

    public void addOneByMq(String msg){
        HotelDoc newOne = new HotelDoc(JSON.parseObject(msg,HotelInfo.class));

        IndexRequest request = new IndexRequest("hotel").id(newOne.getId().toString());

        request.source(JSON.toJSONString(newOne), XContentType.JSON);

        try{
            client.index(request, RequestOptions.DEFAULT);
        }catch (Exception e){
            System.out.println("看看是不是返回信息嘛"+e.toString());
        }
    }
}
