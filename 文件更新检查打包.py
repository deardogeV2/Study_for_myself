#=========================================================================
PPAP='''文件更新检查打包.py
创作者：ZB
更新时间：2020/8/3
使用注意：代码目前现在添加了分支选择和拉取部分，需
要注意文件更新部分包含.git仓库信息更新，请自主屏蔽QAQ'''
#=========================================================================
#库文件定义
import os
import time
import xlrd
import xlwt
from xlutils import copy
import shutil
import zipfile
from git import Git
#=========================================================================

#这里是目录的定义位置
#路径定义简单化
#=========================================================================
#这里是打包文件库名字设定
NAME='Wyy'#一个是Video，一个是Wyy

##这里是判断GIT库文件
#=========================================================================
if NAME=='Video':
    EXCEL_PATH = r'C:\Users\Administrator\Desktop\1\MM-VideoInquiry.xls'  # 作为记录存储的EXCEL所在路径
    O_PATH = r'F:\GIT\MM-VideoInquiry'  # 作为此次更新的仓库最高层目录
else:
    if NAME=='Wyy':
        EXCEL_PATH = r'C:\Users\Administrator\Desktop\1\MM-HLWYY.xls'  # 作为记录存储的EXCEL所在路径
        O_PATH = r'F:\GIT\MM-HLWYY'  # 作为此次更新的仓库最高层目录
    else:
        print('项目名字未找到，请确认！')
        exit()

#=========================================================================
#打包文件夹路径和压缩文件夹路径，不用修改
PAG_PATH=r'C:\Users\Administrator\Desktop\2'#用于压缩成zip的文件夹
ZIP_PATH=r'C:\Users\Administrator\Desktop\ZIP'#用于存放打包的ZIP的文件夹

#=========================================================================
#定义一下日志文件
log_name='更新日志'
log_msg='''更新日志内容。
'''

#=========================================================================
# file_times_modified=time.localtime(os.path.getmtime(PATH))
# Ctime=os.path.getmtime(PATH)#这里三天语句是用于测试时间戳转换时间
# print(Ctime)
#=========================================================================
#全局变量声明。
File_Time={}#创建一个新表单，用于读取和存储路径对应的时间戳内容
New_File={}#创建一个新表单，用于存储什么文件进行过更新
ChangeFiles = 0 #用于记录本次运行更新的文件数量
#=========================================================================
#文件拉取
print("请问分支是什么？")
Q=input("Y/N(Y以外都为不更换，默认为Mater分支):")
if Q=="Y" or Q=="y":
    CHECKOUT = input("请输入分支名字:")  # 切换分支
    try:
        repo = Git(O_PATH)  # 定义Git仓库地址
        repo.execute("git checkout " + CHECKOUT)  # 切换分支
        repo.execute("git pull")  # 拉取最新代码
        EXCEL_PATH=EXCEL_PATH[0:len(EXCEL_PATH)-4]+"_"+CHECKOUT+".xls"
    except:
        print("分支名字请确认正确")
        exit()
else:
    print("不更换分支")
    repo = Git(O_PATH)  # 定义Git仓库地址
    repo.execute("git checkout " + "master")  # 切换分支
    repo.execute("git pull")  # 拉取最新代码
    print("代码拉取完成")
    EXCEL_PATH = EXCEL_PATH[0:len(EXCEL_PATH) - 4] + "_" + "Master" + ".xls"

#=========================================================================
#封装文件更新Excel方法
def excel_update_all():
    global ChangeFiles, New_File, File_Time
    old_excel = xlrd.open_workbook(EXCEL_PATH)
    old_sheet = old_excel.sheet_by_index(0)  # 读取的第一张表
    new_excel = copy.copy(old_excel)  # 新表复制老表
    new_sheet = new_excel.get_sheet(0)  # 进行修改的新sheet
    changetime = 1
    for i in New_File.keys():
        ctime = New_File[i]  # 时间戳
        for j in range(old_sheet.nrows):#这里存在一个问题，使用range的话，
            if old_sheet.row(j)[0].value == i:#如果你用于记录的是一个空表，则不会进行下面的记录循环
                new_sheet.write(j, 1, ctime)
                break
            else:
                if j == old_sheet.nrows - 1:
                    new_sheet.write(j + changetime, 0, i)
                    new_sheet.write(j + changetime, 1, ctime)
                    changetime+=1
    new_excel.save(EXCEL_PATH)
    print('记录完成')

#封装的excel读取方法，用于创造元组
def excel_load_to_tuple():
    excel = xlrd.open_workbook(EXCEL_PATH)
    sheet = excel.sheet_by_index(0)#一般都是放在第一页
    for i in range(0,sheet.nrows):
        File_Time[sheet.row(i)[0].value]=sheet.row(i)[1].value

#时间判断方法,作用是修改元组（方便后期修改）
def judge_new(Path):
    global ChangeFiles,New_File,File_Time
    if Path in File_Time.keys():
        if Path!=EXCEL_PATH:
            if File_Time[Path] != os.path.getmtime(Path):  # 如果元组中记录的时间等于修改时间，
                File_Time[Path] = os.path.getmtime(Path)  # 代表没有修改过，否则进行更新记录
                New_File[Path] = os.path.getmtime(Path)#代表修改过更新记录登记一下
                ChangeFiles += 1
    else:#如果元组中根本不存在记录
        if Path!=EXCEL_PATH:
            File_Time[Path] = os.path.getmtime(Path)  # 元组中添加更新文件
            New_File[Path] = os.path.getmtime(Path)  # 文件更新数组中增加文件名
            ChangeFiles += 1  # 更新文件数量加1

#封装文件夹中文件遍历方法,同时也是递归获取绝对路径的方法。
def file_find(path):
    global ChangeFiles,New_File,File_Time
    for i in os.listdir(path):
        path2 = os.path.join(path, i)
        if os.path.isdir(path2):
            file_find(path2)
        else:
            judge_new(path2)

#路径切片方法,将绝对路径切为，最高层目录和下层目录
def cut_path(path):
    len0= len(O_PATH)
    len1=len(path)
    twopath=path[len0:len1]
    return twopath

#清除文件夹里面所有的文件（打包区清理）,注意同时会生成新的文件夹（同地址）
def clear_path(path):
    shutil.rmtree(path)
    os.mkdir(path)

#压缩一个文件夹至打包区
def zip_file(Path,Name):#第一个为打包内容文件夹地址
    global ChangeFiles, New_File, File_Time
    source_path=Path
    target_path=ZIP_PATH+'\\'+Name+'.zip'
    zip=zipfile.ZipFile(target_path,'w',zipfile.ZIP_DEFLATED)
    for path,dirnames,filenames in os.walk(source_path):
        #去掉目标跟路径只对文件夹下的文件以及文件夹进行压缩
        fpath=path.replace(source_path,'')
        for filename in filenames:
            zip.write(os.path.join(path,filename),os.path.join(fpath,filename))
    zip.close()
    print('打包完成')

#获取顶层文件名方法
def top_name(Path):
    i=0
    length=len(Path)
    topname=Path
    while i <100:
        if Path[-i]=='\\':
            topname=Path[length-i:length]
            return topname
        else:
            i+=1

#将修改文件按照相应路径放入打包区
def take_change_file():
    global ChangeFiles, New_File, File_Time
    for i in New_File.keys():#通过修改文件数来进行复制
        twopath = cut_path(i)  # 获取后字段
        target_path = PAG_PATH + top_name(O_PATH)+twopath  # 得到新地址
        name = ''
        path = ''
        length = len(target_path)
        x = 1
        while x < 100:
            if target_path[-x] == '\\':
                name = target_path[length - x + 1:length]
                path = target_path[0:length - x]
                break
            else:
                x += 1
        if os.path.isdir(path):
            shutil.copy(i, target_path)
        else :
            os.system('md'+' '+'"'+path+'"')#使用cmd会避免mkdir的一层一层添加问题。
            shutil.copy(i, target_path)

#日志文件生成
def log_creater(name,msg,Path):
    full_path=Path+'\\'+name+'.txt'#文件名生成
    file=open(full_path,'w')#写文件
    file.write(msg)
    file.close()#关闭文件

#根据当前时间，变更打包名字。
def time_get_for_zip():
    ticks=time.time()
    tt=time.localtime(ticks)
    a=str(tt.tm_year)
    b=str(tt.tm_mon)
    if tt.tm_mon<10:
        b='0'+str(tt.tm_mon)
    c=str(tt.tm_mday)
    if tt.tm_mday<10:
        c='0'+str(tt.tm_mday)
    out_time=a+b+c
    return out_time
#=========================================================================
#主程序位置

excel_load_to_tuple()#调用excel读取方法
file_find(O_PATH)#调用找到最新文件+文件记录方法
clear_path(PAG_PATH)#调用清除PAG_PATH方法，用于防止上次更新文件,如果运行错误，需要手动删除
take_change_file()#获取最新文件，放在PAG区内
log_creater(log_name,log_msg,PAG_PATH)#添加日志文件
if not os.listdir(PAG_PATH):#文件夹判断是否为空
    print("打包文件夹为空，本地文件没有更新")
else:
    zip_file(PAG_PATH,time_get_for_zip())
    print("更新文件数为："+str(ChangeFiles))
    print("请注意！这里的更新文件数包含.git仓库信息文件更新，")
    print("具体的更新请看项目路径！！！")
    log_creater(log_name,log_msg,PAG_PATH)
    excel_update_all()  # 上传最新的时间戳记录用于下次登记,更新文件过多时，可能速度较慢，需要等待

print("时间戳记录地址:"+EXCEL_PATH)
print(PPAP)
#=========================================================================