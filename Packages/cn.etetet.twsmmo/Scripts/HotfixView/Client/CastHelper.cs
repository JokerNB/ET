namespace ET.Client
{
    [FriendOfAttribute(typeof(ET.Client.Cast))]
    public static class CastHelper
    {
        /// <summary>
        /// 创建一个Cast
        /// </summary>
        /// <param name="caster"></param>
        /// <param name="castConfigId"></param>
        /// <returns></returns>
        public static Cast Create(this Unit caster, int castConfigId)
        {
            CastComponent castComponent = caster.GetComponent<CastComponent>();
            if (castComponent == null)
                return null;

            Cast cast = castComponent.Create(castConfigId);
            cast.Caster = caster;
            return cast;
        }

        /// <summary>
        /// 创建并释放一个Cast
        /// </summary>
        /// <param name="caster"></param>
        /// <param name="castConfigId"></param>
        /// <returns></returns>
        public static int CreateAndCast(this Unit caster, int castConfigId)
        {
            return Create(caster, castConfigId).Cast();
        }
    }
}
