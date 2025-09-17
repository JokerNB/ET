using System;
using System.Threading.Tasks;

namespace ET.Server
{
    public static class DisconnectHelper
    {
        public static async ETTask Disconnect(this Session session)
        {
            if (session == null || session.IsDisposed)
                return;
            long instanceId = session.InstanceId;
            TimerComponent timerComponent = session.Root().GetComponent<TimerComponent>();
            await timerComponent.WaitAsync(1000);

            if (session.InstanceId != instanceId)
                return;
            session.Dispose();
        }

        public static async ETTask KickPlayer(Player player)
        {
            if (player == null || player.IsDisposed)
                return;
            long instanceId = player.InstanceId;
            CoroutineLockComponent coroutineLockComponent = player.Root().GetComponent<CoroutineLockComponent>();
            using (await coroutineLockComponent.Wait(CoroutineLockType.LoginGate, player.Account.GetLongHashCode()))
            {
                if (player.IsDisposed || instanceId != player.InstanceId)
                    return;
                await KickPlayerNoLock(player);
            }
        }

        public static async ETTask KickPlayerNoLock(Player player)
        {
            if (player == null || player.IsDisposed)
                return;
            switch (player.playerState)
            {
                case PlayerState.Disconnect:
                    break;
                case PlayerState.Gate:
                    break;
                case PlayerState.Game:
                    //通知游戏逻辑服下线Unit角色逻辑，并将数据存入数据库
                    var m2gRequestExitGame = await player.Root().GetComponent<MessageLocationSenderComponent>().Get(LocationType.Unit)
                            .Call(player.PlayerId, G2M_RequestExitGame.Create());

                    //通知移除账号角色登录信息
                    G2L_RemoveLoginRecord g2LRemoveLoginRecord = G2L_RemoveLoginRecord.Create();
                    g2LRemoveLoginRecord.AccountName = player.Account;
                    g2LRemoveLoginRecord.ServerId = player.Zone();
                    var l2gRemoveLoginRecord = await player.Root().GetComponent<MessageSender>()
                            .Call(StartSceneConfigCategory.Instance.loginCenterConfig.ActorId, g2LRemoveLoginRecord) as L2G_RemoveLoginRecord;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            TimerComponent timerComponent = player.Root().GetComponent<TimerComponent>();
            player.playerState = PlayerState.Disconnect;
            
            await player.GetComponent<PlayerSessionComponent>().RemoveLocation(LocationType.GateSession);
            await player.RemoveLocation(LocationType.Player);
            player.Root().GetComponent<PlayerComponent>().Remove(player);
            player?.Dispose();
            
            await timerComponent.WaitAsync(300);
        }
    }
}