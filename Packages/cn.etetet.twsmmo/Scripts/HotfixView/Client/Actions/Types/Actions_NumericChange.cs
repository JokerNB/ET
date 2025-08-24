using System;

namespace ET.Client
{
    [Actions(ActionsType.NumericChange)]
    [FriendOfAttribute(typeof(ET.Client.Actions))]
    public class Actions_NumericChange : IActions
    {
        public void Run(Actions actions, ActionsRunType actionsRunType)
        {
            Unit_Client owner = actions.Owner;

            if (owner == null || owner.IsDisposed)
                return;

            int numericType = int.Parse(actions.Config.Param[0]);
            int numericValue = int.Parse(actions.Config.Param[1]);

            switch (actionsRunType)
            {
                case ActionsRunType.CastHit:
                case ActionsRunType.BuffAdd:
                    //根据参数，增加或减少对应属性的数值
                    owner.NumericComponent.Change(numericType, numericValue);
                    break;
                case ActionsRunType.BuffRemove:
                    owner.NumericComponent.Change(numericType, numericValue);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(actionsRunType), actionsRunType, null);
            }
        }
    }
}