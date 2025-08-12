using System;
using UnityEngine;
using YIUIFramework;
using System.Collections.Generic;

namespace ET.Client
{
    /// <summary>
    /// 由YIUI工具自动创建 请勿修改
    /// </summary>
    [FriendOf(typeof(YIUIChild))]
    [FriendOf(typeof(YIUIWindowComponent))]
    [FriendOf(typeof(YIUIViewComponent))]
    [EntitySystemOf(typeof(LoadingUIViewComponent))]
    public static partial class LoadingUIViewComponentSystem
    {
        [EntitySystem]
        private static void Awake(this LoadingUIViewComponent self)
        {
        }

        [EntitySystem]
        private static void YIUIBind(this LoadingUIViewComponent self)
        {
            self.UIBind();
        }

        private static void UIBind(this LoadingUIViewComponent self)
        {
            self.u_UIBase = self.GetParent<YIUIChild>();
            self.u_UIWindow = self.UIBase.GetComponent<YIUIWindowComponent>();
            self.u_UIView = self.UIBase.GetComponent<YIUIViewComponent>();
            self.UIWindow.WindowOption = EWindowOption.BanOpenTween|EWindowOption.BanCloseTween;
            self.UIView.ViewWindowType = EViewWindowType.View;
            self.UIView.StackOption = EViewStackOption.VisibleTween;

            self.u_ComLoadingProgress = self.UIBase.ComponentTable.FindComponent<UnityEngine.RectTransform>("u_ComLoadingProgress");
            self.u_DataSliderValue = self.UIBase.DataTable.FindDataValue<YIUIFramework.UIDataValueFloat>("u_DataSliderValue");

        }
    }
}
