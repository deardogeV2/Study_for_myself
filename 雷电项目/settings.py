class Settings():
    "存储设置"
    def __init__(self):
        self.screen_width=1200
        self.screen_height=800
        self.bg_color=(255,255,255)
        #速度设置
        self.ship_speed_factor=1.5

        self.bullet_speed_factor=3
        self.bullet_width=800
        self.bullet_height=15
        self.bullet_color=60,60,60#一样为元组
        self.bullets_allowed=3#允许子弹数量

        #外星人
        self.alien_speed_factor=0.5
        self.fleet_drop_speed=10
        #fleet_direction 1向右，-1想做
        self.fleet_direction = 1