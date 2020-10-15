import os
''' 1.get_all_file(path,path_list) :获取目录下层所有文件、空文件夹的绝对路径
    参数，path：获取下层文件目录，path_list：用于存储的列表（可以为空）
    返回值:一个下层文件的绝对路径列表。
    2.clear_all_file(path):清空文件夹
    返回值：剩余未清除的文件，如果不为空则没有清干净'''
#获取目录下层所有文件、空文件夹的绝对路径
def get_all_file(path,path_list):
    P=os.listdir(path)
    for i in P:
        t=os.path.join(path,i)
        check=os.path.isdir(t)
        if check and os.listdir(t):
            get_all_file(t,path_list)
        else:
            path_list.append(t)
    return path_list

#清空文件夹！
def clear_all_file(path):
    af=os.listdir(path)
    for i in af:
        P=os.path.join(path,i)
        if not os.path.isdir(P):
            os.remove(P)
        else:
            clear_all_file(P)
            if not os.listdir(P):
                os.rmdir(P)
path=r'C:\Users\Administrator\Desktop\ZIP'
clear_all_file(path)
print()