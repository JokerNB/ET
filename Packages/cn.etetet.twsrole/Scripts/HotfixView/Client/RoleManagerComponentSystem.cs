using System;
using System.Collections.Generic;

namespace ET.Client
{
    /// <summary>
    /// Author  TWS
    /// Date    2025.8.12
    /// Desc    角色
    /// </summary>
    [FriendOf(typeof(RoleManagerComponent))]
    [EntitySystemOf(typeof(RoleManagerComponent))]
    public static partial class RoleManagerComponentSystem
    {
        #region ObjectSystem

        [EntitySystem]
        private static void Awake(this RoleManagerComponent self)
        {
        }

        #endregion
    }
}