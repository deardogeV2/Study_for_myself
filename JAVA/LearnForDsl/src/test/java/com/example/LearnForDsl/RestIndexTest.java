package com.example.LearnForDsl;

import org.apache.http.HttpHost;
import org.elasticsearch.action.index.IndexRequest;
import org.elasticsearch.client.RequestOptions;
import org.elasticsearch.client.RestClient;
import org.elasticsearch.client.RestHighLevelClient;
import org.elasticsearch.xcontent.XContentType;
import org.junit.jupiter.api.AfterEach;
import org.junit.jupiter.api.BeforeEach;
import org.junit.jupiter.api.Test;

import java.io.IOException;

public class RestIndexTest {
    private RestHighLevelClient client;

    @Test
    void testAddDocument() throws IOException {
        // 1、准备Request对象
        IndexRequest request = new IndexRequest("hotel").id("");
        //2、准备json文档
        request.source("", XContentType.JSON);
        //3、发送添加文档请求
        client.index(request, RequestOptions.DEFAULT);
    }

    @BeforeEach
    void setUp() {
        this.client = new RestHighLevelClient(RestClient.builder(HttpHost.create("http://127.0.0.1:9200")));
    }
    @AfterEach
    void tearDown() throws IOException {
        this.client.close();
    }
}
