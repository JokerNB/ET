using Unity.Mathematics;

namespace ET.Client
{
    [EntitySystemOf(typeof(Cast))]
    public static partial class CastSystem
    {
        [EntitySystem]
        private static void Awake(this ET.Client.Cast self, int args2)
        {
            self.ConfigId = args2;
        }

        [EntitySystem]
        private static void Destroy(this ET.Client.Cast self)
        {
            self.ConfigId = default;
            self.Caster = default;
            self.Target.Clear();
            self.StartTime = default;
        }

        /// <summary>
        /// 释放Cast
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static int Cast(this ET.Client.Cast self)
        {
            int err = self.CastCheck();
            if (err != ErrorCode.ERR_Success)
                return err;

            //选择目标
            self.SelectTarget();

            err = self.CastCheckBeforeBegin();
            if (err != ErrorCode.ERR_Success)
                return err;

            self.CastBeginAsync().NoContext();
            return ErrorCode.ERR_Success;
        }

        /// <summary>
        /// Cast释放的前置判断
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static int CastCheck(this ET.Client.Cast self)
        {
            if (self == null || self.IsDisposed)
                return ErrorCode.ERR_Cast_ArgsError;
            Entity unit = self.Caster;
            if (unit == null || unit.IsDisposed)
                return ErrorCode.ERR_Cast_CasterIsNull;
            return ErrorCode.ERR_Success;
        }

        public static void SelectTarget(this ET.Client.Cast self)
        {
            Entity caster = self.Caster;
            CastConfig config = self.CastConfig;

            int rang = 0;
            switch (config.SelectType)
            {
                case 1:
                    rang = int.Parse(config.SelectParam[0]);
                    break;
            }
        }

        public static int CastCheckBeforeBegin(this ET.Client.Cast self)
        {
            return ErrorCode.ERR_Success;
        }

        public static async ETTask CastBeginAsync(this ET.Client.Cast self)
        {
            await ETTask.CompletedTask;
        }
    }
}