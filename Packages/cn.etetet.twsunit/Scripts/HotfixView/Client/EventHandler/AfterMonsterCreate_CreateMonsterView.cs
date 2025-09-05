using UnityEngine;

namespace ET.Client
{
    [Event(SceneType.Current)]
    public class AfterMonsterCreate_CreateMonsterView : AEvent<Scene, AfterMonsterCreate>
    {
        protected override async ETTask Run(Scene scene, AfterMonsterCreate args)
        {
            Unit_Client unit = args.Unit;
            var parent = scene.GetComponent<MonsterManagerComponent>().MonsterRoot;
            GameObject go = await YIUIGameObjectPool.Inst.Get(unit.Config.ResName, parent);
            go.name = unit.ConfigId.ToString();
            go.transform.localScale = new Vector3(unit.Config.InitScale[0], unit.Config.InitScale[1]);

            bool isFromPool = unit.IsFromPool;
            unit.AddComponent<GameObjectComponent, GameObject, bool>(go, true, isFromPool);
            unit.AddComponent<MonsterMoveComponent>(isFromPool);
            unit.AddComponent<CastComponent>(isFromPool);
            unit.AddComponent<BuffComponent>(isFromPool);
            await ETTask.CompletedTask;
        }
    }
}