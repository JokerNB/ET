using System.Collections.Generic;

namespace ET
{
    public struct CastActionTime
    {
        public int Index;
        public bool isSelfHit;
    }

    public partial class CastConfig
    {
        public List<int> Times = new List<int>();
        public MultiMap<int, CastActionTime> TimesDic = new MultiMap<int, CastActionTime>();

        public override void EndInit()
        {
            for (int i = 0; i < this.SelfHitActionTimes.Count; i++)
            {
                int time = this.SelfHitActionTimes[i];
                if (!this.Times.Contains(time))
                    this.Times.Add(time);
                this.TimesDic.Add(time, new CastActionTime
                {
                    Index = i,
                    isSelfHit = true
                });
            }

            for (int i = 0; i < this.HitActionTimes.Count; i++)
            {
                int time = this.HitActionTimes[i];
                if (!this.Times.Contains(time))
                    this.Times.Add(time);
                this.TimesDic.Add(time, new CastActionTime
                {
                    Index = i,
                    isSelfHit = false
                });
            }
            
            this.Times.Sort();
        }
    }
}