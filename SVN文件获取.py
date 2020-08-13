#文件信息
#暂时先做一个从svn上面自动更新和自动获得文件的脚本
#制作人：周彪
#
#导入模块
import os
import subprocess
import requests
import shutil

#环境设置
Ziptool=""#ZIP打包工具路径
SourcePath=""#源码路径
ResulePath=""#打包工具路径
LogPath=""#日志路径
ZipPath=""#Zip包存放路径

#暂时使用环境
SVN_path="D:\\SVN_zb\\"+r"trunk\issue\互联网医院\202008\MsgInquiryServer.zip"#svn更新文件目录,r为绝对路径输入
#文件名-路径获取
length=len(SVN_path)
i=length
while i>0:
    i-=1
    if SVN_path[i]=='\\':
        break

FileName=SVN_path[i+1:length]
Path=SVN_path[0:i]

print("文件名:"+FileName)
print("路径:"+Path)
#更新相应目录
if os.path.exists(Path)==True:
    os.system("cd " + Path)
else:
    os.system("md "+Path+"\\")#生成相应文件夹
    os.system("cd " + Path)#进入文件夹
cmd='TortoiseProc.exe /command:update /path:"{}" /closeonend:0'.format(Path)#svn,cmd更新指令
os.system(cmd)
#更新结束
print('更新结束')
#放到桌面文件夹
shutil.copy(SVN_path,r'''C:\Users\Administrator\Desktop\1''')