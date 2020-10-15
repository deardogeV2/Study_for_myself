import requests
import time
import hmac
import hashlib
import base64
import urllib.parse

'''
目前这个接口用的是钉钉的群聊机器人接口，根据接口文档所写。
因为是公司的机器人，请大家不要使用这个机器人接口。
可以尝试自己去钉钉开发文档中查看创建机器人进行学习。
钉钉开发文档：https://ding-doc.dingtalk.com/doc#/serverapi2/qf2nxq
'''

secret='SEC63b8487cc0b3d0ebf5b39e68c17b996f3aedc005efa86878d80b311d5cf204bf'
def get_timestamp_sign(Secret):#获取timestamp和sign
    timestamp = str(round(time.time() * 1000))
    secret = Secret
    secret_enc = secret.encode('utf-8')
    string_to_sign = '{}\n{}'.format(timestamp, secret)
    string_to_sign_enc = string_to_sign.encode('utf-8')
    hmac_code = hmac.new(secret_enc, string_to_sign_enc, digestmod=hashlib.sha256).digest()
    sign = urllib.parse.quote_plus(base64.b64encode(hmac_code))
    return [timestamp,sign]
timestamp,sign=get_timestamp_sign(secret)

list={
    'msgtype':'markdown',
    'markdown':{'title':'测试markdown信息',
                'text':'#### 杭州天气 @150XXXXXXXX \n> 9度，西北风1级，空气良89，相对温度73%\n> ![screenshot](https://img.alicdn.com/tfs/TB1NwmBEL9TBuNjy1zbXXXpepXa-2400-1218.png)\n> ###### 10点20分发布 [天气](https://www.dingtalk.com) \n'}
}
url='https://oapi.dingtalk.com/robot/' \
    'send?access_token=284397cd7ac0a0ab6dc522fe1492b640f35ca8be6c3b9753452a8040d29f55ab'
url_and_jiaqian=url+'&'+'timestamp='+timestamp+'&'+'sign='+sign

res=requests.post(url_and_jiaqian,json=list)#这里因为我们传入的是一个json格式，所以要用json关键字。
                                            #传入一个单层的item（value中不包含迭代）时可以直接赋值给data（拆包问题）
print(res.text)


