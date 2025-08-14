using System.Collections.Generic;

namespace ET.Client
{
    [ComponentOf(typeof(Scene))]
    public class ChapterComponent : Entity, IAwake
    {
        public Dictionary<int, List<int>> ChapterLevelsDic = new Dictionary<int, List<int>>();
        public int CurrentSelectChapter = 0;
        public int CurrentSelectLevel = 0;
    }
}