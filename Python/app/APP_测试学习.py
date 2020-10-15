from appium import webdriver
import  time,traceback
from selenium.webdriver.common.by import By
from selenium.common.exceptions import NoSuchElementException
import sys
#自动登录
User="18813158233"
Password="123456a"
#
desired_caps={}#进行自动化之前，告诉appium各项数据的字典
desired_caps['platformName']='Android'#自动化平台
desired_caps['platformVersion']='10'#安卓版本
desired_caps['deviceName']='test'#名字
desired_caps['app']=r''#设置路径,如果你手机没装应用时，系统从这里给你安装安装包
desired_caps['appPackage']='cn.longmaster.health'#用于告诉手机启动软件的包名
desired_caps['appActivity']='cn.longmaster.health.ui.appstart.AppStartActivity'#打开的界面
#我这里用的是登录界面
desired_caps['unicodeKeyboard']=True#界面允许输入中文
desired_caps['resetKeyboard']=True
desired_caps['noReset']=True#是否重置软件（类似以第一次安装来进行测试）
desired_caps['newCommandTimeout']=6000

#========定位
driver=webdriver.Remote('http://localhost:4723/wd/hub',desired_caps)
#元素检查存在方法定义，存在True，不存在False

def is_element_present(how,what):
    try:
        driver.find_element(by=how,value=what)#查找参数进行查找
    except NoSuchElementException:#如果存在则这里是
        return False
    return True

try:
    driver.implicitly_wait(5)
    #10次连接
    if is_element_present("id","com.lbe.security.miui:id/permission_allow_button_1")!=False:
        driver.find_element_by_id("com.lbe.security.miui:id/permission_allow_button_1").click()
        time.sleep(1)
    if is_element_present("id","com.lbe.security.miui:id/permission_allow_always_button")!=False:
        driver.find_element_by_id(
            "com.lbe.security.miui:id/permission_allow_always_button").click()  # 寻找element，使用element方法cleck
        time.sleep(1)
    if is_element_present("class_name","android.widget.ImageView"):
        for i in range (10):
            driver.swipe(798, 761, 245, 702)
            time.sleep(0.5)
            if is_element_present("id","cn.longmaster.health:id/experience"):
                driver.find_element_by_id("cn.longmaster.health:id/experience").click()
                break#这里使用循环滑动，因为一次操作时间太快来不及界面显示
            if i == 9:
                print("在滑动区域出错。")
                sys.exit(0)
        time.sleep(1)
    if is_element_present("id","cn.longmaster.health:id/btn_statement_dialog_i_see")!=False:
        driver.find_element_by_id("cn.longmaster.health:id/btn_statement_dialog_i_see").click()
        time.sleep(1)

    print("第一次进入通过")
    driver.find_element_by_id("cn.longmaster.health:id/mine").click()
    time.sleep(1)
    #进入登录判断
    if is_element_present("id","cn.longmaster.health:id/fragment_mine_login_rl"):
        driver.find_element_by_id("cn.longmaster.health:id/fragment_mine_login_rl").click()
        time.sleep(1)
    #智能密码管理
        if is_element_present("id","com.miui.contentcatcher:id/auto_fill_data_name"):
            driver.find_element_by_id("com.miui.contentcatcher:id/auto_fill_data_name").click()
            time.sleep(1)
        else:#这里是说明你的手机不是小米没有存入账号密码
            ele=driver.find_element_by_id("cn.longmaster.health:id/user_login_phone_num")
            ele.send_keys(User)
            ele=driver.find_element_by_id("cn.longmaster.health:id/user_login_password")
            ele.send_keys(Password)
            driver.find_element_by_id("cn.longmaster.health:id/user_login_btn").click()
    print("账号成功登陆。")
    time.sleep(5)#这里等待时间拉长因为害怕网络出现问题
    #到这里已经登录了
    #现在进行element列表排查

    if is_element_present("id","cn.longmaster.health:id/mine")!=False:
        driver.find_element_by_id("cn.longmaster.health:id/mine").click()
        time.sleep(2)
    else:
        print("切屏没有成功，测试结束")
        sys.exit(0)

    if is_element_present("id","cn.longmaster.health:id/fragment_mine_record")==True:
        driver.find_element_by_id("cn.longmaster.health:id/fragment_mine_record").click()
        time.sleep(5)#这一步需要读取数据，所以时间给长一点。
    else:
        print("登录没有成功，测试结束")
        sys.exit(0)
#至此登录环节结束！
#==================================================================================
    if is_element_present("id","cn.longmaster.health:id/name")!=False:
        list=driver.find_elements_by_id("cn.longmaster.health:id/name")#获得整个列表
    #这里所有的诊断医师名字都存在list中了
        for i in range(len(list)):
            print(list[i].text)
        time.sleep(1)
    else:
        print("没有拉取到数据，请检查网络!")
# ==================================================================================

# ==================================================================================
    pirnt("测试成功")
except:
    print(traceback.format_exc())

