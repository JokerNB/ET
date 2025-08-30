using UnityEngine;

namespace ET.Client
{
    [Actions(ActionType.HitRotate)]
    [FriendOfAttribute(typeof(ET.Client.Actions))]
    [FriendOfAttribute(typeof(ET.Client.UnitMoveComponent))]
    public class Actions_HitRotate : IActions
    {
        public void Run(Actions actions, ActionsRunType actionsRunType)
        {
            Unit_Client skillUnit = actions.SkillUnit;
            var unitMoveComponent = skillUnit.GetComponent<UnitMoveComponent>();
            var dir = unitMoveComponent.dir;
            var contactPoint2D = unitMoveComponent.contactPoint;
            var reflectDir = Vector3.Reflect(dir, contactPoint2D.normal).normalized;
            if (Mathf.Abs(reflectDir.x) <= 0.01f)
                reflectDir.x = 0;
            if (Mathf.Abs(reflectDir.y) <= 0.01f)
                reflectDir.y = 0;
            unitMoveComponent.Contact(dir);
        }
    }
}

