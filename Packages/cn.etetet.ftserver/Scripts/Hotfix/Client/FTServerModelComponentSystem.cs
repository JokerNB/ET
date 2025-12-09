using Fantasy;
using Fantasy.Async;
using Fantasy.Network;

namespace ET.Client
{
    [EntitySystemOf(typeof(FTServerModelComponent))]
    public static partial class FTServerModelComponentSystem
    {
        [EntitySystem]
        private static void Awake(this ET.Client.FTServerModelComponent self)
        {

        }

        public static async FTask CreateSession(this ET.Client.FTServerModelComponent self)
        {
            var scene = await Fantasy.Scene.Create(SceneRuntimeMode.MainThread);
            var session = scene.Connect("114.55.29.103:20001",
                NetworkProtocolType.KCP,
                self.OnConnectComplete, self.OnConnectFail, self.OnConnectDisconnect,
                false, 5000);
        }
        
        
        public static void OnConnectComplete(this ET.Client.FTServerModelComponent self)
        {
            Log.Error("OnConnectComplete");
            EventSystem.Instance.Publish(self.Root(),new ConnectState
            {
                state = "OnConnectComplete"
            });

        }

        public static void OnConnectFail(this ET.Client.FTServerModelComponent self)
        {
            Log.Error("OnConnectFail");
            EventSystem.Instance.Publish(self.Root(),new ConnectState
            {
                state = "OnConnectFail"
            });
        }

        public static void OnConnectDisconnect(this ET.Client.FTServerModelComponent self)
        {
            Log.Error("OnConnectDisconnect");
            EventSystem.Instance.Publish(self.Root(),new ConnectState
            {
                state = "OnConnectDisconnect"
            });
        }
    }
}
