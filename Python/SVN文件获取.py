#文件信息
PPAP='''
作者：ZB
更新时间：2020/9/8
使用注意：配置PP（邮件路径），Top（SVN本地仓库根目录），OUT_PATH（输出目录）
其他：自动更新打包，出现问题可能是清除代码clear被拒绝或者是邮件路径给错了。
'''
#导入模块
import os
import shutil


PP=r'''trunk\issue\互联网医院\202009\29\db_physicial_examination.sql
trunk\issue\互联网医院\202009\29\sso_20200929.zip
trunk\issue\互联网医院\202009\29\timingTask_20200929.zip
trunk\issue\互联网医院\202009\29\WebAPI.zip'''

Top="D:\\SVN_zb\\"
OUT_PATH=r'C:\Users\Administrator\Desktop\SVN更新文件'
#暂时使用环境
SVN_path=[]#svn更新文件目录,r为绝对路径输入

def clear_path(path):
    shutil.rmtree(path)
    os.mkdir(path)

def qiepian(PP):
    a=PP.split('\n')
    for t in a:
        if t!='':
            SVN_path.append(Top+t)

def fuzhi(SVN_path):
    for path in SVN_path:
        # 文件名-路径获取
        FileName = os.path.split(path)[1]
        Path = os.path.split(path)[0]

        print("文件名:" + FileName)
        print("路径:" + Path)
        # 更新相应目录
        if os.path.exists(Path) == True:
            os.system("cd " + Path)
        else:
            os.system("md " + Path + "\\")  # 生成相应文件夹
            os.system("cd " + Path)  # 进入文件夹
        cmd = 'TortoiseProc.exe /command:update /path:"{}" /closeonend:0'.format(Path)  # svn,cmd更新指令
        os.system(cmd)
        # 更新结束
        print('更新结束')
        # 放到桌面文件夹
        shutil.copy(path, OUT_PATH)

#主程序部分
clear_path(OUT_PATH)
qiepian(PP)
fuzhi(SVN_path)
print('完成获取',f'更新文件共;{len(SVN_path)}个。',PPAP,sep='\n')