import pygame
from pygame.sprite import Sprite

class Bullet(Sprite):
    #发射子弹的类
    def __init__(self,ai_aisettings,screen,ship):
        #飞船所处位置创建一个飞弹
        super(Bullet,self).__init__()#super方法
        self.screen=screen
        #在（0,0）处创建一个表示子弹的形状，再设置位置
        self.rect=pygame.Rect(0,0,ai_aisettings.bullet_width,
                              ai_aisettings.bullet_height)
        self.rect.centerx=ship.rect.centerx
        self.rect.top=ship.rect.top
        self.y=float(self.rect.y)
        self.color=ai_aisettings.bullet_color
        self.speed_factor = ai_aisettings.bullet_speed_factor

    # 向上移动子弹
    def update(self):

        self.y-=self.speed_factor
        self.rect.y=self.y

    #在屏幕上绘制子弹
    def draw_bullet(self):
        pygame.draw.rect(self.screen,self.color,self.rect)

