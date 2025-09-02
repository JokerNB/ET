using UnityEngine;

namespace ET.Client
{
    [Event(SceneType.Current)]
    public class AfterUnitCreate_CreateUnitView : AEvent<Scene, AfterUnitCreate>
    {
        protected override async ETTask Run(Scene scene, AfterUnitCreate args)
        {
            Unit_Client unit = args.Unit;
            // Unit View层
            string assetsPath = $"Packages/cn.etetet.twsunit/Assets/GameRes/";
            GameObject bundleGameObject = await scene.GetComponent<ResourcesLoaderComponent>()
                    .LoadAssetAsync<GameObject>($"{assetsPath}Bundles/{unit.Config.ResName}.prefab");

            GlobalComponent globalComponent = scene.Root().GetComponent<GlobalComponent>();
            GameObject go = UnityEngine.Object.Instantiate(bundleGameObject, globalComponent.Unit, true);
            go.transform.position = new Vector3(0, 0.001f, 0);

            unit.AddComponent<GameObjectComponent, GameObject, bool>(go, false, true);
            unit.AddComponent<AnimatorComponent>();
            unit.AddComponent<PlayerMoveComponent>();
            unit.AddComponent<CastComponent>();
            unit.AddComponent<BuffComponent>();
            await ETTask.CompletedTask;
        }
    }
}