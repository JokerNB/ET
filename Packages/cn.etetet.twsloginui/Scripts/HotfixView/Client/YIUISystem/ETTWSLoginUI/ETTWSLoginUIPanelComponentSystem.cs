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
    [FriendOf(typeof(ETTWSLoginUIPanelComponent))]
    public static partial class ETTWSLoginUIPanelComponentSystem
    {
        [EntitySystem]
        private static void YIUIInitialize(this ETTWSLoginUIPanelComponent self)
        {
        }

        [EntitySystem]
        private static void Destroy(this ETTWSLoginUIPanelComponent self)
        {
        }

        [EntitySystem]
        private static async ETTask<bool> YIUIOpen(this ETTWSLoginUIPanelComponent self)
        {
            // await self.YIUIRoot().OpenPanelAsync<LoginUIViewComponent>();
            await self.UIPanel.OpenViewAsync<LoginUIViewComponent>();
            // self.UIPanel.Open().ActiveSelfView<ETTWSLoginUIPanelComponent, LoginUIViewComponent>();
            await ETTask.CompletedTask;
            return true;
        }

        #region YIUIEvent开始
        #endregion YIUIEvent结束
    }
}
