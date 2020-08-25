import sys
import pygame
from bullet import Bullet
from alien import Alien
from time import sleep

#相应鼠标事件
def check_events(ai_settings,screen,stats,sb,play_button,ship,aliens,bullets):
    for event in pygame.event.get():
        if event.type == pygame.QUIT:
            sys.exit()
        elif event.type==pygame.MOUSEBUTTONDOWN:
            mouse_x,mouse_y=pygame.mouse.get_pos()
            check_play_button(ai_settings,screen,stats,sb,play_button,ship,aliens,bullets,mouse_x,mouse_y)
        #非退出键
        elif event.type == pygame.KEYDOWN:  # 向右移动
            check_keydown_events(event,ai_settings,screen,ship,bullets)
        elif event.type == pygame.KEYUP:
            check_keyup_events(event,ship)

#单击play，开始游戏
def check_play_button(ai_settings,screen,stats,sb,play_button,ship,aliens,bullets,mouse_x,mouse_y):
    button_clicked = play_button.rect.collidepoint(mouse_x,mouse_y)
    if button_clicked and  not stats.game_active:
        if play_button.rect.collidepoint(mouse_x,mouse_y):
            #隐藏光标
            pygame.mouse.set_visible(False)

            #重置游戏速度
            ai_settings.initialize_dynamic_settings()

            #重置记分牌图像
            sb.prep_score()
            sb.prep_level()
            sb.prep_high_score()
            sb.prep_ships()

            #重置游戏统计信息
            stats.rest_stats()
            stats.game_active = True

            #清空外星人和子弹列表
            aliens.empty()
            bullets.empty()

            #创建一群新的外星人，让飞船居中
            creat_fleet(ai_settings,screen,ship,aliens)
            ship.center_ship()
#按下键
def check_keydown_events(event,ai_settings,screen,ship,bullets):
    if event.key == pygame.K_RIGHT:
        ship.moving_right = True
    elif event.key == pygame.K_LEFT:
        ship.moving_left = True
    elif event.key == pygame.K_UP:
        ship.moving_up = True
    elif event.key == pygame.K_DOWN:
        ship.moving_down = True#鼠标
    elif event.key==pygame.K_SPACE:#按下空格键
        #创建一颗子弹
        if len(bullets)<ai_settings.bullets_allowed:
            new_bullet = Bullet(ai_settings, screen, ship)
            bullets.add(new_bullet)
    elif event.key==pygame.K_q:#按下结束键
        sys.exit()
#松开键
def check_keyup_events(event,ship):
    if event.key == pygame.K_RIGHT:
        ship.moving_right = False
    elif event.key == pygame.K_LEFT:
        ship.moving_left = False
    elif event.key == pygame.K_UP:
        ship.moving_up = False
    elif event.key == pygame.K_DOWN:
        ship.moving_down = False

def update_screen(ai_settings,screen,stats,sb,ship,aliens,bullets,play_button):
    screen.fill(ai_settings.bg_color)#先绘制底色，因为这个画布是层层叠加结构
    ship.blitme()
    #显示得分
    sb.show_score()
    aliens.draw(screen)#在屏幕绘制外星人
    for bullet in bullets.sprites():
        bullet.draw_bullet()
    if not stats.game_active:
        play_button.draw_button()
    #显示最近绘制的屏幕
    pygame.display.flip()

def update_bullets(ai_settings,screen,stats,sb,ship,aliens,bullets):
    # 子弹更新
    bullets.update()
    # 删除消失的子弹
    for bullet in bullets.copy():
        if bullet.rect.bottom <= 0:
            bullets.remove(bullet)
    #检查子弹是否击中外星人
    check_bullet_alien_conllsions(ai_settings,screen,stats,sb,ship,aliens,bullets)

# 检查子弹是否集中外星人方法
def check_bullet_alien_conllsions(ai_settings,screen,stats,sb,ship,aliens,bullets):
    collisions = pygame.sprite.groupcollide(bullets,aliens,True,True)

    #计分
    if collisions:
        for aliens in collisions.values():
            stats.score +=ai_settings.alien_points * len(aliens)
            sb.prep_score()

    #生成新外星人
    if len(aliens)==0:
        #提高等级
        stats.level+=1
        sb.prep_level()
        #清除子弹
        bullets.empty()
        #速度提高
        ai_settings.initialize_speed()
        #创建新的外星人
        creat_fleet(ai_settings,screen,ship,aliens)

def get_number_aliens_x(ai_settings,alien_width):
    #计算每行可以容纳多少个飞船
    available_space_x=ai_settings.screen_width-2*alien_width
    number_aliens_x = int (available_space_x/(2*alien_width))
    return number_aliens_x

def get_number_aliens_rows(ai_settings,ship_height,alien_height):
    #计算多少行外星人
    available_space_rows=(ai_settings.screen_height-
                       (3*alien_height)
                       -ship_height)
    number_rows=int(available_space_rows/(2*alien_height))
    return number_rows

def creat_alien(ai_settings,screen,aliens,alien_number,row_number):
    #外星人群，间距为宽度
    alien=Alien(ai_settings,screen)
    alien_width=alien.rect.width
    alien.x = alien_width + 2 * alien_width * alien_number
    alien.rect.x = alien.x
    alien.rect.y = alien.rect.height+2*alien.rect.height*row_number
    aliens.add(alien)

def creat_fleet(ai_settings,screen,ship,aliens):
    alien=Alien(ai_settings,screen)
    number_aliens_x=get_number_aliens_x(ai_settings,alien.rect.width)
    number_rows=get_number_aliens_rows(ai_settings,ship.rect.height,
                                      alien.rect.height)
    for row_number in range(number_rows):
        for alien_number in range(number_aliens_x):
            creat_alien(ai_settings,screen,aliens,alien_number,row_number)

#外星人速度更新
# def aliens_speed_change(ai_settings,aliens):

#更新外星人位置，包含边缘检查
def update_aliens(ai_settings,screen,stats,sb,ship,aliens,bullets):
    check_cleet_edges(ai_settings,aliens)
    aliens.update()
    #外星人跟飞船碰撞检车
    if pygame.sprite.spritecollideany(ship,aliens):
        ship_hit(ai_settings,screen,stats,sb,ship,aliens,bullets)
    check_aliens_bottom(ai_settings, screen, stats,sb,ship, aliens, bullets)#底边检测

#外星人边缘处理
def check_cleet_edges(ai_settings,aliens):
    for alien in aliens.sprites():
        if alien.check_edges():#边缘碰撞
            change_fleet_direction(ai_settings,aliens)
            break

#所有外星人改变他们的方向
def change_fleet_direction(ai_settings,aliens):
    for alien in aliens.sprites():
        alien.rect.y+=ai_settings.fleet_drop_speed
    ai_settings.fleet_direction*=-1

#响应呗外星人撞击的飞船
def ship_hit(ai_settings,screen,stats,sb,ship,aliens,bullets):
    if stats.ships_left>0:
        # ship_limit减少
        stats.ships_left -= 1

        # 清空外星人列表和子弹列表
        aliens.empty()
        bullets.empty()

        #更新记分牌
        sb.prep_ships()

        # 创建一群新的外星人，并且重新安置飞船到底部中央
        creat_fleet(ai_settings, screen, ship, aliens)
        ship.center_ship()

        # 死亡重启操作，可以自由发挥
        sleep(0.5)
    else:
        stats.game_active=False
        if stats.score>stats.high_score:
            stats.high_score=stats.score
        pygame.mouse.set_visible(True)

#检查外星人到达底部
def check_aliens_bottom(ai_settings, screen, stats,sb,ship, aliens, bullets):
    screen_rect=screen.get_rect()
    for alien in aliens.sprites():
        if alien.rect.bottom>=screen_rect.bottom:
            ship_hit(ai_settings,screen,stats,sb,ship,aliens,bullets)
            break

