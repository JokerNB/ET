using UnityEngine;

namespace ET.Client
{
    [Actions(ActionType.MoveToMouse)]
    [FriendOfAttribute(typeof(ET.Client.Actions))]
    [FriendOfAttribute(typeof(ET.Client.PlayerMoveComponent))]
    public class Actions_MoveToMouse : IActions
    {
        public void Run(Actions actions, ActionsRunType actionsRunType)
        {
            //向鼠标方向移动
            Unit_Client unitClient = actions.Owner;
            Unit_Client skillUnit = actions.SkillUnit;
            Cast cast = actions.CastSelf;
            Vector2 mousePos = cast.Root().CurrentScene().GetComponent<CameraComponent>().GetInputMousePos();
            var dir = (mousePos - unitClient.GetSelfPosition()).normalized;
            float speed = cast.GetComponent<NumericDataComponent>().GetAsFloat(ENumericType.SkillMoveSpeed0);
            skillUnit.GetComponent<UnitMoveComponent>().MoveToMouse(dir, speed);
        }
    }
}