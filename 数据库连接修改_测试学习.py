import MySQLdb

'''
为了方面之后的学习，这里稍微记录一下常用SQL语句：
一，关于用户管理（SQL用户）：
1.新建用户:(需要注意`不是单引号')
CREATE USER `用户名字`@`用户IP` IDENTIFIED BY `密码`
2.成员从属：
GRANT `成员名字`@`成员IP` TO `老大名字`@`老大IP`
3.修改密码：
ALTER USER `用户名字`@`用户IP` IDENTIFIED BY ('新密码')；
4.查看用户权限：
SHOW GRANTS FOR `用户名字`@`用户IP`
5.用户权限赋予：
GRANT 权限名字（逗号隔开） ON *.* TO `用户名字`@`用户IP`;
6.去除用户权限：
REVOKE 权限名字（逗号隔开） ON 数据库名(全部用*.*) FROM `用户名字`@`用户IP`;

二、数据库操作：
1.查看数据库：
SHOW DATABASES
2.创建数据库
CREATE DATABASE 数据库名字
3.使用数据库
USE 数据库名字
4.删除数据库
DROP DATABASE 数据库名字

三、表的控制(重要)
1.创建表(添加判断，如果表不存在的话:CREATE
CREATE TABLE IF NOT EXISTS `数据库名字`.`表名字`(
`id` TINYINT UNSIGNED NOT NULL AUTO_INCREMENT,#这里每一行都是表中的一列，而auto_increment是主键
`name` VARCHAR(100) NOT NULL,#添加一列非空name列的char列
`user_id` INT(30) NOT NULL,#添加一列非空user_id 的int列
PRIMART KEY ('id')#设置基础键
)ENGINE InnoDb DEFAULT CHARSET=utf8;

2.复制表:CREATE
CREATE TABLE `库名`.`新表` SELECT 条目（逗号隔开，全部用*）FROM `库名`.`旧表`

3.查看数据库中可用的表:SHOW
SHOW TABLES

4.删除表:DROP
DROP TABLE IF EXISTS `表明`（也可以是`库`.`表明`）

5.重命名:RENAME
RENAME TABLE `老名字` TO `新名字`

6.修改表结构（重要）：ALTER的使用就是在已有表中进行怎删改查
ALTER TABLE `表名` ADD(DROP,CHANGE选一个)
如：
(1)添加列
ALTER TABLE `表1` 
ADD '列名' VARCHAR（类型） NOT NULL
(2)删除列:DROP
ALTER TABLE `表1` 
DROP CLOUMN '列名'（有些数据库系统不允许使用这种方法）
(3)修改数据类型:ALTER
ALTER TABLE `表名`
ALTER COLUMN '列名' VARCHAR（数据类型）

四、表中数据（很重要）
1.插入数据:INSERT
INSERT INTO `表名` (列1，列2...)VALUES(第一组数据),(第二组数据)，.....#数据必须满足每个列的要求（比如不为空）
2.将检索出来的数据插入（重要）
INSERT INTO `表名`(列1，列2...)
SELECT 检索列1，检索列2...FROM 条件（比如FROM `表2`）
3.更新数据：
（1）UPDATE `表1` SET name='小猪' WHERE id=1
根据SET来更新表1:，比如把所有id为1的项目的name都改为‘小猪’
4.删除数据：DELETE
DELETE FROM `表名` WHERE id=1
根据where条件对表明中的项目进行删除。
5.排序数据:ORDER BY
SELECT * FROM `表名` ORDER BY '列名' LIMIT 0,1000（从小到大排列）
SELECT * FROM `表名` ORDER BY '列名' DESC LIMIT 0,1000（从大到小排列）

五、条件控制（重要）：
1.WHERE:
SELECT *FROM 来源 WHERE条件
根据条件从来源筛选出数据进行展示
2.HAVING:
SELECT * FROM 来源 GROUP BY score HAVING count(*)>2
'''

conn=MySQLdb.connect(
    host='127.0.0.1',
    user='root',
    passwd='123456',
    db='zb_test',
    charset='utf8'
)

c=conn.cursor()

c.execute('SELECT * FROM zb_test.name')

all=c.fetchall()
print(all)