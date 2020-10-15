import paramiko
import time,sys
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
    sys.exit(0)
else:
    time.sleep(0.1)
    print('程序没有在运行，开始重启')
    time.sleep(0.1)
    shell.send('cd /server/licode \n')
    time.sleep(0.1)
    shell.send('./scripts/starterizoerver.sh \n')
    time.sleep(0.1)
    print('重启成功')

recv=shell.recv(100)
while not str(recv.decode('utf-8')).endswith('$ '):
    recv = shell.recv(100)#有问题
    buff+=recv.decode('utf-8')
    print(recv.decode('utf-8'))

print('程序完成')
shell.close()
ssh.close()