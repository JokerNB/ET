using System;
using UnityEngine;
using YIUIFramework;
using System.Collections.Generic;

namespace ET.Client
{

    /// <summary>
    /// 由YIUI工具自动创建 请勿修改
    /// </summary>
    [YIUI(EUICodeType.View)]
    [ComponentOf(typeof(YIUIChild))]
    public partial class LoginUIViewComponent : Entity, IDestroy, IAwake, IYIUIBind, IYIUIInitialize, IYIUIOpen
    {
        public const string PkgName = "ETTWSLoginUI";
        public const string ResName = "LoginUIView";

        public EntityRef<YIUIChild> u_UIBase;
        public YIUIChild UIBase => u_UIBase;
        public EntityRef<YIUIWindowComponent> u_UIWindow;
        public YIUIWindowComponent UIWindow => u_UIWindow;
        public EntityRef<YIUIViewComponent> u_UIView;
        public YIUIViewComponent UIView => u_UIView;
        public UnityEngine.RectTransform u_ComInput_Account;
        public UnityEngine.RectTransform u_ComInput_Password;
        public UnityEngine.RectTransform u_ComLoginText;
        public YIUIFramework.UIDataValueString u_DataLoginText;
        public UIEventP1<string> u_EventInput_Account;
        public UIEventHandleP1<string> u_EventInput_AccountHandle;
        public const string OnEventInput_AccountInvoke = "LoginUIViewComponent.OnEventInput_AccountInvoke";
        public UIEventP1<string> u_EventInput_Password;
        public UIEventHandleP1<string> u_EventInput_PasswordHandle;
        public const string OnEventInput_PasswordInvoke = "LoginUIViewComponent.OnEventInput_PasswordInvoke";
        public UITaskEventP0 u_EventLogin;
        public UITaskEventHandleP0 u_EventLoginHandle;
        public const string OnEventLoginInvoke = "LoginUIViewComponent.OnEventLoginInvoke";

    }
}