import sys,pygame
from settings import Settings
from ship import Ship
import game_function as gf
from pygame.sprite import Group
from alien import  Alien

def run_game():
    pygame.init()
    ai_settings=Settings()
    screen = pygame.display.set_mode((ai_settings.screen_width,
                                     ai_settings.screen_height))
    ship=Ship(ai_settings,screen)#在出生点创造一艘飞船

    #创建子弹编组
    bullets=Group()
    #创造外星人
    aliens=Group()
    gf.creat_fleet(ai_settings,screen,ship,aliens)

    while True:
        #键盘鼠标监听
        gf.check_events(ai_settings,screen,ship,bullets)
        #飞船位置更新
        ship.update()
        #更新子弹
        gf.update_bullets(ai_settings,screen,ship,aliens,bullets)
        #更新机器人
        gf.update_aliens(ai_settings,aliens)
        #更新画布
        gf.update_screen(ai_settings,screen,ship,aliens,bullets)


run_game()