using System;
using System.Collections.Generic;
using System.IO;

namespace ET.Client
{
    [Event(SceneType.StateSync)]
    public class EntryEvent3_InitClient : AEvent<Scene, EntryEvent3>
    {
        protected override async ETTask Run(Scene root, EntryEvent3 args)
        {
            root.AddComponent<GlobalComponent>();
            root.AddComponent<ResourcesLoaderComponent>();
            root.AddComponent<PlayerComponent>();
            root.AddComponent<CurrentScenesComponent>();
            root.AddComponent<ArchiveInfoManagerComponent_Client>();

            root.AddComponent<FUIAssetComponent, bool>(true);
            var fuiComponent = root.AddComponent<FUIComponent>();
            await fuiComponent.ShowPanelAsync<LoginUI>();

            await EventSystem.Instance.PublishAsync(root, new AppStartInitFinish());
        }
    }
}