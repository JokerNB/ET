using UnityEngine;

namespace ET.Client
{
    [Event(SceneType.Current)]
    public class AfterMonsterCreate_CreateMonsterView : AEvent<Scene, AfterMonsterCreate>
    {
        protected override async ETTask Run(Scene scene, AfterMonsterCreate args)
        {
            Unit unit = args.Unit;
            unit.AddComponent<MonsterGameObjectComponent>();
            await ETTask.CompletedTask;
        }
    }
}