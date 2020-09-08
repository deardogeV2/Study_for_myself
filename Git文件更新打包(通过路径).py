# =========================================================================
PPAP = '''文件更新检查打包.py
创作者：ZB
更新时间：2020/9/8
使用注意：代码目前实现了自动更新GIT然后按照目录查找文件进行打包。
注意：如果出问题，可以把Git获取文件夹里面的文件给清除一哈。'''
SOUR_PATH = r'''vi_api\.env
vi_api\.env.debug'''
# =========================================================================
# 库文件定义
import os
import sys
import shutil
import zipfile
from git import Git

# =========================================================================

# 这里是目录的定义位置
# 路径定义简单化
# =========================================================================
# 这里是打包文件库名字设定
NAME = 'Video'  # 一个是Video，一个是Wyy

##这里是判断GIT库文件
# =========================================================================
if NAME == 'Video':
    EXCEL_PATH = r'C:\Users\Administrator\Desktop\1\MM-VideoInquiry.xls'  # 作为记录存储的EXCEL所在路径
    O_PATH = r'F:\GIT\MM-VideoInquiry'  # 作为此次更新的仓库最高层目录
else:
    if NAME == 'Wyy':
        EXCEL_PATH = r'C:\Users\Administrator\Desktop\1\MM-HLWYY.xls'  # 作为记录存储的EXCEL所在路径
        O_PATH = r'F:\GIT\MM-HLWYY'  # 作为此次更新的仓库最高层目录
    else:
        print('项目名字未找到，请确认！')
        exit()

# =========================================================================
# 打包文件夹路径和压缩文件夹路径，不用修改
PAGPATH = r'C:\Users\Administrator\Desktop\Git获取'  # 用于压缩成zip的文件夹
# =========================================================================
# 定义一下日志文件
log_name = '更新日志'
log_msg = '''更新日志内容。
'''
# =========================================================================
# file_times_modified=time.localtime(os.path.getmtime(PATH))
# Ctime=os.path.getmtime(PATH)#这里三天语句是用于测试时间戳转换时间
# print(Ctime)
# =========================================================================
# 全局变量声明。
File_Time = {}  # 创建一个新表单，用于读取和存储路径对应的时间戳内容
New_File = {}  # 创建一个新表单，用于存储什么文件进行过更新
ChangeFiles = 0  # 用于记录本次运行更新的文件数量


# =========================================================================
# 文件拉取
def get():
    print("请问分支是什么？")
    Q = input("Y/N(Y以外都为不更换，默认为Mater分支):")
    if Q.lower() == 'q':
        CHECKOUT = input("请输入分支名字:")  # 切换分支
        try:
            repo = Git(O_PATH)  # 定义Git仓库地址
            repo.execute("git checkout " + CHECKOUT)  # 切换分支
            repo.execute("git pull")  # 拉取最新代码
        except:
            print("分支名字请确认正确")
            exit()
    else:
        print("不更换分支")
        repo = Git(O_PATH)  # 定义Git仓库地址
        repo.execute("git checkout " + "master")  # 切换分支
        repo.execute("git pull")  # 拉取最新代码
        print("代码拉取完成")


# =========================================================================
# 切片路径
# 获取所有Git文件路径
def qiepian_git(PP):
    a = PP.split('\n')
    for t in a:
        if t != '':
            GIT_PATH.append(Top + t)


# 获取打包文件路径
def qiepian_pag(PP):
    a = PP.split('\n')
    for t in a:
        if t != '':
            PAG_PATH.append(PAGPATH + '\\' + t)


# 单个文件的目录获取
def mulu_get(path):
    str = os.path.split(path)[0]
    return str


# 检查文件夹是否为空
def clear_path(path):
    shutil.rmtree(path)
    os.mkdir(path)


# 获取PAG目录地址
def mulu(P):
    for i in P:
        path = mulu_get(i)
        MULU_PATH.append(path)


# Git_TOP获取
def Git_Top():
    global Top
    if NAME == 'Video':
        Top = 'F:\\GIT\\MM-VideoInquiry\\'
    elif NAME == 'Wyy':
        Top = 'F:\\GIT\\MM-HLWYY\\'
    else:
        print('库名错误，请确认名字')
        sys.exit(0)


# 打包Git获取中的文件
def make_zip(source_dir, output_filename):
    zipf = zipfile.ZipFile(output_filename, 'w')
    pre_len = len(os.path.dirname(source_dir))
    for parent, dirnames, filenames in os.walk(source_dir):
        for filename in filenames:
            pathfile = os.path.join(parent, filename)
            arcname = pathfile[pre_len:].strip(os.path.sep)  # 相对路径
            zipf.write(pathfile, arcname)
    zipf.close()


# 全部打包文件名字表单
def get_dabao_name():
    for filename in os.listdir(r'C:\Users\Administrator\Desktop\Git获取'):
        PAG.append(filename)


# =========================================================================
# 主程序位置
get()
# 此前已经进行过代码拉取
GIT_PATH = []
PAG_PATH = []
MULU_PATH = []
PAG = []
# 清空文件夹（清除文件夹重新建立
clear_path(PAGPATH)

# 开始代码拉取
Git_Top()
qiepian_git(SOUR_PATH)
qiepian_pag(SOUR_PATH)
mulu(PAG_PATH)
TIME = len(GIT_PATH)
for i in range(TIME):
    if os.path.exists(MULU_PATH[i]) == True:
        shutil.copy(GIT_PATH[i], PAG_PATH[i])
    else:
        os.system("md " + MULU_PATH[i])
        shutil.copy(GIT_PATH[i], PAG_PATH[i])
# 文件已经全部获取

# 获取打包区文件名
get_dabao_name()
# 打包区文件打包
for dabao in PAG:
    make_zip(PAGPATH + '\\' + dabao, PAGPATH + '\\' + dabao + '.zip')
#打包完成
print(f'打包完成，打包文件数:{len(PAG_PATH)}\n请在' +
      PAGPATH + '中查看')
print(PPAP)
# =========================================================================
