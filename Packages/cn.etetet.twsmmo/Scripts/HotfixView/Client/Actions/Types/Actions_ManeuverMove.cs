using UnityEngine;

namespace ET.Client
{
    [Actions(ActionType.ManeuverMove)]
    [FriendOfAttribute(typeof(ET.Client.Actions))]
    [FriendOfAttribute(typeof(ET.Client.PlayerMoveComponent))]
    public class Actions_ManeuverMove : IActions
    {
        public void Run(Actions actions, ActionsRunType actionsRunType)
        {
            //回旋移动
            Unit_Client unitClient = actions.Owner;
            Unit_Client skillUnit = actions.SkillUnit;
            if (unitClient == null || unitClient.IsDisposed)
                return;
            Cast cast = actions.CastSelf;

            float horizontalinput = unitClient.GetComponent<PlayerMoveComponent>().horizontalinput;
            float verticalinput = unitClient.GetComponent<PlayerMoveComponent>().Verticalinput;

            Vector2 moveDir = new Vector2(horizontalinput, verticalinput).normalized;
            float speed = cast.GetComponent<NumericDataComponent>().GetAsFloat(ENumericType.SkillMoveSpeed0);
            skillUnit.GetComponent<UnitMoveComponent>().MoveByManeuver(moveDir, speed).NoContext();
        }
    }
}