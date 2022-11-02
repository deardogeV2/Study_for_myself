package com.example.LearnForZookeeper;

import org.apache.curator.RetryPolicy;
import org.apache.curator.framework.CuratorFramework;
import org.apache.curator.framework.CuratorFrameworkFactory;
import org.apache.curator.framework.api.BackgroundCallback;
import org.apache.curator.framework.api.CuratorEvent;
import org.apache.curator.framework.recipes.cache.*;
import org.apache.curator.retry.ExponentialBackoffRetry;
import org.apache.zookeeper.CreateMode;
import org.apache.zookeeper.data.Stat;
import org.junit.After;
import org.junit.Before;
import org.junit.jupiter.api.AfterEach;
import org.junit.jupiter.api.BeforeEach;
import org.junit.jupiter.api.Test;
import org.springframework.boot.test.context.SpringBootTest;
import org.springframework.cloud.zookeeper.CuratorFactory;

import java.nio.charset.StandardCharsets;
import java.util.List;

//@SpringBootTest
class LearnForZookeeperApplicationTests {

	CuratorFramework curatorFramework;

	@AfterEach
	void cloud() {
		if (curatorFramework !=null){
			curatorFramework.close();
		}
	}

	//建立链接
	@BeforeEach
	public void testConnect(){
		// 第一种方式：CuratorFrameworkFactorynewClient() 创建客户端
//		1、connectString：zk server 链接地址和端口 多个用逗号隔开 "127.0.0.1:38080,127.0.0.1:38081"
//		2、sessionTimeoutMs：会话超时时间，单位毫秒
//		3、connectionTimeoutMs：链接超时时间，单位毫秒
//		4、retryPolicy： 重试策略
//		新建一个重试策略，参数1：每次休眠时间单位毫秒， 参数2：重试次数
		RetryPolicy retryPolicy = new ExponentialBackoffRetry(3000,10);

//		CuratorFramework curatorFramework = CuratorFrameworkFactory.newClient("127.0.0.1:38080",60*1000,15*1000,retryPolicy);

//		 第二种方式：CuratorFrameworkFactory.builder()创建客户端 链式编程，感觉也还行
		curatorFramework = CuratorFrameworkFactory.builder()
				.connectString("127.0.0.1:2181")
				.sessionTimeoutMs(60*1000)
				.connectionTimeoutMs(15*1000)
				.retryPolicy(retryPolicy)
				.namespace("ZB") // 名称空间，实际操作的时候不用再写完整路径中的/ZB/...
				.build(); // 最后要加上一个build

		curatorFramework.start(); // 开启链接
	}

	@Test
	public void testCreate() throws Exception {
//		1、先建立链接【略过】
//		如果创建节点，没有指定数据。则默认将当前客户端的IP作为数据存储。
//		String path = curatorFramework.create().forPath("/testAPI");
//		System.out.println(path);

//		2、带有数据的创建
//		String path2 = curatorFramework.create().forPath("/testAPI2","testForDate".getBytes(StandardCharsets.UTF_8));
//		System.out.println(path2);

//		3、节点的类型设置 - 临时的 ，当前会话一旦结束就消失。注意！是当前客户端的会话结束不是服务端结束。
//		String path3 = curatorFramework.create().withMode(CreateMode.EPHEMERAL).forPath("/testAPI3","testEPHEMERAL".getBytes(StandardCharsets.UTF_8));
//		System.out.println(path3);

//		4、多层节点的创建 - 命令行不行，但是API这里提供了该方法creatingParentsIfNeeded()
		String path4 = curatorFramework.create()
				.creatingParentsIfNeeded()
				.forPath("/testAPI4/point1","多层节点测试"
						.getBytes(StandardCharsets.UTF_8));
		System.out.println(path4);
	}

	@Test
	public void testSearch() throws Exception {
		// 查询节点 get方法
//		byte[] testOne = curatorFramework.getData().forPath("/testAPI4/point1");
//		System.out.println(new String(testOne));

//		// 查询子节点 : ls
//		List<String> children = curatorFramework.getChildren().forPath("/");
//		System.out.println(children);

//		查询节点的状态信息： ls -s .storingStatIn(status)将数据存进status中
		Stat status = new Stat();
		byte[] testTwo = curatorFramework.getData().storingStatIn(status).forPath("/testAPI");
		System.out.println(status);
		System.out.println(new String(testTwo));
	}

	@Test
	public void testSet() throws Exception {
		// 修改节点数据
//		curatorFramework.setData().forPath("/testAPI","测试用第一个端口".getBytes(StandardCharsets.UTF_8));

//		根据版本进行修改
		int version = 0; // 查询出来的
		Stat status = new Stat();
		byte[] testTwo = curatorFramework.getData().storingStatIn(status).forPath("/testAPI");
		version = status.getVersion(); // 查询到对应的版本号
		curatorFramework.setData().withVersion(100).forPath("/testAPI","测试版本修改内容".getBytes(StandardCharsets.UTF_8));
	}

	/**
	 * 删除节点：Delete deleteall
	 * 1、删除单个节点
	 * 2、删除带有子节点的节点
	 * 3、必须成功的删除节点
	 * 4、回调方法
	 */
	@Test
	public void testDelete() throws Exception {
		// 1、删除单个节点
//		curatorFramework.delete().forPath("/testAPI");

//		2、删除多有带有子节点的节点
//		curatorFramework.delete().deletingChildrenIfNeeded().forPath("/testAPI4");

//		3、必须成功的删除节点 .guaranteed()
//		curatorFramework.delete().guaranteed().forPath("testAPI2");

//		4、回调方法 这里用一个匿名函数试试 .inBackground()方法
		curatorFramework.delete().guaranteed().inBackground((c,s)->{
			System.out.println("我被删除了");
			System.out.println(c);
			System.out.println(s);
		}).forPath("/testAPI2");
	}

	@Test
	public void testNodeCache() throws Exception {
//		1、创建NodeCache对象
		NodeCache nodeCache = new NodeCache(curatorFramework,"/app1"); // 监听对象时app1
//		2、注册监听
		nodeCache.getListenable().addListener(new NodeCacheListener() {
			@Override
			public void nodeChanged() throws Exception {
				System.out.println("节点发生了变化");

				// 想要获取最新数据
				byte[] nowdate = nodeCache.getCurrentData().getData();
				System.out.println(new String(nowdate));
			}
		});

//		3、开启监听 如果设置为true，则开启监听时，加载缓存数据。
		nodeCache.start(true);

		while (true){
		}
	}

	/**
	 * 监听某个节点的所有子节点是否有变化
	 */
	@Test
	public void testPathChildrenCache() throws Exception {

//		1、PathChildrenCache 对象创建
		// cacheData 参数是控制是否在注册的时候帮你先缓存当前数据
		PathChildrenCache pathChildrenCache = new PathChildrenCache(curatorFramework,"/app1",true);

//		2、绑定监听器
		pathChildrenCache.getListenable().addListener(new PathChildrenCacheListener() {
			@Override
			public void childEvent(CuratorFramework curatorFramework, PathChildrenCacheEvent pathChildrenCacheEvent) throws Exception {
				System.out.println("子节点发生了变化");
				System.out.println(pathChildrenCacheEvent);
				// 监听更新，并且拿到变更之后的数据
				if (pathChildrenCacheEvent.getType() == PathChildrenCacheEvent.Type.CHILD_UPDATED){
					System.out.println("数据更新了");
					byte[] newDate = pathChildrenCacheEvent.getData().getData();
					System.out.println(new String(newDate));
				}
			}
		});

//		3、开启
		pathChildrenCache.start();

		while (true){
		}
	}

	@Test
	public void testTreeCache() throws Exception {
//		1、创建监听器
		TreeCache treeCache = new TreeCache(curatorFramework,"/app1");

//		2、注册监听器
		treeCache.getListenable().addListener(new TreeCacheListener() {
			@Override
			public void childEvent(CuratorFramework curatorFramework, TreeCacheEvent treeCacheEvent) throws Exception {
				System.out.println("子节点发生了变化");
				System.out.println(treeCacheEvent);
				// 监听更新，并且拿到变更之后的数据
				if (treeCacheEvent.getType() == TreeCacheEvent.Type.NODE_UPDATED){
					System.out.println("数据更新了");
					byte[] newDate = treeCacheEvent.getData().getData();
					System.out.println(new String(newDate));
				}
			}
		});

//		3、启动监听器
		treeCache.start();
		while (true){
		}
	}


}
