using System.Diagnostics;

namespace ET.Server
{
    [ChildOf(typeof(UnitComponent))]
    [DebuggerDisplay("ViewName,nq")]
    public partial class Unit : Entity, IAwake<int>, IGetComponentSys
    {
        public int ConfigId { get; set; } //配置表id
        public UnitConfig Config => UnitConfigCategory.Instance.Get(this.ConfigId);
        public UnitType UnitType => Config.UnitType;

        protected override string ViewName
        {
            get
            {
                return $"{this.GetType().FullName} ({this.Id})";
            }
        }
    }
}