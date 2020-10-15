class GameStats():
    #跟踪游戏的统计信息
    def __init__(self,ai_settings):
        self.ai_settings=ai_settings
        self.rest_stats()
        self.game_active=False

        #最高分数
        self.high_score=3000

        #玩家等级
        self.level=1

    # 初始化游戏运行期间可能变化的统计信息
    def rest_stats(self):
        self.ships_left=self.ai_settings.ship_limit

        #分数重置
        self.score=0