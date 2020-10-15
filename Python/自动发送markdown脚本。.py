import requests
import time
import hmac
import hashlib
import base64
import urllib.parse
import json
from urllib import parse
import re
'''
目前这个接口用的是钉钉的群聊机器人接口，根据接口文档所写。
因为是公司的机器人，请大家不要使用这个机器人接口。
可以尝试自己去钉钉开发文档中查看创建机器人进行学习。
钉钉开发文档：https://ding-doc.dingtalk.com/doc#/serverapi2/qf2nxq
'''

def json_to_markdown(json_data,err=None,errname='问题反馈'):
    jsondata=json_data
    text = json.loads(jsondata)
    mokuai = ['p1', 'p2', 'p3', 'p4', 'p5_1', 'p5_2', 'p5_3', 'p5_4', 'p5_5', 'p12', 'p_title']
    for i in text['template_args']:
        #url解码
        text['template_args'][i]=parse.unquote(text['template_args'][i])
    print(text)

    #至此url解码完毕,开始进行格式规整
    out={}
    out['title']=errname
    out['text']='#### '+errname+' @'+'电话号码:'+text['mobile']+'\n'
    for i in text:
        if i !='template_args':
            out['text']+='>'+i+':'+text[i]+'\n\n'
        else:
            for p in text[i]:
                out['text']+='>'+p+':'+text['template_args'][p]+'\n\n'

    if err!=None:
        out['text']+='>问题点:\n\n'
        for i in err:
            out['text']+='>>'+i+'\n\n'
    return out

jsondata = '{"app_id":"10004","client_id":"70000","mobile":"15247841251","send_status":"1","template_args":{"p1":"%E5%91%A8%E5%BD%AA%E5%BD%AA","p2":"%E8%B4%B5%E9%98%B3%E5%B8%82%E5%8F%A3%E8%85%94%E5%8C%BB%E9%99%A2","p3":"%E5%8F%A3%E8%85%94%E4%BF%AE%E5%A4%8D%E7%A7%91","p4":"%E5%BC%A0%E7%BF%8A","p5_1":"2020%E5%B9%B4","p5_2":"09%E6%9C%8828%E6%97%A5","p5_3":"","p5_4":"%E5%91%A8%E4%B8%80","p5_5":"%E4%B8%8A%E5%8D%88","p12":"2","p_title":"%E5%8F%96%E6%B6%88%E6%8C%82%E5%8F%B7"},"template_id":"1186"}'
ERR='''
<body>^M
    <div class=\"echo\">^M
            </div>^M
        <div class=\"exception\">^M
        ^M
            <div class=\"info\"><h1>页面错误！请稍后再试～</h1></div>^M
        ^M
    </div>^M
        ^M
    ^M
    ^M
    <div class=\"copyright\">^M
        <a title=\"官方网站\" href=\"http://www.thinkphp.cn\">ThinkPHP</a> ^M
        <span>V5.0.13</span> ^M
        <span>{ 十年磨一剑-为API开发设计的高性能框架 }</span>^M
    </div>^M
    </body>^M
<ml>
'''
ERR_data=re.findall(r'<\w+>(.+)</\w+>',ERR)
out=json_to_markdown(jsondata,err=ERR_data,errname='问题')

'''
这里是断层，上面是将json形式字符串变成钉钉需求的markdown格式
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

def fasong(json):
    timestamp,sign=get_timestamp_sign(secret)
    list={
        'msgtype':'markdown',
        'markdown':json
    }
    url='https://oapi.dingtalk.com/robot/' \
        'send?access_token=284397cd7ac0a0ab6dc522fe1492b640f35ca8be6c3b9753452a8040d29f55ab'
    url_and_jiaqian=url+'&'+'timestamp='+timestamp+'&'+'sign='+sign
    res=requests.post(url_and_jiaqian,json=list)#这里因为我们传入的是一个json格式，所以要用json关键字。                                         #传入一个单层的item（value中不包含迭代）时可以直接赋值给data（拆包问题）
    print(res.text)

fasong(out)

print(ERR_data)