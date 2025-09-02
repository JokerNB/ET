using UnityEngine;

namespace ET.Client
{
    [Actions(ActionType.SetPos)]
    [FriendOfAttribute(typeof(ET.Client.Actions))]
    public class Actions_SetPos : IActions
    {
        public void Run(Actions actions, ActionsRunType actionsRunType)
        {
            Unit_Client unitClient = actions.Owner;
            Unit_Client skillUnit = actions.SkillUnit;
            if (unitClient == null || unitClient.IsDisposed)
                return;
            Cast cast = actions.CastSelf;
            skillUnit.GetComponent<GameObjectComponent>()
                    .SetPosition(unitClient.GetSelfPosition() + new Vector2(cast.CastConfig.InitCastPos[0], cast.CastConfig.InitCastPos[1]));
            skillUnit.GetComponent<GameObjectComponent>().SetScale(new Vector2(cast.CastConfig.InitCastScale[0], cast.CastConfig.InitCastScale[1]));
        }
    }
}