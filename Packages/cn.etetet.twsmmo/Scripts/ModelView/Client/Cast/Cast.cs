using System.Collections.Generic;

namespace ET.Client
{
    [ChildOf(typeof(CastComponent))]
    public class Cast : Entity, IAwake<int, Unit_Client>, IDestroy, INumericHandlerDynamic<Cast, NumericChange>
    {
        public int ConfigId;

        public CastConfig CastConfig => CastConfigCategory.Instance.Get(this.ConfigId);

        public EntityRef<Unit_Client> OwnerUnit;

        public long Timer = default;
    }
}