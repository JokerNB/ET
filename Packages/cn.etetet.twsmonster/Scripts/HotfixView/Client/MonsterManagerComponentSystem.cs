using System;
using UnityEngine;

namespace ET.Client
{
    [Invoke(TimerInvokeType.CreateNormalMonster)]
    public class CreateMonster_TimerHandler : ATimer<MonsterManagerComponent>
    {
        protected override void Run(MonsterManagerComponent t)
        {
            t?.CreateNormalMonster();
        }
    }
    
    [Invoke(TimerInvokeType.CreateFinalMonster)]
    public class CreateFinalMonster_TimerHandler : ATimer<MonsterManagerComponent>
    {
        protected override void Run(MonsterManagerComponent t)
        {
            t?.CreateFinalMonster();
        }
    }

    [EntitySystemOf(typeof(MonsterManagerComponent))]
    [FriendOfAttribute(typeof(ET.Client.ChapterComponent))]
    public static partial class MonsterManagerComponentSystem
    {
        [EntitySystem]
        private static void Awake(this ET.Client.MonsterManagerComponent self)
        {
            self.MonsterRoot = GameObject.Find("World").GetComponent<ReferenceCollector>().Get<Transform>("MonsterRoot");
        }

        public static async ETTask StartBattle(this ET.Client.MonsterManagerComponent self)
        {
            //TODO:暂时先直接获取，后续通过事件通知
            GameLevelConfig gameLevelConfig = self.Root().CurrentScene().GetComponent<ChapterComponent>().GetGameLevelConfig();
            foreach (CreateMonsterData createMonsterData in gameLevelConfig.MonsterConfigData)
            {
                //先创建一次
                self.CreateNormalMonster();
                self.Timers_NormalMonster.Add(self.Root().GetComponent<TimerComponent>()
                        .NewRepeatedTimer((long)(createMonsterData.Interval * 1000), TimerInvokeType.CreateNormalMonster,self));
            }

            CreateMonsterData finalMonsterConfigData = gameLevelConfig.FinalMonsterConfigData;
            self.Timer_FinalMonster = self.Root().GetComponent<TimerComponent>()
                    .NewRepeatedTimer((long)(finalMonsterConfigData.Interval * 1000), TimerInvokeType.CreateFinalMonster,self);

            await ETTask.CompletedTask;
        }

        public static void CreateNormalMonster(this ET.Client.MonsterManagerComponent self)
        {
            GameLevelConfig gameLevelConfig = self.Root().CurrentScene().GetComponent<ChapterComponent>().GetGameLevelConfig();
            CreateMonsterData? createMonsterData = default;
            foreach (CreateMonsterData monsterData in gameLevelConfig.MonsterConfigData)
            {
                UnitConfig unitConfig = UnitConfigCategory.Instance.Get(monsterData.MonsterConfigId);
                if (unitConfig.UnitType == UnitType.Monster)
                {
                    createMonsterData = monsterData;
                    break;
                }
            }

            if (!createMonsterData.HasValue)
            {
                Log.Error("monster config doesn't exist");
                return;
            }
            
            for (int i = 0; i < createMonsterData.Value.CreateNum; i++)
            {
                UnitFactory.CreateMonster(self.Root().CurrentScene(), createMonsterData.Value.MonsterConfigId);
            }
        }

        public static void CreateFinalMonster(this ET.Client.MonsterManagerComponent self)
        {
            //清理所有小怪
            self.Root().CurrentScene().GetComponent<UnitComponent>().RemoveAllMonster();
            GameLevelConfig gameLevelConfig = self.Root().CurrentScene().GetComponent<ChapterComponent>().GetGameLevelConfig();
            CreateMonsterData finalMonsterConfigData = gameLevelConfig.FinalMonsterConfigData;
            UnitFactory.CreateMonster(self.Root().CurrentScene(), finalMonsterConfigData.MonsterConfigId);
        }
    }
}