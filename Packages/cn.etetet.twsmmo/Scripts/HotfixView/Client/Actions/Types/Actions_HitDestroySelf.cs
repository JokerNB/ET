namespace ET.Client
{
    [Actions(ActionType.HitDestroySelf)]
    [FriendOfAttribute(typeof(ET.Client.Actions))]
    public class Actions_HitDestroySelf : IActions
    {
        public void Run(Actions actions, ActionsRunType actionsRunType)
        {
            Unit_Client skillUnit = actions.SkillUnit;
            skillUnit.Root().CurrentScene().GetComponent<UnitComponent_Client>().Remove(skillUnit.Id);
        }
    }
}