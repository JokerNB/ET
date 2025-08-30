namespace ET.Client
{
    [Actions(ActionType.AutoDispose)]
    [FriendOfAttribute(typeof(ET.Client.Actions))]
    public class Actions_AutoDispose : IActions
    {
        public void Run(Actions actions, ActionsRunType actionsRunType)
        {
            Cast cast = actions.CastSelf;
            Unit_Client skillUnit = actions.SkillUnit;
            long castDurTime = cast.GetComponent<NumericDataComponent>().GetAsLong(ENumericType.SkillDuration0);
            if (castDurTime > 0)
                skillUnit.AddComponent<UnitAutoDisposeComponent, long>(castDurTime, true);
        }
    }
}