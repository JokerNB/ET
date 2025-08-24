namespace ET.Client
{
    [Event(SceneType.Current)]
    [FriendOfAttribute(typeof(ET.Client.Buff))]
    public class BuffAdd_EventHandler : AEvent<Scene, Event_BuffAdd>
    {
        protected override async ETTask Run(Scene scene, Event_BuffAdd a)
        {
            Log.Error($"-> 玩家 {a.OwnerId} 添加了 {((Buff)a.buffData).ConfigId.ToString()} BUFF {((Buff)a.buffData).Id.ToString()}");
            //buff添加，状态记录到客户端buffcomponent，显示buff图标、信息、播放buff特效等等
            await ETTask.CompletedTask;
        }
    }
}

