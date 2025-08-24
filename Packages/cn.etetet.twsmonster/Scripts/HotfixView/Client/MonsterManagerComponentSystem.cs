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
    [FriendOfAttribute(typeof(ET.Client.UnitComponent_Client))]
    public static partial class MonsterManagerComponentSystem
    {
        [EntitySystem]
        private static void Awake(this ET.Client.MonsterManagerComponent self)
        {
            self.MonsterRoot = GameObject.Find("World").GetComponent<ReferenceCollector>().Get<Transform>("MonsterRoot");
        }

        [EntitySystem]
        private static void Destroy(this ET.Client.MonsterManagerComponent self)
        {
            self.Root().GetComponent<TimerComponent>().Remove(ref self.Timer_FinalMonster);
            self.ClearNormalMonsterTimer();
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
                        .NewRepeatedTimer((long)(createMonsterData.Interval * 1000), TimerInvokeType.CreateNormalMonster, self));
            }

            CreateMonsterData finalMonsterConfigData = gameLevelConfig.FinalMonsterConfigData;
            self.Timer_FinalMonster = self.Root().GetComponent<TimerComponent>()
                    .NewRepeatedTimer((long)(finalMonsterConfigData.Interval * 1000), TimerInvokeType.CreateFinalMonster, self);

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
            self.ClearNormalMonsterTimer();

            //清理小怪
            self.Root().CurrentScene().GetComponent<UnitComponent_Client>().RemoveAllMonster();
            GameLevelConfig gameLevelConfig = self.Root().CurrentScene().GetComponent<ChapterComponent>().GetGameLevelConfig();
            CreateMonsterData finalMonsterConfigData = gameLevelConfig.FinalMonsterConfigData;
            UnitFactory.CreateMonster(self.Root().CurrentScene(), finalMonsterConfigData.MonsterConfigId);
        }

        public static void ClearNormalMonsterTimer(this ET.Client.MonsterManagerComponent self)
        {
            //清理定时器
            for (int i = 0; i < self.Timers_NormalMonster.Count; i++)
            {
                long time = self.Timers_NormalMonster[i];
                self.Root().GetComponent<TimerComponent>().Remove(ref time);
            }

            self.Timers_NormalMonster.Clear();
        }
    }
}