using UnityEngine;

namespace ET.Client
{
    [Event(SceneType.Current)]
    public class AfterSkillCreate_CreatSkillView : AEvent<Scene, AfterSkillCreate>
    {
        protected override async ETTask Run(Scene scene, AfterSkillCreate a)
        {
            Unit_Client skillUnit = a.SkillUnit;
            Unit_Client ownerUnit = a.OwnerUnit;
            CastConfig castConfig = CastConfigCategory.Instance.Get(a.castConfigId);
            GameObject go = await YIUIGameObjectPool.Inst.Get(castConfig.ResName, scene.GetComponent<MapManagerComponent>().CastRootTr);

            bool isFromPoll = skillUnit.IsFromPool;
            skillUnit.AddComponent<GameObjectComponent, GameObject, bool>(go, isFromPoll, true);
            skillUnit.AddComponent<UnitMoveComponent, int, long>(a.castConfigId, ownerUnit.Id, isFromPoll);

            Cast cast = a.castSelf;
            cast.Cast(skillUnit);

            await ETTask.CompletedTask;
        }
    }
}