package com.example.LearnForZookeeper;

import org.apache.curator.RetryPolicy;
import org.apache.curator.framework.CuratorFramework;
import org.apache.curator.framework.CuratorFrameworkFactory;
import org.apache.curator.framework.recipes.locks.InterProcessMutex;
import org.apache.curator.retry.ExponentialBackoffRetry;

import java.util.concurrent.TimeUnit;

public class TicketTest {

    public static void main(String[] args) throws Exception {
        Ticket12306 ticket12306 = new Ticket12306();

        // 创建客户端
        Thread t1 = new Thread(ticket12306,"携程");
        Thread t2 = new Thread(ticket12306,"飞猪");
        Thread t3 = new Thread(ticket12306,"去哪儿");

        t1.start();
        t2.start();
        t3.start();
    }
}
