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
            var numericValueType = numericType.GetNumericValueType();
            switch (numericValueType)
            {
                case ENumericValueType.Int:
                case ENumericValueType.Long:
                case ENumericValueType.Float:
                    owner.NumericComponent.Change(numericType, cast.CastConfig.SelfActionParam[actions.idx]);
                    break;
                case ENumericValueType.Bool:
                    owner.NumericComponent.Change(numericType, cast.CastConfig.SelfActionParamBool[actions.idx]);
                    break;
                default:
                {
                    Log.Error($"Actions NumericChange ValueType not supported: {numericValueType}");
                    return;
                }
            }
        }
    }
}