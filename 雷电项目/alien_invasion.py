import sys,pygame
from settings import Settings
from ship import Ship
import game_function as gf
from pygame.sprite import Group
from alien import  Alien
from game_stats import GameStats
from button import Button
from scoreboard import Scoreboard

def run_game():
    pygame.init()
    ai_settings=Settings()
    screen = pygame.display.set_mode((ai_settings.screen_width,
                                     ai_settings.screen_height))
    play_button = Button(ai_settings,screen,'Play')

    ship=Ship(ai_settings,screen)#在出生点创造一艘飞船
    stats=GameStats(ai_settings)#创建游戏统计信息实例
    sb=Scoreboard(ai_settings,screen,stats)#得分实例

    #创建子弹编组
    bullets=Group()
    #创造外星人
    aliens=Group()
    gf.creat_fleet(ai_settings,screen,ship,aliens)

    while True:
        #键盘鼠标监听
        gf.check_events(ai_settings,screen,stats,sb,play_button,ship,aliens,bullets)

        if stats.game_active:
            #飞船位置更新
            ship.update()
            #更新子弹
            gf.update_bullets(ai_settings,screen,stats,sb,ship,aliens,bullets)
            #更新机器人
            gf.update_aliens(ai_settings,screen,stats,sb,ship,aliens,bullets)
        #更新画布
        gf.update_screen(ai_settings,screen,stats,sb,ship,aliens,bullets,play_button)

run_game()