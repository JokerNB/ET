using System.Collections.Generic;
using UnityEngine;

namespace ET.Client
{
    [ComponentOf(typeof(Scene))]
    public class MonsterManagerComponent : Entity, IAwake, IDestroy
    {
        public Transform MonsterRoot { get; set; }
        public List<long> Timers_NormalMonster = new List<long>();
        public long Timer_FinalMonster;
    }
}