namespace ET.Client
{
    [ChildOf(typeof(BuffComponent))]
    public class Buff : Entity, IAwake<int>, IDestroy
    {
        public int ConfigId;
        public BuffConfig Config => BuffConfigCategory.Instance.Get(this.ConfigId);
        public EntityRef<Unit_Client> Owner;

        /// <summary>
        /// 创建时间
        /// </summary>
        public long CreateTime;

        /// <summary>
        /// 迭代间隔时间
        /// </summary>
        public int TickTime;

        /// <summary>
        /// 迭代开始时间
        /// </summary>
        public long TickBeginTime;

        /// <summary>
        /// 迭代定时器任务Timer
        /// </summary>
        public long TickTimer;

        /// <summary>
        /// 等待迭代定时器任务Timer
        /// </summary>
        public long WaitTickTimer;

        /// <summary>
        /// 过期时间
        /// </summary>
        public long ExpireTime;

        /// <summary>
        /// 过期时间定时器任务Timer
        /// </summary>
        public long ExpireTimer;
    }
}