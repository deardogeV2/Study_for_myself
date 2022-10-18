package com.example.LearnForDsl.server;

import com.alibaba.fastjson.JSON;
import com.example.LearnForDsl.domain.HotelDoc;
import org.elasticsearch.action.index.IndexRequest;
import org.elasticsearch.client.RequestOptions;
import org.elasticsearch.client.RestHighLevelClient;
import org.elasticsearch.xcontent.XContentType;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

@Service
public class HotelDslServerImpl {

    @Autowired
    RestHighLevelClient client;

    public void addOne(HotelDoc hotelDoc){
        //1、先通过SQL服务查询对应数据，并且存放至正确的数据类型中。正确的数据类型是对应索引库的domain而不是对应sql的domain
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

    public void addOneByMq(String message){

    }
}
