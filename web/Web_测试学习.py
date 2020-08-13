from selenium import webdriver
from selenium.common.exceptions import NoSuchElementException
import time
driver=webdriver.Chrome(executable_path=r"F:\Chrome\chromedriver_win32\chromedriver.exe")#选择驱动路径，打开驱动
#WebDriver实例对象，可以看做遥控器
#==============================
def is_element_present(how,what):
    try:
        driver.find_element(by=how,value=what)#查找参数进行查找
    except NoSuchElementException:#如果存在则这里是
        return False
    return True

#==============================
driver.get("http://www.51job.com")#登录网页
time.sleep(3)
ele=driver.find_element_by_id("kwdselectid")
ele.send_keys("Python")
time.sleep(1)

ele=driver.find_element_by_id("work_position_click")
ele.click()
time.sleep(1)

eles = driver.find_elements_by_css_selector("#work_position_click_ip_location em[class=on]")
for el in eles:
    el.click()#把每一个亮着的点灭了
time.sleep(1)

ele=driver.find_element_by_id("work_position_click_center_right_list_category_000000_010000")
ele.click()#点上北京
time.sleep(1)

ele=driver.find_element_by_id("work_position_click_bottom_save")
ele.click()#点击地址保存
time.sleep(1)

ele=driver.find_element_by_css_selector(".ush   button")
ele.click()#点击搜索
time.sleep(3)

eles=driver.find_elements_by_css_selector('a[class="el"]')
for ele in eles:
    jname=ele.find_element_by_css_selector('span[class="jname at"]').text
    tags=ele.find_element_by_css_selector('span[class="d at"]').text
    sal=ele.find_element_by_css_selector('span[class="sal"]').text
    print(jname+'|'+tags+'|'+sal)