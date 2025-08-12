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
    [FriendOf(typeof(LoadingUIViewComponent))]
    public static partial class LoadingUIViewComponentSystem
    {
        [EntitySystem]
        private static void YIUIInitialize(this LoadingUIViewComponent self)
        {
            self.SetSliderValue(0);
            self.u_DataSliderValue.AddValueChangeAction(() =>
            {
                var sliderValue = self.u_DataSliderValue.GetValue();
                if (sliderValue >= 1)
                {
                    self.YIUIMgr().ClosePanel<ETTWSLoadingUIPanelComponent>();
                }
            });
        }

        [EntitySystem]
        private static void Destroy(this LoadingUIViewComponent self)
        {
        }

        [EntitySystem]
        private static async ETTask<bool> YIUIOpen(this LoadingUIViewComponent self)
        {
            self.SetSliderValue(1);
            await ETTask.CompletedTask;
            return true;
        }

        public static void SetSliderValue(this LoadingUIViewComponent self, float value)
        {
            self.u_DataSliderValue.SetValue(value);
        }

        public static void SetSliderValueAdd(this LoadingUIViewComponent self, float AddValue)
        {
            self.u_DataSliderValue.SetValue(self.u_DataSliderValue.GetValue() + AddValue);
        }
    }
}