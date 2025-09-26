using System;

namespace ET.Server
{
    [Invoke(SceneType.Realm)]
    public class NetComponentOnReadInvoker_Realm: AInvokeHandler<NetComponentOnRead>
    {
        public override void Handle(NetComponentOnRead args)
        {
            Session session = args.Session;
            object message = args.Message;
            // 根据消息接口判断是不是Actor消息，不同的接口做不同的处理,比如需要转发给Chat Scene，可以做一个IChatMessage接口
            switch (message)
            {
                case ISessionMessage:
                {
                    MessageSessionDispatcher.Instance.Handle(session, message);
                    break;
                }
                case IActorArchiveMessage actorArchiveMessage:
                {
                    var actorId = StartSceneConfigCategory.Instance.archiveCenterConfig.ActorId;
                    session.Fiber().Root.GetComponent<MessageSender>().Send(actorId, actorArchiveMessage);
                    break;
                }
                case IActorArchiveRequest actorArchiveRequest:
                {
                    CallActorArchiveRequest(session, actorArchiveRequest).NoContext();
                    break;
                }
                default:
                {
                    throw new Exception($"not found handler: {message}");
                }
            }
        }

        public async ETTask CallActorArchiveRequest(Session session,IActorArchiveRequest actorArchiveRequest)
        {
            var actorId = StartSceneConfigCategory.Instance.archiveCenterConfig.ActorId;
            int rpcId = actorArchiveRequest.RpcId; // 这里要保存客户端的rpcId
            long instanceId = session.InstanceId;
            IResponse iResponse = await session.Fiber().Root.GetComponent<MessageSender>().Call(actorId, actorArchiveRequest);
            iResponse.RpcId = rpcId;
            // session可能已经断开了，所以这里需要判断
            if (session.InstanceId == instanceId)
            {
                session.Send(iResponse);
            }
        }
    }
}