using UnityEngine;

namespace ET.Client
{
    [Event(SceneType.Current)]
    public class AfterUnitCreate_CreateUnitView : AEvent<Scene, AfterUnitCreate>
    {
        protected override async ETTask Run(Scene scene, AfterUnitCreate args)
        {
            Unit unit = args.Unit;
            GameObject bundleGameObject =
                    await scene.GetComponent<ResourcesLoaderComponent>().LoadAssetAsync<GameObject>($"{unit.Config.ResName}");

            GlobalComponent globalComponent = scene.Root().GetComponent<GlobalComponent>();
            GameObject go = UnityEngine.Object.Instantiate(bundleGameObject, globalComponent.Unit, true);
            go.transform.position = new Vector3(0, 0.001f, 0);

            unit.AddComponent<GameObjectComponent, GameObject>(go);
            unit.AddComponent<AnimatorComponent>();

            await ETTask.CompletedTask;
        }
    }
}