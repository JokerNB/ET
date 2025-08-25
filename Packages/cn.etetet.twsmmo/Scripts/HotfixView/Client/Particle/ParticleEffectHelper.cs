using UnityEngine;

namespace ET.Client
{
    public static class ParticleEffectHelper
    {
        public static async ETTask<Unit_Client> CreateParticle(Unit_Client target, int configId)
        {
            ParticleEffectConfig config = ParticleEffectConfigCategory.Instance.Get(configId);
            string name = config.PrefabName;
            GameObject prefab = await target.Root().CurrentScene().GetComponent<ResourcesLoaderComponent>().LoadAssetAsync<GameObject>(name);
            GameObject go = UnityEngine.Object.Instantiate(prefab);
            if (config.IsFollowUnit)
                go.transform.SetParent(target.GetComponent<GameObjectComponent>().GameObject.transform, false);
            else
                go.transform.SetParent(target.Root().CurrentScene().GetComponent<MapManagerComponent>().ParticleRootTr, false);

            Unit_Client particleUnit = UnitFactory.CreateParticleUnit(target.Root().CurrentScene(), config.UnitConfigId);
            particleUnit.AddComponent<GameObjectComponent, GameObject>(go);
            go.transform.localPosition = new Vector3(config.InitPos[0], config.InitPos[1], config.InitPos[2]);
            go.transform.localScale = new Vector3(config.InitScale[0], config.InitScale[1], config.InitScale[2]);
            OutDurationTime(particleUnit, config.TotalTime).NoContext();
            return particleUnit;
        }

        public static async ETTask OutDurationTime(Unit_Client unit, float time)
        {
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