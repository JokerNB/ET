namespace ET.Client
{
    [MessageSessionHandler(SceneType.All)]
    public class A2C_DisconnectHandler : MessageSessionHandler<A2C_Disconnect>
    {
        protected override async ETTask Run(Session session, A2C_Disconnect message)
        {
            //TODO:二次登录下线通知
            await ETTask.CompletedTask;
        }
    }
}
