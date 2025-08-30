using UnityEngine;

namespace ET.Client
{
    [Actions(ActionType.UpParabola)]
    [FriendOfAttribute(typeof(ET.Client.Actions))]
    [FriendOfAttribute(typeof(ET.Client.PlayerMoveComponent))]
    public class Actions_UpParabola : IActions
    {
        public void Run(Actions actions, ActionsRunType actionsRunType)
        {
            //抛物线移动
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


            if (Mathf.Abs(horizontalinput) <= 0.001f)
                horizontalinput = 0.2f;
                
            Vector2 moveDir = new Vector2(horizontalinput, 1).normalized;
            skillUnit.GetComponent<UnitMoveComponent>().InitAndMove(flipX, moveDir, speed, owner.GetUnitPosition());
        }

        public void DoOtherMove(Unit_Client unitClient, Unit_Client skillUnit)
        {
        }
    }
}

