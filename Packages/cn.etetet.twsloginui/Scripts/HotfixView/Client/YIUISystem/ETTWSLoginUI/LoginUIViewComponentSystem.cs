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
    [FriendOf(typeof(LoginUIViewComponent))]
    public static partial class LoginUIViewComponentSystem
    {
        [EntitySystem]
        private static void YIUIInitialize(this LoginUIViewComponent self)
        {
        }

        [EntitySystem]
        private static void Destroy(this LoginUIViewComponent self)
        {
        }

        [EntitySystem]
        private static async ETTask<bool> YIUIOpen(this LoginUIViewComponent self)
        {
            await ETTask.CompletedTask;
            return true;
        }

        #region YIUIEvent开始

        [YIUIInvoke(LoginUIViewComponent.OnEventInput_PasswordInvoke)]
        private static void OnEventInput_PasswordInvoke(this LoginUIViewComponent self, string p1)
        {
            self.Account = p1;
        }

        [YIUIInvoke(LoginUIViewComponent.OnEventInput_AccountInvoke)]
        private static void OnEventInput_AccountInvoke(this LoginUIViewComponent self, string p1)
        {
            self.Password = p1;
        }

        [YIUIInvoke(LoginUIViewComponent.OnEventLoginInvoke)]
        private static async ETTask OnEventLoginInvoke(this LoginUIViewComponent self)
        {
            //TODO:使用默认账号、密码
            self.Account = "Et123456";
            self.Password = "123";
            GlobalComponent globalComponent = self.Root().GetComponent<GlobalComponent>();
            await LoginHelper.Login(self.Root(),
                globalComponent.GlobalConfig.Address,
                self.Account,
                self.Password);
        }

        #endregion YIUIEvent结束
    }
}