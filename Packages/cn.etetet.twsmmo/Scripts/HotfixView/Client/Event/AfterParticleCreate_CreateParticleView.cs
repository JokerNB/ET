using UnityEngine;

namespace ET.Client
{
    [Event(SceneType.Current)]
    public class AfterParticleCreate_CreateParticleView : AEvent<Scene, AfterParticleCreate>
    {
        protected override async ETTask Run(Scene scene, AfterParticleCreate a)
        {
            Unit_Client caster = a.Caster;
            Unit_Client owner = a.Owner;
            Unit_Client particleUnit = a.Unit_Particle;
            if (caster == null || caster.IsDisposed)
                return;
            if (owner == null || owner.IsDisposed)
                return;

            string name = particleUnit.Config.ResName;
            GameObject prefab = await scene.GetComponent<ResourcesLoaderComponent>()
                    .LoadAssetAsync<GameObject>($"Packages/cn.etetet.twsmmo/Assets/GameRes/Bundles/Particle/{name}.prefab");
            GameObject go = UnityEngine.Object.Instantiate(prefab);
            bool IsForbidFollowUnit = particleUnit.NumericComponent.GetAsBool(ENumericType.ForbidFollow0);
            if (IsForbidFollowUnit)
                go.transform.SetParent(owner.GetComponent<GameObjectComponent>().GameObject.transform, false);
            else
                go.transform.SetParent(owner.Root().CurrentScene().GetComponent<MapManagerComponent>().ParticleRootTr, false);

            var config = particleUnit.Config;
            particleUnit.AddComponent<GameObjectComponent, GameObject, bool>(go, false, true);
            InitCastData initCastData = config.InitCastDatas[0];
            go.transform.localPosition = new Vector3(initCastData.InitCastPos[0], initCastData.InitCastPos[1], initCastData.InitCastPos[2]);
            go.transform.localScale = new Vector3(initCastData.InitCastScale[0], initCastData.InitCastScale[1], initCastData.InitCastScale[2]);

            OutDurationTime(particleUnit).NoContext();
        }

        public static async ETTask OutDurationTime(Unit_Client unit)
        {
            float time = unit.NumericComponent.GetAsFloat(ENumericType.SkillDuration0);
            if (time <= 0)
                return;
            long instanceId = unit.InstanceId;
            await unit.Root().GetComponent<TimerComponent>().WaitAsync((long)time);
            if (unit.InstanceId != instanceId)
                return;
            unit.Root().CurrentScene().GetComponent<UnitComponent_Client>().Remove(unit.Id);
        }
    }
}