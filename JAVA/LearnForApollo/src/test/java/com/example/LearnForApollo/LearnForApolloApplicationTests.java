package com.example.LearnForApollo;

import com.ctrip.framework.apollo.Config;
import com.ctrip.framework.apollo.ConfigService;
import org.junit.jupiter.api.Test;
import org.springframework.boot.test.context.SpringBootTest;

//@SpringBootTest
class LearnForApolloApplicationTests {

	@Test
	void contextLoads() throws InterruptedException {
		Config config = ConfigService.getAppConfig(); // 获取配置对象

		while(true){  // 这里循环去，你会发现config.getProperty获取的内容是实时。也就是说你一旦发布新的配置，这里就会更新。
			// 这就是因为apollo配置中心服务在数据发生修改的时候会通知客户端获取最新的配置。所以config对象中的内容就修改了。
			Thread.sleep(3000);
			String testString = config.getProperty("app.name",null); // 获取配置内容
			System.out.println(testString);
		}
	}

}
