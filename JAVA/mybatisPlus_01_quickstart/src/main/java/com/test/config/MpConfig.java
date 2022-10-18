package com.test.config;

import com.baomidou.mybatisplus.extension.plugins.MybatisPlusInterceptor;
import com.baomidou.mybatisplus.extension.plugins.inner.OptimisticLockerInnerInterceptor;
import com.baomidou.mybatisplus.extension.plugins.inner.PaginationInnerInterceptor;
import org.springframework.context.annotation.Bean;
import org.springframework.context.annotation.Configuration;

@Configuration // 证明它是配置类
public class MpConfig {

    @Bean
    public MybatisPlusInterceptor mybatisPlusInterceptor(){
        MybatisPlusInterceptor mp = new MybatisPlusInterceptor(); // 创建新拦截器
        mp.addInnerInterceptor(new PaginationInnerInterceptor()); // 添加分页插件拦截器

        mp.addInnerInterceptor(new OptimisticLockerInnerInterceptor());// 添加乐观锁拦截器

        return mp; // 交给SpringBoot进行管理
    }
}
