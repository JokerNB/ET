namespace ET.Client
{
    [ComponentOf(typeof(Scene))]
    public class PlayerComponent : Entity, IAwake
    {
        public long PlayerId { get; set; }
        public string Token { get; set; }

        public long Key { get; set; }

        public string Address { get; set; }

        public string Account { get; set; } = "Et123456";
        public string Password { get; set; } = "123";
    }
}