import paramiko
import time,sys
'''
在此记录一下常用的linux命令。
关于系统：
1.查看内核版本
uname -r

2.查看pic设备和USB设备
ispci -tv
isusb -tv

一：关于系统关机等：
1.关闭系统
shutdown -h now
init 0
telinit 0
shutdown -h hours:minutes &按预定时间关闭系统,时：分
shutdown -c &取消预订的时间关闭系统
shutdown -r now &现在重启
reboot &重启
logout &注销

2.查看运行程序(|grep为筛选)
ps -ef|grep 关键字

二：关于文件目录等（重点之一）：
1.cd (进入)
cd /data  &进入根目录下的data
cd data  &进入当前目录下的data文件夹
cd ../.. &返回上两级目录（..的话就是一级）

2.pwd（显示工作路径）

3.
ls/ll &显示当前目录的文件
tree &显示由根目录开始的树状结构

4.mkdir
mkdir name name2 name3 &创建一些叫name[n]的目录

5.rm
rm -f name &删除文件name
rmdir dir &删除文件夹dir
rm -rf dir dir2 dir3&删除目录及其中文件

6.cp
cp file1 file2 &文件复制，名字为绝对路径最好，否则就是当前目录下
cp -a dir1 dir2 &复制一个目录（包含替代

7.cat
cat file &查看文件

三：关于文件搜索：
1.find /目录 -name file &从/进入目录系统搜索文件和目录
2.find /目录 -name \*.bin &搜索以bin结尾文件

四：挂载一个文件系统：



'''
SERVER='9'
if SERVER=='9' or SERVER=='6':
    if SERVER == '9':
        HOST='10.21.128.9'
        NAME='zhoubiao'#root密码为：v$WL7Bq5@OkxRjc
        PASSWORD='cs5816497'
    if SERVER == '6':
        HOST = '10.21.128.6'
        NAME='zhoubiao'
        PASSWORD='cs5816497'
else:
    print("服务器名字选择错误，请选择6/9")
    sys.exit(0)

ssh=paramiko.SSHClient()
ssh.set_missing_host_key_policy(paramiko.AutoAddPolicy())
ssh.connect(HOST,22,NAME,PASSWORD)

print('连接完成')

shell=ssh.invoke_shell()

shell.send('su- \n')
time.sleep(0.1)

shell.send('v$WL7Bq5@OkxRjc\n')
time.sleep(0.1)

shell.send('cd /server/licode \n')
time.sleep(0.1)

shell.send('ps -ef |grep node \n')
time.sleep(0.1)
buff=''
t=1
recv=shell.recv(100)
print('输入完成')

while not str(recv.decode('utf-8')).endswith('$ '):
    recv = shell.recv(100)#有问题
    buff+=recv.decode('utf-8')
    print(recv.decode('utf-8'))

if 'erizoserver.js' in buff or 'erizoAgent.js' in buff:#单独查找
    print('程序正在运行')
else:
    print('程序没有在运行，开始重启')
    shell.send('./scripts/starterizoerver.sh \n')
    print('重启成功')

recv=shell.recv(100)
while not str(recv.decode('utf-8')).endswith('$ '):
    recv = shell.recv(100)#有问题
    buff+=recv.decode('utf-8')
    print(recv.decode('utf-8'))

print('程序完成')
shell.close()
ssh.close()