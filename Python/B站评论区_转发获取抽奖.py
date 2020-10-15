from selenium import webdriver
import time
import random
from selenium.common.exceptions import NoSuchElementException
driver=webdriver.Chrome(executable_path=r"F:\Chrome\chromedriver_win32\chromedriver.exe")#选择驱动路径，打开驱动
#WebDriver实例对象，可以看做遥控器
#==============================
def is_element_present(how,what):
    try:
        driver.find_element(by=how,value=what)#查找参数进行查找
    except NoSuchElementException:#如果存在则这里是
        return False
    return True

def T():
    time.sleep(2)
#==============================
List=[]
times=0
js='''window.scrollTo(0,2000)'''
def zhuanfa():
    global times,List
    a=driver.find_elements_by_css_selector('a[class="user-name c-pointer"]')
    driver.execute_script(js)
    time.sleep(1)
    b=driver.find_elements_by_css_selector('a[class="user-name c-pointer"]')
    while a!=b:
        a=b
        driver.execute_script(js)
        time.sleep(1)
        b = driver.find_elements_by_css_selector('a[class="user-name c-pointer"]')

    eles = driver.find_elements_by_css_selector('a[class="user-name c-pointer"]')
    for el in eles:
        name = el.text
        if name not in List:
            List.append(name)
            times += 1


def pinglun():
    global  times,List
    elem[1].click()  # 转发登录完成，进入评论区
    T()
    eles = driver.find_elements_by_css_selector('a[class="name "]')
    for el in eles:
        name = el.text
        if name not in List:
            List.append(name)
            times += 1
    eles = driver.find_elements_by_css_selector('a[class="name vip-red-name"]')
    for el in eles:
        name = el.text
        if name not in List:
            List.append(name)
            times += 1
#==============================
driver.get("https://t.bilibili.com/421883689621462201?tab=2")
#查看评论/转发页面
time.sleep(5)

elem=driver.find_elements_by_css_selector('div[class="single-button c-pointer"]')
elem[0].click()
T()
zhuanfa()
T()
pinglun()
while is_element_present('class name','next')!=False:
    eles=driver.find_elements_by_class_name('next')
    eles[1].click()
    T()
    pinglun()
T()
print(List)
print('↑↑↑↑↑↑↑↑上面是参与抽奖的名单↑↑↑↑↑↑↑↑↑')
print('抽奖人数：%s'%(times))
wins=[]
x=random.randint(0,times-1)
wins.append(List[x])
y=random.randint(0,times-1)
while y==x:
    print('第二个人跟第一个人重复了,自动重新抽取ing')
    y = random.randint(0, times - 1)
wins.append(List[y])
print('随机数为%s和%s所以：'%(x,y))
print('↓↓↓↓↓↓恭喜下面这两个B中奖↓↓↓↓↓↓')
print(wins)




