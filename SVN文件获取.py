#文件信息
#暂时先做一个从svn上面自动更新和自动获得文件的脚本
#制作人：周彪
#
#导入模块
import os
import shutil

PP=r'''trunk\issue\互联网医院\202008\31\vi_websocket_20200831.zip'''

Top="D:\\SVN_zb\\"
OUT_PATH=r'C:\Users\Administrator\Desktop\SVN更新文件'
#暂时使用环境
SVN_path=[]#svn更新文件目录,r为绝对路径输入

def clear_path(path):
    shutil.rmtree(path)
    os.mkdir(path)

def qiepian(PP):
    p = 0
    q = 0
    for t in PP:
        if t == '\n' :
            SVN_path.append(Top+PP[q:p])
            q = p+1
        if p+1==len(PP):
            SVN_path.append(Top+PP[q:p+1])
        p += 1

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