package com.hmdp.utils;

import org.springframework.data.redis.core.StringRedisTemplate;
import org.springframework.stereotype.Component;

import javax.annotation.Resource;
import java.time.LocalDateTime;
import java.time.LocalTime;
import java.time.ZoneOffset;
import java.time.format.DateTimeFormatter;

@Component
public class RedisIdWorker {

    /**
     * 开始时间戳
     *
     * @param keyPrefix
     * @return
     */
    private static final long BEGIN_TIMESTAMP = 1640995200;
    private static final int COUNT_BITS = 32; // 序列号位数

    @Resource
    StringRedisTemplate stringRedisTemplate;

    // 不同业务的key不同
    public long nextId(String keyPrefix) {

        // 准备时间戳
        LocalDateTime now = LocalDateTime.now();
        long nowSecond = now.toEpochSecond(ZoneOffset.UTC);
        long timestamp = nowSecond - BEGIN_TIMESTAMP; // 存储时间错差值来进行存储

        // 生成序列号 利用Redis
        // 最好在添加当天日期防止历史数据过大，因为Redis一个key的自增也是有限制的。
        String nowDay = now.format(DateTimeFormatter.ofPattern("yyyyMMdd"));
        long count = stringRedisTemplate.opsForValue().increment("icr:" + keyPrefix + ":" + nowDay);

        // 位运算拼接，先把timestamp左移32位，然后再与count或就可以完成拼接拉
        long out = timestamp << COUNT_BITS;
        out = out | count;

        return out;
    }

    public static void main(String[] args) {
        LocalDateTime localDateTime = LocalDateTime.of(2022, 1, 1, 0, 0, 0);
        long secound = localDateTime.toEpochSecond(ZoneOffset.UTC); // 时区
        System.out.println(secound);
    }
}
