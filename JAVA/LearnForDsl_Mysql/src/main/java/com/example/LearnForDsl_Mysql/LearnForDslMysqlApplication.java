package com.example.LearnForDsl_Mysql;

import org.apache.http.HttpHost;
import org.elasticsearch.client.RestClient;
import org.elasticsearch.client.RestHighLevelClient;
import org.springframework.amqp.rabbit.core.RabbitTemplate;
import org.springframework.boot.SpringApplication;
import org.springframework.boot.autoconfigure.SpringBootApplication;
import org.springframework.context.annotation.Bean;

@SpringBootApplication
public class LearnForDslMysqlApplication {

	public static void main(String[] args) {
		SpringApplication.run(LearnForDslMysqlApplication.class, args);
	}
	@Bean
	public RestHighLevelClient client(){
		return new RestHighLevelClient(RestClient.builder(HttpHost.create("http://127.0.0.1:9200")));
	}


}
