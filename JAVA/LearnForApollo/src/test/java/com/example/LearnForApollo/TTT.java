package com.example.LearnForApollo;

import org.apache.http.util.Args;

@TestAnnotation("TTT")
public class TTT {

    public String testString = "测试用全局变量";

    public void MethodA(int a){
        System.out.println(a);
    }

    public static void main(String[] args){
        Class tttClass = TTT.class;

        System.out.println(tttClass.isAnnotationPresent(TestAnnotation.class));



    }
}
