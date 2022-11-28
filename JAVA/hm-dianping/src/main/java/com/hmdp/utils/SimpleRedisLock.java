package com.hmdp.utils;

import org.apache.tomcat.jni.File;
import org.springframework.core.io.ClassPathResource;
import org.springframework.data.redis.core.StringRedisTemplate;
import org.springframework.data.redis.core.script.DefaultRedisScript;
import org.springframework.data.redis.core.script.RedisScript;

import java.util.Collections;
import java.util.Objects;
import java.util.UUID;
import java.util.concurrent.TimeUnit;

public class SimpleRedisLock implements ILock {
    private String name;
    private StringRedisTemplate stringRedisTemplate;
    private static final String LOCK_KEY_PREFIX = "lock:";
    private static final String ID_PREFIX = UUID.randomUUID() + "-"; // 使用UUID生成随机数

    private static final DefaultRedisScript<Long> UNLOCK_SCRIPT;

    static { // 初始化脚本对象
        UNLOCK_SCRIPT = new DefaultRedisScript<>();
        UNLOCK_SCRIPT.setLocation(new ClassPathResource("unlock.lua"));// 设置脚本对象的内容
        UNLOCK_SCRIPT.setResultType(Long.class); // 设置返回类型
    }
    public SimpleRedisLock(String name, StringRedisTemplate stringRedisTemplate) {
        this.name = name;
        this.stringRedisTemplate = stringRedisTemplate;
    }

    @Override
    public boolean tryLock(long timeoutSec) {
        // 获取当前线程的标识
        String threadId = ID_PREFIX + Thread.currentThread().getId();
        // 尝试添加锁内容
        Boolean success = stringRedisTemplate.opsForValue().setIfAbsent(LOCK_KEY_PREFIX + name, threadId, timeoutSec, TimeUnit.SECONDS);
        return Boolean.TRUE.equals(success); // 防止自动拆箱导致的指针异常。
    }

    @Override
    public void unlock() {
        // 调用lua脚本释放锁
        stringRedisTemplate.execute(
                UNLOCK_SCRIPT,
                Collections.singletonList(LOCK_KEY_PREFIX + name) // 使用工具传入单元素集合
                ,ID_PREFIX + Thread.currentThread().getId());

    }

//    @Override
//    public void unlock() {
//        // 获取线程标识
//        String threadId = ID_PREFIX + Thread.currentThread().getId();
//        String id = stringRedisTemplate.opsForValue().get(LOCK_KEY_PREFIX + name);
//        // 判断一下标识
//        if (threadId.equals(id)) {
//            // 删除redis存储内容
//            stringRedisTemplate.delete(LOCK_KEY_PREFIX + name);
//        }
//    }
}
