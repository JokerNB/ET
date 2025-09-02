using UnityEngine;

namespace ET.Client
{
    [Actions(ActionType.DirLineMove)]
    [FriendOfAttribute(typeof(ET.Client.Actions))]
    [FriendOfAttribute(typeof(ET.Client.PlayerMoveComponent))]
    [FriendOfAttribute(typeof(ET.Client.Cast))]
    public class Actions_DirLineMove : IActions
    {
        public void Run(Actions actions, ActionsRunType actionsRunType)
        {
            //移动方向直线移动
            Unit_Client unitClient = actions.Owner;
            Unit_Client skillUnit = actions.SkillUnit;
            if (unitClient == null || unitClient.IsDisposed)
                return;
            Cast cast = actions.CastSelf;
            if (unitClient.UnitType == ET.UnitType.Player)
            {
                float speed = cast.GetComponent<NumericDataComponent>().GetAsFloat(ENumericType.SkillMoveSpeed0);
                this.DoPlayerMove(unitClient, skillUnit, speed);
            }
            else
                this.DoOtherMove(unitClient, skillUnit);
        }

        public void DoPlayerMove(Unit_Client owner, Unit_Client skillUnit, float speed)
        {
            float horizontalinput = owner.GetComponent<PlayerMoveComponent>().horizontalinput;
            float verticalinput = owner.GetComponent<PlayerMoveComponent>().Verticalinput;
            bool flipX = owner.GetComponent<PlayerMoveComponent>().SpriteRenderer.flipX;

            Vector2 moveDir = new Vector2(horizontalinput, verticalinput).normalized;
            skillUnit.GetComponent<UnitMoveComponent>().InitAndMove(flipX, moveDir, speed);
        }

        public void DoOtherMove(Unit_Client unitClient, Unit_Client skillUnit)
        {
        }
    }
}