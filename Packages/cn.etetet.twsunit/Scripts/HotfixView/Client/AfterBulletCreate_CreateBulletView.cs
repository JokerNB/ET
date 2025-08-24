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
            a.Unit.AddComponent<BulletGameObjectComponent>().SetPosition(a.pos);
            await ETTask.CompletedTask;
        }
    }
}

