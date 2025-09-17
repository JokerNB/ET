namespace ET.Server
{
    public enum PlayerState
    {
        Disconnect,
        Gate,
        Game
    }
    [ChildOf(typeof(PlayerComponent))]
    public sealed class Player : Entity, IAwake<string>
    {
        public string Account { get; set; }
        public PlayerState playerState { get; set; }
        
        public long PlayerId { get; set; }
    }
}