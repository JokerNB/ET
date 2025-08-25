using UnityEngine;

namespace ET.Client
{
    [Event(SceneType.Current)]
    [FriendOfAttribute(typeof(ET.Client.BulletComponent))]
    public class AfterBulletCreate_CreateBulletView : AEvent<Scene, AfterBulletCreate>
    {
        protected override async ETTask Run(Scene scene, AfterBulletCreate a)
        {
            BulletComponent bulletComponent = a.Unit.AddComponent<BulletComponent, int>(a.bulledId);
            bulletComponent.ownerId = a.ownerId;
            //TODO:创建子弹实体
            // a.Unit.AddComponent<GameObjectComponent,GameObject>().SetPosition(a.pos);
            await ETTask.CompletedTask;
        }
    }
}

