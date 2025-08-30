using System;

namespace ET.Client
{
    [Actions(ActionType.NumericChange)]
    [FriendOfAttribute(typeof(ET.Client.Actions))]
    public class Actions_NumericChange : IActions
    {
        public void Run(Actions actions, ActionsRunType actionsRunType)
        {
            Unit_Client owner = actions.Owner;

            if (owner == null || owner.IsDisposed)
                return;

            Cast cast = actions.CastSelf;
            
            var numericType = actions.Config.NumericType;
            owner.NumericComponent.Change(numericType, cast.CastConfig.SelfActionParam[actions.idx]);
        }
    }
}