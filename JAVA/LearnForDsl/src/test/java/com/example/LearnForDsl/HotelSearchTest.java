package com.example.LearnForDsl;

import com.example.LearnForDsl.service.HotelServiceImpl;
import org.apache.http.HttpHost;
import org.elasticsearch.action.search.SearchRequest;
import org.elasticsearch.action.search.SearchResponse;
import org.elasticsearch.client.RequestOptions;
import org.elasticsearch.client.RestClient;
import org.elasticsearch.client.RestHighLevelClient;
import org.elasticsearch.common.text.Text;
import org.elasticsearch.index.query.QueryBuilders;
import org.elasticsearch.search.SearchHit;
import org.elasticsearch.search.SearchHits;
import org.elasticsearch.search.aggregations.AggregationBuilders;
import org.elasticsearch.search.aggregations.Aggregations;
import org.elasticsearch.search.aggregations.bucket.terms.Terms;
import org.elasticsearch.search.fetch.subphase.highlight.HighlightBuilder;
import org.elasticsearch.search.fetch.subphase.highlight.HighlightField;
import org.elasticsearch.search.suggest.SuggestBuilder;
import org.elasticsearch.search.suggest.SuggestBuilders;
import org.junit.jupiter.api.AfterEach;
import org.junit.jupiter.api.BeforeEach;
import org.junit.jupiter.api.Test;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.boot.test.context.SpringBootTest;

import java.io.IOException;
import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;
import java.util.Map;

@SpringBootTest
public class HotelSearchTest {
    private RestHighLevelClient client;

    @Autowired
    HotelServiceImpl hotelService;

    @Test
    void testSuggest() throws IOException {
        SearchRequest request = new SearchRequest("test");

        request.source().suggest(new SuggestBuilder().addSuggestion(
                "suggest",
                SuggestBuilders.completionSuggestion("CS")
                        .prefix("r")
                        .skipDuplicates(true)
                        .size(5)));
        //发起请求
        SearchResponse response = client.search(request, RequestOptions.DEFAULT);

        System.out.println(response);
    }

    @Test
    void testFilters() throws IOException {
        Map<String, List<String>> outList = filters();
        System.out.println(outList);
    }

    Map<String, List<String>> filters() throws IOException {
        // 该方法就是酒店城市、星级、品牌聚合内容的输出

        // 1、准备Request
        SearchRequest request = new SearchRequest("hotel");
        // 多条件拼接-一层层的做，结构清晰一些
        request.source().size(0); //

        // 先对城市进行聚合
        request.source().aggregation(AggregationBuilders.terms("cityAgg")
                .field("city").
                size(100));

        // 再对星级进行聚合
        request.source().aggregation(AggregationBuilders.terms("starNameAgg")
                .field("starName").
                size(100));

        // 先对品牌进行聚合
        request.source().aggregation(AggregationBuilders.terms("brandAgg")
                .field("brand").
                size(100));

        //提交请求
        SearchResponse response = client.search(request, RequestOptions.DEFAULT);

        // 结果处理

        Aggregations aggregations = response.getAggregations();
        if ((aggregations == null)) {
            System.out.println("没有聚合结果，聚合写法有误");
            return null;
        }
        Map<String, List<String>> outMap = new HashMap<>();

        // 获取聚合结果一共有三个所以需要做三次 城市解析
        Terms cityTerms = aggregations.get("cityAgg");
        // 获取桶
        List<? extends Terms.Bucket> cityTermsBuckets = cityTerms.getBuckets();
        // 遍历
        if (!cityTermsBuckets.isEmpty()) {
            List<String> cityList = new ArrayList<>();
            for (Terms.Bucket bucket : cityTermsBuckets) {
                cityList.add(bucket.getKeyAsString());
            }
            outMap.put("city", cityList);
        }

        // 获取聚合结果一共有三个所以需要做三次 星级解析
        Terms starNameTerms = aggregations.get("starNameAgg");
        // 获取桶
        List<? extends Terms.Bucket> starNameTermsBuckets = starNameTerms.getBuckets();
        // 遍历
        if (!starNameTermsBuckets.isEmpty()) {
            List<String> starNameList = new ArrayList<>();
            for (Terms.Bucket bucket : starNameTermsBuckets) {
                starNameList.add(bucket.getKeyAsString());
            }
            outMap.put("starName", starNameList);
        }

        // 获取聚合结果一共有三个所以需要做三次 品牌解析
        Terms brandTerms = aggregations.get("brandAgg");
        // 获取桶
        List<? extends Terms.Bucket> brandBuckets = brandTerms.getBuckets();
        // 遍历
        if (!brandBuckets.isEmpty()) {
            List<String> brandList = new ArrayList<>();
            for (Terms.Bucket bucket : brandBuckets) {
                brandList.add(bucket.getKeyAsString());
            }
            outMap.put("brand", brandList);
        }
        return outMap;
    }

    @Test
    void testAggregation() throws IOException {
        // 1、准备Request
        SearchRequest request = new SearchRequest("hotel");

        // source调整
        request.source().size(0).aggregation(AggregationBuilders.terms("brand_agg").field("brand").size(20));
        // 发送
        SearchResponse response = client.search(request, RequestOptions.DEFAULT);

        handleRespone(response);
    }

    @Test
    void testMatch() throws IOException {
        // 1、准备Request
        SearchRequest request = new SearchRequest("hotel");

        // 2、 精确查询，单字段查询
        request.source().query(QueryBuilders.matchQuery("searchAll", "酒店")).highlighter(new HighlightBuilder().field("name").requireFieldMatch(false));

        // 3、精确查询，多字段查询
//        request.source().query(QueryBuilders.multiMatchQuery("酒店","name","business"));


        SearchResponse response = client.search(request, RequestOptions.DEFAULT);

        handleRespone(response);
    }

    @Test
    void testMatchALL() throws IOException {
        // 1、准备Request
        SearchRequest request = new SearchRequest("hotel");

        // 2、 组织DSL参数
        request.source().query(QueryBuilders.matchAllQuery());

        // 3、发送七扭去获得响应
        SearchResponse response = client.search(request, RequestOptions.DEFAULT);

        handleRespone(response);


    }

    private void handleRespone(SearchResponse response) {
        // 解析响应结果
        SearchHits searchHits = response.getHits();
        // 获取总条数
        long total = searchHits.getTotalHits().value;
        System.out.println("总条数" + total);
        // 搜索结果处理
        for (SearchHit searchHit : searchHits) { // 对hits中每个数据进行处理
            String json = searchHit.getSourceAsString();
            Map<String, HighlightField> highlightFields = searchHit.getHighlightFields();
            if (!highlightFields.isEmpty()) { // 高亮内容判断
                System.out.println("有高亮内容");
                for (String hightName : highlightFields.keySet()) { // 高亮内容中每个匹配内容处理
                    HighlightField highlightField = highlightFields.get(hightName);
                    for (Text text : highlightField.getFragments()) { // 匹配内容中的每个高亮文本处理
                        System.out.println(text.toString());
                    }
                }
            }
            System.out.println(json);
        }
        // 聚合结果处理
        Aggregations aggregations = response.getAggregations();
        if (!(aggregations == null)) {
            System.out.println("有聚合内容");
            // 获取聚合结果
            Terms brandTerms = aggregations.get("brand_agg");
            // 获取桶
            List<? extends Terms.Bucket> buckets = brandTerms.getBuckets();
            // 遍历操作
            for (Terms.Bucket bucket : buckets) {
                // 获取key，也就是品牌信息
                String keyName = bucket.getKeyAsString();
                System.out.println(keyName);
            }
        }

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
