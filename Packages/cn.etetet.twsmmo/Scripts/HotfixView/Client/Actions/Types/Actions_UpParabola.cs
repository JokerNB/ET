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
            float speed = cast.GetComponent<NumericDataComponent>().GetAsFloat(ENumericType.SkillMoveSpeed0);
            float horizontalinput = unitClient.GetComponent<PlayerMoveComponent>().horizontalinput;
            bool flipX = unitClient.GetComponent<PlayerMoveComponent>().SpriteRenderer.flipX;

            if (Mathf.Abs(horizontalinput) <= 0.001f)
                horizontalinput = flipX ? -0.2f : 0.2f;

            Vector2 moveDir = new Vector2(horizontalinput, 1).normalized;
            skillUnit.GetComponent<UnitMoveComponent>().InitAndMove(flipX, moveDir, speed);
        }
    }
}