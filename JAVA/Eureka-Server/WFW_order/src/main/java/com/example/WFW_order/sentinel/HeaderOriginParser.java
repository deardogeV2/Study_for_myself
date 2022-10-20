package com.example.WFW_order.sentinel;


import com.alibaba.csp.sentinel.adapter.spring.webmvc.callback.RequestOriginParser;
import org.springframework.stereotype.Component;

import javax.servlet.http.HttpServletRequest;

@Component
public class HeaderOriginParser implements RequestOriginParser {
    @Override
    public String parseOrigin(HttpServletRequest httpServletRequest) {
        // 尝试获取请求头
        String origin = httpServletRequest.getHeader("origin");// 约定的头

        if(origin == null || origin.isEmpty()){
            origin = "blank";
        }
        return origin; // 这里实际上就是返回给Sentinel的流控应用名字
    }
}
