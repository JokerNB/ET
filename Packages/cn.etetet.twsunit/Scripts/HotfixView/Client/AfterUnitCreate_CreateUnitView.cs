using UnityEngine;

namespace ET.Client
{
    [Event(SceneType.Current)]
    public class AfterUnitCreate_CreateUnitView : AEvent<Scene, AfterUnitCreate>
    {
        protected override async ETTask Run(Scene scene, AfterUnitCreate args)
        {
            Unit unit = args.Unit;
            // Unit View层
            string assetsPath = $"Packages/cn.etetet.twsunit/Assets/GameRes/";
            GameObject bundleGameObject = await scene.GetComponent<ResourcesLoaderComponent>()
                    .LoadAssetAsync<GameObject>($"{assetsPath}Bundles/Unit/Unit.prefab");
            Sprite sprite = await scene.GetComponent<ResourcesLoaderComponent>()
                    .LoadAssetAsync<Sprite>($"{assetsPath}Atlas/{UnitConfigCategory.Instance.Get(unit.ConfigId).ResName}");

            GlobalComponent globalComponent = scene.Root().GetComponent<GlobalComponent>();
            GameObject go = UnityEngine.Object.Instantiate(bundleGameObject, globalComponent.Unit, true);
            go.transform.position = Vector3.zero;
            go.GetComponentInChildren<SpriteRenderer>().sprite = sprite;

            unit.AddComponent<GameObjectComponent, GameObject>(go);
            unit.AddComponent<AnimatorComponent>();
            await ETTask.CompletedTask;
        }
    }
}