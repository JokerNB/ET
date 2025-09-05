using System;
using UnityEngine;
using YIUIFramework;
using System.Collections.Generic;

namespace ET.Client
{
    /// <summary>
    /// Author  TWS
    /// Date    2025.8.12
    /// Desc
    /// </summary>
    [FriendOf(typeof(MainUIViewComponent))]
    public static partial class MainUIViewComponentSystem
    {
        [EntitySystem]
        private static void YIUIInitialize(this MainUIViewComponent self)
        {
        }

        [EntitySystem]
        private static void Destroy(this MainUIViewComponent self)
        {
        }

        [EntitySystem]
        private static async ETTask<bool> YIUIOpen(this MainUIViewComponent self)
        {
            await ETTask.CompletedTask;
            return true;
        }

        #region YIUIEvent开始
        
        [YIUIInvoke(MainUIViewComponent.OnEventClickInvoke)]
        private static void OnEventClickInvoke(this MainUIViewComponent self)
        {

        }
        #endregion YIUIEvent结束
    }
}
