using System.Collections.Generic;

namespace ET.Client
{
    [ChildOf(typeof(CastComponent))]
    public class Cast : Entity, IAwake<int>, IDestroy
    {
        public int ConfigId;

        public CastConfig CastConfig => CastConfigCategory.Instance.Get(this.ConfigId);

        public EntityRef<Entity> Caster;
        
        public List<long> Target = new List<long>();

        public long StartTime;
    }
}