#文件信息
#暂时先做一个从svn上面自动更新和自动获得文件的脚本
#制作人：周彪
#
#导入模块
import os
import subprocess
import requests
import shutil

PP=r'''trunk\issue\互联网医院\202007\video_server\db_im_inquiry.sql
trunk\issue\互联网医院\202007\video_server\db_sys.sql
trunk\issue\互联网医院\202007\video_server\doctor_platfrom.zip
trunk\issue\互联网医院\202007\video_server\hsol_admin.zip
trunk\issue\互联网医院\202007\video_server\hsol_api.zip
trunk\issue\互联网医院\202007\video_server\hsol_doctor.zip
trunk\issue\互联网医院\202007\video_server\vi_api.zip
trunk\issue\互联网医院\202007\video_server\vi_websocket.zip
trunk\issue\互联网医院\202007\video_server\WebAPI.zip'''

Top="D:\\SVN_zb\\"
#暂时使用环境
SVN_path=[]#svn更新文件目录,r为绝对路径输入

def qiepian(PP):
    global SVN_path
    p = 0
    q = 0
    time = 0
    for t in PP:
        if t == '\n':
            SVN_path.append(Top+PP[q:p])
            time += 1
            q = p+1
        p += 1


def fuzhi(SVN_path):
    for path in SVN_path:
        # 文件名-路径获取
        length = len(path)
        i = length
        while i > 0:
            i -= 1
            if path[i] == '\\':
                break

        FileName = path[i + 1:length]
        Path = path[0:i]

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
        shutil.copy(path, r'''C:\Users\Administrator\Desktop\SVN更新文件''')

qiepian(PP)

fuzhi(SVN_path)