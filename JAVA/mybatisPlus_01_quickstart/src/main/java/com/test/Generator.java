package com.test;

import com.baomidou.mybatisplus.annotation.IdType;
import com.baomidou.mybatisplus.generator.AutoGenerator;
import com.baomidou.mybatisplus.generator.config.DataSourceConfig;
import com.baomidou.mybatisplus.generator.config.GlobalConfig;
import com.baomidou.mybatisplus.generator.config.PackageConfig;
import com.baomidou.mybatisplus.generator.config.StrategyConfig;

public class Generator {
    public static void main(String[] args) {
        // 创建生成器对象
        AutoGenerator autoGenerator = new AutoGenerator();
        // 1、配置数据源
        DataSourceConfig dataSourceConfig = new DataSourceConfig();
        dataSourceConfig.setDriverName("com.mysql.cj.jdbc.Driver");
        dataSourceConfig.setUrl("jdbc:mysql://localhost:3306/db1?serverTimezone=UTC");
        dataSourceConfig.setUsername("root");
        dataSourceConfig.setPassword("123");
        autoGenerator.setDataSource(dataSourceConfig);

        // 2、Global 全局配置
        GlobalConfig globalConfig = new GlobalConfig();
        globalConfig.setOutputDir(System.getProperty("user.dir")+"/mybatisPlus_01_quickstart/src/main/java");// 输出路径
        globalConfig.setOpen(false); // 是否自动打开
        globalConfig.setAuthor("周彪"); // 作者
        globalConfig.setFileOverride(true); // 设置是否覆盖原始生成的文件
        globalConfig.setMapperName("%sDao"); // 设置数据层接口名，%s是占位符，指代模块名称
        globalConfig.setIdType(IdType.ASSIGN_ID); // 设置ID 生成策略
        autoGenerator.setGlobalConfig(globalConfig); // 设置生成器全局设置

        //3、 包名设置
        PackageConfig packageConfig = new PackageConfig();
        packageConfig.setParent("com.test");// 设置生成的包名，与代码所在位置不冲突，二者叠加组成完整路径
        packageConfig.setEntity("domain"); // 设置实体类包名
        packageConfig.setMapper("dao"); // 设置数据层包名
        autoGenerator.setPackageInfo(packageConfig); // 设置生成器包名设置

        //4、 策略设置
        StrategyConfig strategyConfig = new StrategyConfig();
        strategyConfig.setInclude("db1");// 设置当前参与生成的表名，参数为可变参数
        strategyConfig.setTablePrefix("tb_"); //设置数据库表名前缀 模块名 = 数据库表名-前缀名
        strategyConfig.setRestControllerStyle(true);// 是否使用Rest风格
        strategyConfig.setVersionFieldName("version");// 设置乐观锁字段名
        strategyConfig.setLogicDeleteFieldName("isDelete");// 设置逻辑删除字段
        strategyConfig.setEntityLombokModel(true); // 设置是否启用lombok的@Data @Getter 等注解功能
        autoGenerator.setStrategy(strategyConfig); // 设置生成器的策略设置
        // 执行生成器
        autoGenerator.execute();
    }
}
