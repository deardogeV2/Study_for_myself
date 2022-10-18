package com.example.test.Filter;

import org.springframework.cloud.gateway.filter.GatewayFilter;
import org.springframework.cloud.gateway.filter.GatewayFilterChain;
import org.springframework.core.annotation.Order;
import org.springframework.http.HttpStatus;
import org.springframework.http.server.reactive.ServerHttpRequest;
import org.springframework.stereotype.Component;
import org.springframework.util.MultiValueMap;
import org.springframework.web.server.ServerWebExchange;
import reactor.core.publisher.Mono;

@Order(-1) // 过滤器优先级，值越小，优先级最高
//@Component  // 告诉Spring
public class GlobalFilter implements GatewayFilter {
    /**
     * 处理当前请求，有必要的话通过{@link GatewayFilterChain} 将请求交给下一个过滤器处理
     *
     * @param exchange 请求上下文，里面可以获取Request、Response等信息
     * @param chain 用来把请求委托给下一个过滤器
     * @return {@code Mono<Void>} 返回标示当前过滤器结束
     *
     */

    @Override
    public Mono<Void> filter(ServerWebExchange exchange, GatewayFilterChain chain){
        // 1、 获取请求参数
        ServerHttpRequest request = exchange.getRequest();

        // 2、获取参数中的 对应参数
        MultiValueMap<String, String> params = request.getQueryParams(); // 获取所有参数，并且放到特殊的Map当中
        String auth = params.getFirst("key"); // 获取对应key的参数值，这里是获取Map的第一个值，也就是key

        // 3、 判断参数值是否等于admin
        if ("admin".equals(auth)){
            // 4、 是，放行
            return chain.filter(exchange); // 调用下一个过滤器的方法，并且把当前过滤器内容传递过去。意思就是放行了。
        }
        else {
            // 5.1、 否，拦截
            // 5.2 设置状态码
            exchange.getResponse().setStatusCode(HttpStatus.UNAUTHORIZED);

            // 5.3 拦截请求
            return exchange.getResponse().setComplete(); // 直接返回
        }
    }

}
