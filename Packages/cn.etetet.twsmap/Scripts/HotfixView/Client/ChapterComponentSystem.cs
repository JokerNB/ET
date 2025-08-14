using System.Collections.Generic;

namespace ET.Client
{
    [EntitySystemOf(typeof(ChapterComponent))]
    public static partial class ChapterComponentSystem
    {
        [EntitySystem]
        private static void Awake(this ET.Client.ChapterComponent self)
        {
            var chapterConfigs = ChapterConfigCategory.Instance.GetAll();
            foreach (var KV in chapterConfigs)
            {
                var chapterConfig = KV.Value;
                self.ChapterLevelsDic[KV.Key] = new List<int>(chapterConfig.GameLevels);
            }
        }

        public static bool isContainsGameLevel(this ET.Client.ChapterComponent self, int chapterConfigId, int idx_GameLevel)
        {
            if (!self.ChapterLevelsDic.TryGetValue(chapterConfigId, out var gameLevelConfigs))
                return false;
            if (idx_GameLevel < 0 || idx_GameLevel >= gameLevelConfigs.Count)
                return false;
            return true;
        }

        public static GameLevelConfig GetGameLevelConfig(this ET.Client.ChapterComponent self)
        {
            // int chapterConfigId = self.CurrentSelectChapter;
            // int idx_GameLevel = self.CurrentSelectLevel;
            int chapterConfigId = 1;
            int idx_GameLevel = 1;
            if (!self.isContainsGameLevel(chapterConfigId, idx_GameLevel))
            {
                Log.Error($"Dont Contains GameLevelConfig, Please Check :{chapterConfigId}.{idx_GameLevel} ");
                return null;
            }

            return GameLevelConfigCategory.Instance.Get(self.ChapterLevelsDic[chapterConfigId][idx_GameLevel]);
        }

        public static void SetCurrentSelect(this ET.Client.ChapterComponent self, int chapterConfigId, int idx_GameLevel)
        {
            self.CurrentSelectChapter = chapterConfigId;
            self.CurrentSelectLevel = idx_GameLevel;
        }

        public static void ClearCurrentSelect(this ET.Client.ChapterComponent self)
        {
            self.CurrentSelectChapter = 0;
            self.CurrentSelectLevel = 0;
        }
    }
}