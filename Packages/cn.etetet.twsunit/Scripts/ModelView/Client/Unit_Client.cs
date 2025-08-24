namespace ET.Client
{
    [ChildOf(typeof(UnitComponent_Client))]
    public partial class Unit_Client : Entity, IAwake<int>, IDestroy
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