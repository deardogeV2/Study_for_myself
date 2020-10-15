import requests
from pprint import pprint

list={
'app_id':'10001','app_key':'QCYtAnfkaZiwrNwnxIlR6CTfG3gf90La'}
res=requests.get('http://api.hlwyy.cn/im/info/?inquiry_id=6415496029898385847',list)

if res.status_code==200:
    print('测试正常奥利给！')
    print('回显为：'+str(res.status_code))

else :
    print('没有连接上，请查看你的地址有没有写对。')