package com.test;

import com.test.dao.ItemDao;
import org.junit.jupiter.api.Test;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.boot.test.context.SpringBootTest;

@SpringBootTest
class Springboot02MybatisApplicationTests {

	@Autowired
	private ItemDao itemDao;

	@Test
	void contextLoads() {
		System.out.println(itemDao.getById(1));
	}

}
