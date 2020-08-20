import pygame
class Ship():
    def __init__(self,ai_settings,screen):#初始位置
        self.screen=screen
        #移动标志
        self.moving_right=False
        self.moving_left=False
        self.moving_up=False
        self.moving_down=False
        #图像选择
        self.image=pygame.image.load(r'C:\Users\Administrator\Desktop\1.bmp')
        self.rect=self.image.get_rect()
        self.screen_rect=screen.get_rect()
        #位置
        self.ai_settings=ai_settings
        #center存储小数据设置，同时赋予初始位置
        self.centerx = float(self.screen_rect.centerx)
        self.centery = float(self.screen_rect.bottom-self.rect.bottom/2)

    def blitme(self):
        self.screen.blit(self.image,self.rect)

    def update(self):
        #"根据移动标志调整飞船位置",同时附加边界限制
        if self.moving_right and self.rect.right<self.screen_rect.right:
            self.centerx +=self.ai_settings.ship_speed_factor
        if self.moving_left and self.rect.left>0:#等于true执行
            self.centerx -=self.ai_settings.ship_speed_factor
        if self.moving_up and self.rect.top>0:
            self.centery -=self.ai_settings.ship_speed_factor
        if self.moving_down and self.rect.bottom<self.screen_rect.bottom:
            self.centery +=self.ai_settings.ship_speed_factor



        #根据新位置更新船体实际位置
        self.rect.centerx=self.centerx
        self.rect.centery=self.centery
