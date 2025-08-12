using System;
using UnityEngine;
using YIUIFramework;
using System.Collections.Generic;

namespace ET.Client
{
    /// <summary>
    /// Author  TWS
    /// Date    2025.8.11
    /// Desc
    /// </summary>
    [FriendOf(typeof(ETTWSLoadingUIPanelComponent))]
    public static partial class ETTWSLoadingUIPanelComponentSystem
    {
        [EntitySystem]
        private static void YIUIInitialize(this ETTWSLoadingUIPanelComponent self)
        {
        }

        [EntitySystem]
        private static void Destroy(this ETTWSLoadingUIPanelComponent self)
        {
        }

        [EntitySystem]
        private static async ETTask<bool> YIUIOpen(this ETTWSLoadingUIPanelComponent self)
        {
            await self.UIPanel.OpenViewAsync<LoadingUIViewComponent>();
            await ETTask.CompletedTask;
            return true;
        }

        #region YIUIEvent开始
        #endregion YIUIEvent结束
    }
}
