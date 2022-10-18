package com.example.LearnForDsl;

import com.example.LearnForDsl.constants.HotelConstants;
import com.example.LearnForDsl.service.HotelServiceImpl;
import org.apache.http.HttpHost;
import org.elasticsearch.action.admin.indices.delete.DeleteIndexRequest;
import org.elasticsearch.action.delete.DeleteRequest;
import org.elasticsearch.action.get.GetRequest;
import org.elasticsearch.action.get.GetResponse;
import org.elasticsearch.action.update.UpdateRequest;
import org.elasticsearch.client.RequestOptions;
import org.elasticsearch.client.RestClient;
import org.elasticsearch.client.RestHighLevelClient;
import org.elasticsearch.client.indices.CreateIndexRequest;
import org.elasticsearch.client.indices.GetIndexRequest;
import org.elasticsearch.xcontent.XContentType;
import org.junit.jupiter.api.AfterEach;
import org.junit.jupiter.api.BeforeEach;
import org.junit.jupiter.api.Test;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.boot.test.context.SpringBootTest;

import java.io.IOException;
import java.util.List;

@SpringBootTest
class RestInittest {

	private RestHighLevelClient client;

	@Autowired
	HotelServiceImpl hotelService;


	@Test
	void testDeleteDocument() throws IOException {
		DeleteRequest request = new DeleteRequest("hotel","2");
		try{
			client.delete(request,RequestOptions.DEFAULT);
		}catch (Exception e){
			System.out.println("看看是不是返回信息嘛"+e.toString());
		}
	}

	@Test
	void testGetDocument() throws IOException {
		//1、准备request对象
		GetRequest request = new GetRequest("hotel","1");

		//2、发送请求，得到结果。
		GetResponse response = client.get(request,RequestOptions.DEFAULT);

		//3、解析结果
		String json = response.getSourceAsString();

		System.out.println(json);
	}

	@Test
	void testUpdateDocumentById() throws IOException {
		UpdateRequest request = new UpdateRequest("hotel","1");

		request.doc(
				"starName","5星"
		);

		try{
			client.update(request,RequestOptions.DEFAULT);
		}catch (Exception e){
			System.out.println("看看是不是返回信息嘛"+e.toString());
		}
	}

	@Test
	void testInit(){
		System.out.println(this.client);
	}

	@Test
	void creatHotelIndex() throws IOException {

		//1、创建Request对象
		CreateIndexRequest request = new CreateIndexRequest("hotel");
		//2、准备请求的参数：DSL语句
		request.source(HotelConstants.MAPPING_TEMPLATE, XContentType.JSON);

		//3、发送Request请求——执行索引库操作
		client.indices().create(request, RequestOptions.DEFAULT);
	}

	@Test
	void DeleteHotelIndex() throws IOException {
		DeleteIndexRequest request = new org.elasticsearch.action.admin.indices.delete.DeleteIndexRequest("hotel");
		client.indices().delete(request,RequestOptions.DEFAULT);
	}

	@Test
	void testExistsHotelIndex() throws IOException {
		GetIndexRequest request = new GetIndexRequest("hotel");

		boolean exists = client.indices().exists(request,RequestOptions.DEFAULT);

		System.out.println(exists);
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
