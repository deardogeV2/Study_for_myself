package com.example.LearnForZookeeper;

import org.apache.curator.RetryPolicy;
import org.apache.curator.framework.CuratorFramework;
import org.apache.curator.framework.CuratorFrameworkFactory;
import org.apache.curator.framework.recipes.locks.InterProcessMutex;
import org.apache.curator.retry.ExponentialBackoffRetry;

import java.util.concurrent.TimeUnit;

public class Ticket12306 implements Runnable{

    private int tickets = 10; // 假设数据库里面的票数有10张票

    private final InterProcessMutex lock; // Zookeeper的分布式锁实现类

    public Ticket12306(){ // 在构造参数中初始化lock
//        两个参数
//        1、ZK的客户端
        RetryPolicy retryPolicy = new ExponentialBackoffRetry(3000,10);
        CuratorFramework curatorFramework = CuratorFrameworkFactory.builder()
            .connectString("127.0.0.1:2181")
            .sessionTimeoutMs(60*1000)
            .connectionTimeoutMs(15*1000)
            .retryPolicy(retryPolicy)
            .build(); // 最后要加上一个build
        curatorFramework.start();
//        2、路径，锁所在的路径。

        lock = new InterProcessMutex(curatorFramework,"/ZBlock");
    }

    @Override
    public void run() {
        while(true){
            try { // 获取锁和释放锁操作必须放在try中
                // 获取锁
                lock.acquire(3, TimeUnit.SECONDS);
                if (tickets>0){
                    tickets--;
                    System.out.println(Thread.currentThread()+"：这个线程买了一张票。"+"剩余票数:"+tickets);
                }
                Thread.sleep(1000);
            } catch (Exception e) {
                System.out.println("运行报错了");
            }finally { // 释放锁的操作是必须的，所以放在finally中更安全
                try {
                    lock.release();
                } catch (Exception e) {
                    System.out.println("释放锁报错了");
                }
            }
        }

    }
}
