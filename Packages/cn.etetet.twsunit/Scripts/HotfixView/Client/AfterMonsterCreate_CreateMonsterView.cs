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
            GameObject go = await YIUIGameObjectPool.Inst.Get("Monster", parent);
            var referenceCollector = go.GetComponent<ReferenceCollector>();
            var spriteRenderer = referenceCollector.Get<SpriteRenderer>("Sprite");
            Sprite sprite = await scene.GetComponent<ResourcesLoaderComponent>()
                    .LoadAssetAsync<Sprite>($"Packages/cn.etetet.twsmonster/Assets/GameRes/Atlas/{unit.Config().ResName}");
            spriteRenderer.sprite = sprite;
            go.name = unit.ConfigId.ToString();

            unit.AddComponent<GameObjectComponent, GameObject>(go);
            unit.AddComponent<MonsterMoveComponent>();
            unit.AddComponent<CastComponent>();
            unit.AddComponent<BuffComponent>();
            await ETTask.CompletedTask;
        }
    }
}