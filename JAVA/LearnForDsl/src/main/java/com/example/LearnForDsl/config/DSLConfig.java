package com.example.LearnForDsl.config;

import org.apache.http.HttpHost;
import org.elasticsearch.client.RestClient;
import org.elasticsearch.client.RestHighLevelClient;
import org.springframework.context.annotation.Configuration;
import org.springframework.stereotype.Component;

@Component
public class DSLConfig {
    RestHighLevelClient client = new RestHighLevelClient(RestClient.builder(HttpHost.create("http://127.0.0.1:9200")));
}
