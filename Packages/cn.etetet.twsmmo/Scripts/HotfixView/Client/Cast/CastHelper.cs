namespace ET.Client
{
    [FriendOfAttribute(typeof(ET.Client.Cast))]
    public static class CastHelper
    {
        public static Cast CreateCast(this Unit_Client caster, int castConfigId)
        {
            CastComponent castComponent = caster.GetComponent<CastComponent>();
            if (castComponent == null)
                return null;
            Cast cast = castComponent.Get(castConfigId);
            if (cast != null)
                return cast;

            cast = castComponent.Create(castConfigId, caster);
            return cast;
        }
    }
}