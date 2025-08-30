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
            
            skillUnit.AddComponent<GameObjectComponent, GameObject, bool>(go, true);
            skillUnit.AddComponent<UnitMoveComponent, int, long>(a.castConfigId, ownerUnit.Id);
            
            Cast cast = a.castSelf;
            cast.Cast(skillUnit);

            await ETTask.CompletedTask;
        }
    }
}