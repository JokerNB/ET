using System;
using System.Collections.Generic;
using UnityEngine;

namespace ET.Client
{
    /// <summary>
    /// Author  TWS
    /// Date    2025.8.12
    /// Desc    
    /// </summary>
    [FriendOf(typeof(RoleChild))]
    [EntitySystemOf(typeof(RoleChild))]
    public static partial class RoleChildSystem
    {
        [EntitySystem]
        private static void Awake(this RoleChild self, int configId)
        {
            self.ConfigId = configId;
        }

        public static async ETTask LoadRes(this RoleChild self)
        {
            Sprite sprite = await self.Root().CurrentScene().GetComponent<ResourcesLoaderComponent>().LoadAssetAsync<Sprite>(self.roleConfig.ResPath);
            
        }
    }
}