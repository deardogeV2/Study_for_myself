class Settings():
    "存储设置"
    def __init__(self):
        self.screen_width=1200
        self.screen_height=800
        self.bg_color=(255,255,255)
        #飞船设置
        self.ship_speed_factor=1.5
        self.ship_limit=3

        #子弹设置
        self.bullet_speed_factor=3
        self.bullet_width=800
        self.bullet_height=15
        self.bullet_color=60,60,60#一样为元组
        self.bullets_allowed=3#允许子弹数量

        #外星人
        self.alien_speed_factor=0.5
        self.fleet_drop_speed=10
        #fleet_direction 1向右，-1向右
        self.fleet_direction = 1

        #什么样的速度加快游戏接节奏
        self.speedup_scale=1.1
        self.initialize_dynamic_settings()

        #外星人击败得分
        self.alien_points=50
        self.score_scale=1.5

    #速度重置函数
    def initialize_dynamic_settings(self):
        self.ship_speed_factor=1.5
        self.bullet_speed_factor=3
        self.alien_speed_factor=1
        self.fleet_direction=1

    #加速函数
    def initialize_speed(self):
        self.ship_speed_factor*=self.speedup_scale
        self.bullet_speed_factor*=self.speedup_scale
        self.alien_speed_factor*=self.speedup_scale

        self.alien_points=int(self.alien_points*self.score_scale)
        print(self.alien_points)

