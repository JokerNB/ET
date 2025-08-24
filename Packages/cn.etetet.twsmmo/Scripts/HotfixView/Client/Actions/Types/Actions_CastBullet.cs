namespace ET.Client
{
    [Actions(ActionsType.CastBullet)]
    [FriendOfAttribute(typeof(ET.Client.Cast))]
    public class Actions_CastBullet : IActions
    {
        public void Run(Actions actions, ActionsRunType actionsRunType)
        {
            Cast cast = actions.CastSelf;
            if (cast == null || actionsRunType != ActionsRunType.CastHit)
                return;
            if (cast.Target.Count <= 0)
                return;

            ActionConfig config = actions.Config;
            UnitComponent_Client unitComponent = actions.Root().CurrentScene().GetComponent<UnitComponent_Client>();
            foreach (long uid in cast.Target)
            {
                Unit_Client unit = unitComponent.Get(uid);
                if(unit == null)
                    continue;

                int unitId = int.Parse(config.Param[0]);
                int bulletId = int.Parse(config.Param[1]);

                Unit_Client bullet = UnitFactory.CreateBullet(cast.Root().CurrentScene(), ((Unit_Client)cast.Caster).Id, unitId, bulletId, unit.GetUnitPosition());
                bullet.GetComponent<BulletComponent>().Start();
            }
        }
    }
}