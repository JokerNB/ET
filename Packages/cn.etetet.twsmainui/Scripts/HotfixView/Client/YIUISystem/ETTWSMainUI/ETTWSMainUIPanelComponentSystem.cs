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
    [FriendOf(typeof(ETTWSMainUIPanelComponent))]
    public static partial class ETTWSMainUIPanelComponentSystem
    {
        [EntitySystem]
        private static void YIUIInitialize(this ETTWSMainUIPanelComponent self)
        {
        }

        [EntitySystem]
        private static void Destroy(this ETTWSMainUIPanelComponent self)
        {
        }

        [EntitySystem]
        private static async ETTask<bool> YIUIOpen(this ETTWSMainUIPanelComponent self)
        {
            await self.UIPanel.OpenViewAsync<MainUIViewComponent>();
            await ETTask.CompletedTask;
            return true;
        }

        #region YIUIEvent开始
        #endregion YIUIEvent结束
    }
}
