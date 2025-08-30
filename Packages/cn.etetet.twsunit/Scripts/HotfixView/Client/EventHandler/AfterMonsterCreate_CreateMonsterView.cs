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

            unit.AddComponent<GameObjectComponent, GameObject, bool>(go, true);
            unit.AddComponent<MonsterMoveComponent>();
            unit.AddComponent<CastComponent>();
            unit.AddComponent<BuffComponent>();
            await ETTask.CompletedTask;
        }
    }
}