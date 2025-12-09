using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
	[EntitySystemOf(typeof(UILoginComponent))]
	public static partial class UILoginComponentSystem
	{
		[EntitySystem]
		private static void Awake(this UILoginComponent self)
		{
			ReferenceCollector rc = self.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();
			self.loginBtn = rc.Get<GameObject>("LoginBtn");
			self.Text_LoginMsg = rc.Get<Text>("Text_LoginMsg");
			self.Text_LoginMsg.text = "等待连接";
			
			self.loginBtn.GetComponent<Button>().onClick.AddListener(()=> { self.OnLogin(); });
			self.account = rc.Get<GameObject>("Account");
			self.password = rc.Get<GameObject>("Password");
		}

		
		public static void OnLogin(this UILoginComponent self)
		{
			self.Root().GetComponent<TestContentFTServer>().StartAsync().Coroutine();
			// GlobalComponent globalComponent = self.Root().GetComponent<GlobalComponent>();
			// LoginHelper.Login(
			// 	self.Root(), 
			// 	globalComponent.GlobalConfig.Address,
			// 	self.account.GetComponent<InputField>().text, 
			// 	self.password.GetComponent<InputField>().text).NoContext();
		}

		public static void SetText(this UILoginComponent self, string text)
		{
			self.Text_LoginMsg.text = text;
		}
	}
}
