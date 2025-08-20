using System.Collections.Generic;
using UnityEngine;

namespace ET.Client
{
    [ComponentOf(typeof(Scene))]
    public class MonsterManagerComponent : Entity, IAwake
    {
        public Transform MonsterRoot;
        public List<long> Timers_NormalMonster = new List<long>();
        public long Timer_FinalMonster;
    }
}
