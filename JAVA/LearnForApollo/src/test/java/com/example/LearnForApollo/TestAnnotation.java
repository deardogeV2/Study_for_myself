package com.example.LearnForApollo;


import java.lang.annotation.ElementType;
import java.lang.annotation.Retention;
import java.lang.annotation.RetentionPolicy;
import java.lang.annotation.Target;

@Target(ElementType.TYPE) // 这个注解可作用域类
@Retention(RetentionPolicy.RUNTIME) // 操作权限是运行时可操作
@interface TestAnnotation {
    String value();
}
