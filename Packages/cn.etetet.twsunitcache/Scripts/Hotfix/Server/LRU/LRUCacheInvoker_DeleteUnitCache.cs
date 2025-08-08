namespace ET.Server
{
    [Invoke(SceneType.UnitCache)]
    public class LRUCacheInvoker_DeleteUnitCache : AInvokeHandler<LRUUnitCacheDelete>
    {
        public override void Handle(LRUUnitCacheDelete args)
        {
            LRUCache lruCache = args.LRUCache;
            lruCache?.GetParent<UnitCacheComponent>().Delete(args.key).NoContext();
        }
    }
}