using System;
using UnityEngine.SceneManagement;

namespace ET.Client
{
    [Event(SceneType.StateSync)]
    public class SceneChangeStart_AddComponent: AEvent<Scene, SceneChangeStart>
    {
        protected override async ETTask Run(Scene root, SceneChangeStart args)
        {
            try
            {
                Scene currentScene = root.CurrentScene();

                ResourcesLoaderComponent resourcesLoaderComponent = currentScene.GetComponent<ResourcesLoaderComponent>();
            
                // 加载场景资源
                await resourcesLoaderComponent.LoadSceneAsync($"Packages/cn.etetet.demores/Scenes/{currentScene.Name}.unity", LoadSceneMode.Single);
                // 切换到map场景

                currentScene.AddComponent<OperaComponent>();
                currentScene.AddComponent<MapManagerComponent>();
            }
            catch (Exception e)
            {
                Log.Error(e);
            }

        }
    }
}