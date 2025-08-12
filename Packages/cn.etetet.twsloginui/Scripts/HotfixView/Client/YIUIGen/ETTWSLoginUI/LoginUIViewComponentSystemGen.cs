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
    [EntitySystemOf(typeof(LoginUIViewComponent))]
    public static partial class LoginUIViewComponentSystem
    {
        [EntitySystem]
        private static void Awake(this LoginUIViewComponent self)
        {
        }

        [EntitySystem]
        private static void YIUIBind(this LoginUIViewComponent self)
        {
            self.UIBind();
        }

        private static void UIBind(this LoginUIViewComponent self)
        {
            self.u_UIBase = self.GetParent<YIUIChild>();
            self.u_UIWindow = self.UIBase.GetComponent<YIUIWindowComponent>();
            self.u_UIView = self.UIBase.GetComponent<YIUIViewComponent>();
            self.UIWindow.WindowOption = EWindowOption.None;
            self.UIView.ViewWindowType = EViewWindowType.View;
            self.UIView.StackOption = EViewStackOption.VisibleTween;

            self.u_ComInput_Account = self.UIBase.ComponentTable.FindComponent<UnityEngine.RectTransform>("u_ComInput_Account");
            self.u_ComInput_Password = self.UIBase.ComponentTable.FindComponent<UnityEngine.RectTransform>("u_ComInput_Password");
            self.u_ComLoginText = self.UIBase.ComponentTable.FindComponent<UnityEngine.RectTransform>("u_ComLoginText");
            self.u_DataLoginText = self.UIBase.DataTable.FindDataValue<YIUIFramework.UIDataValueString>("u_DataLoginText");
            self.u_EventInput_Account = self.UIBase.EventTable.FindEvent<UIEventP1<string>>("u_EventInput_Account");
            self.u_EventInput_AccountHandle = self.u_EventInput_Account.Add(self,LoginUIViewComponent.OnEventInput_AccountInvoke);
            self.u_EventInput_Password = self.UIBase.EventTable.FindEvent<UIEventP1<string>>("u_EventInput_Password");
            self.u_EventInput_PasswordHandle = self.u_EventInput_Password.Add(self,LoginUIViewComponent.OnEventInput_PasswordInvoke);
            self.u_EventLogin = self.UIBase.EventTable.FindEvent<UITaskEventP0>("u_EventLogin");
            self.u_EventLoginHandle = self.u_EventLogin.Add(self,LoginUIViewComponent.OnEventLoginInvoke);

        }
    }
}
