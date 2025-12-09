using System;
using Fantasy;
using Fantasy.Async;
using Fantasy.Network;
using UnityEngine.UI;

namespace ET.Client
{
    [EntitySystemOf(typeof(TestContentFTServer))]
    public static partial class TestContentFTServerSystem
    {
        [EntitySystem]
        private static void Awake(this ET.Client.TestContentFTServer self)
        {
        }

        public static async FTask StartAsync(this ET.Client.TestContentFTServer self)
        {
            await Fantasy.Platform.Unity.Entry.Initialize();
            await self.Root().GetComponent<FTServerModelComponent>().CreateSession();
        }
    }
}