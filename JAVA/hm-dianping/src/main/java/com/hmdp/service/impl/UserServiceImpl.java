package com.hmdp.service.impl;

import cn.hutool.core.bean.BeanUtil;
import cn.hutool.core.bean.copier.CopyOptions;
import cn.hutool.core.lang.UUID;
import cn.hutool.core.util.RandomUtil;
import com.baomidou.mybatisplus.extension.service.impl.ServiceImpl;
import com.hmdp.dto.LoginFormDTO;
import com.hmdp.dto.Result;
import com.hmdp.dto.UserDTO;
import com.hmdp.entity.User;
import com.hmdp.mapper.UserMapper;
import com.hmdp.service.IUserService;
import com.hmdp.utils.RegexUtils;
import com.hmdp.utils.UserHolder;
import org.springframework.data.redis.core.StringRedisTemplate;
import org.springframework.stereotype.Service;

import javax.annotation.Resource;
import javax.servlet.http.HttpSession;
import java.time.LocalDateTime;
import java.time.format.DateTimeFormatter;
import java.util.*;
import java.util.concurrent.TimeUnit;
import java.util.stream.Collectors;

import static com.hmdp.Constants.RedisConstants.LOGIN_CODE_KEY_Z;
import static com.hmdp.Constants.RedisConstants.LOGIN_USER_KEY_Z;

/**
 * <p>
 * 服务实现类
 * </p>
 *
 * @author 虎哥
 * @since 2021-12-22
 */
@Service
public class UserServiceImpl extends ServiceImpl<UserMapper, User> implements IUserService {

    @Resource
    private StringRedisTemplate stringRedisTemplate;// 注入一下默认的StringRedisTemplate

    @Resource
    private IUserService userService;

    @Override
    public Result sendCode(String phone, HttpSession session) {
//        1、校验手机号，正则表达式校验。为了节约时间，正则表达式已经在项目中提前提供。
        if (RegexUtils.isPhoneInvalid(phone)) {
//        2、如果不符合返回错误信息
            return Result.fail("手机号格式错误");
        }
//        3、符合、生成验证码,利用之前导入的hutoolAPI中的随机数方法生成6位随机数
        String code = RandomUtil.randomNumbers(6);

////        4、保存验证码至Session
//        session.setAttribute("code",code);

//        4、保存验证码至redis，key为业务前缀+电话号码，value为验证码，时间为2分钟。
//        最好设置有效期。不然验证码一直有效太夸张了。而且内存容易爆炸。
        stringRedisTemplate.opsForValue().set(LOGIN_CODE_KEY_Z + phone, code, 2, TimeUnit.MINUTES);

//        5、发送验证码，一般是调用第三方的平台短信发送功能。但是介于我们确实没钱搞，所以我们这里就直接输出在日志里面看就行啦！
        log.debug("本次的code = " + code);

//        5、返回Ok
        return Result.ok();
    }

    @Override
    public Result login(LoginFormDTO loginFormDTO, HttpSession session) {
        String phone = loginFormDTO.getPhone();
        // 1、校验手机号
        if (RegexUtils.isPhoneInvalid(phone)) {
            return Result.fail("手机号校验失败");
        }

        // 2、现在修改为从Redis中取数据进行校验
        String cacheCode = stringRedisTemplate.opsForValue().get(LOGIN_CODE_KEY_Z + phone);
        String code = loginFormDTO.getCode();
        if (cacheCode == null) {
            return Result.fail("没有验证码记录");
        } else if (!Objects.equals(cacheCode, code)) {
            return Result.fail("验证码没写对");
        }

        // 3、根据手机号查询用户，mysql数据库查
        User user = query().eq("phone", phone).one();

        // 4、用户不存在，注册
        if (user == null) {
            // 单独写一个创建新用户操作
            user = createUserWithPhone(phone);
        }
        // 5、session 存储内容装修
        UserDTO userDTO = BeanUtil.copyProperties(user, UserDTO.class); // 使用工具中的参数复制功能也可以。

        // 6、现在修改为存储在Redis之中
        // token 生成，利用工具生成
        String token = UUID.randomUUID().toString(true);

        // 把UserDto转换为Map,CopyOptions复制配置，setIgnoreNullValue遗忘空值，setFieldValueEditor参数值变更类型都为String。
        Map<String, Object> newOne = BeanUtil.beanToMap(userDTO, new HashMap<>(),
                CopyOptions.create()
                        .setIgnoreNullValue(true)
                        .setFieldValueEditor((fieldName, fieldValue) -> fieldValue.toString()));

        String allToken = LOGIN_USER_KEY_Z + token;

        // 一次性全部存入redis，小坑点，stringRedisTemplate的key和value都是String，传递Long数据会报错。所以上面的Map变换就有一次修改。
        stringRedisTemplate.opsForHash().putAll(allToken, newOne);
        // 设置有效期也要设置有效期 30 分钟
        stringRedisTemplate.expire(allToken, 30, TimeUnit.MINUTES);

        // 返回token数据
        return Result.ok(allToken);
    }

    /**
     * 查询用户详情
     *
     * @param userId
     * @return
     */
    @Override
    public Result queryUserDetail(Long userId) {
        User user = getById(userId);
        if (user == null) {
            return Result.ok();
        }
        UserDTO userDTO = BeanUtil.copyProperties(user, UserDTO.class);
        return Result.ok(userDTO);
    }

    @Override
    public Result sign() {
        // 1、获得当前登录用户
        Long userId = UserHolder.getUser().getId();
        // 2、获取日期
        LocalDateTime now = LocalDateTime.now();

        // 3、拼接Key
        String keySuffix = now.format(DateTimeFormatter.ofPattern(":yyyyMM"));
        String key = "sign:" + userId + keySuffix;

        int dayofMonth = now.getDayOfMonth();

        // 写入Redis SETBIT key offset
        stringRedisTemplate.opsForValue().setBit(key, dayofMonth - 1, true);
        return Result.ok();
    }


    private User createUserWithPhone(String phone) {
//        1、创建用户
        User user = new User();
        user.setPhone(phone);
        user.setNickName("User_" + RandomUtil.randomNumbers(10));
//        2、保存用户，mybatisPlus提供的，继承的serviceImpl的方法
        save(user);

        return user;
    }
}
