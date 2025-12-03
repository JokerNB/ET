using System.Collections.Generic;
using UnityEngine.SceneManagement;
using YooAsset;

namespace ET.Client
{
    public static partial class ResourcesLoaderComponentSystem
    {
        public static T LoadAssetSync<T>(this ResourcesLoaderComponent self, string location)where T: UnityEngine.Object
        {
            HandleBase handler;
            if (!self.handlers.TryGetValue(location, out handler))
            {
                handler = YooAssets.LoadAssetSync<T>(location);
                self.handlers[location] = handler;
            }
            return (T)((AssetHandle)handler).AssetObject;
        }
        
        public static void UnloadAsset(this ResourcesLoaderComponent self, string location)
        {
            if (!self.handlers.TryGetValue(location, out HandleBase handleBase))
            {
                return;
            }
        
            switch (handleBase)
            {
                case AssetHandle handle:
                    handle.Release();
                    break;
                case AllAssetsHandle handle:
                    handle.Release();
                    break;
                case SubAssetsHandle handle:
                    handle.Release();
                    break;
                case RawFileHandle handle:
                    handle.Release();
                    break;
                case SceneHandle handle:
                    if (!handle.IsMainScene())
                    {
                        handle.UnloadAsync();
                    }
                    break;
            }
        }
    }
}

