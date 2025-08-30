namespace ET.Client
{
    [ChildOf(typeof(ActionsTempComponent))]
    public class Actions : Entity, IAwake<int, int>, IDestroy
    {
        public int ConfigId;
        public ActionConfig Config => ActionConfigCategory.Instance.Get(this.ConfigId);

        public EntityRef<Unit_Client> Caster;

        public EntityRef<Unit_Client> Owner;

        public EntityRef<Unit_Client> SkillUnit;

        public EntityRef<Cast> CastSelf => this.Parent.GetParent<Cast>();

        public EntityRef<Buff> BuffSelf => this.Parent.GetParent<Buff>();

        public int idx;
    }
}