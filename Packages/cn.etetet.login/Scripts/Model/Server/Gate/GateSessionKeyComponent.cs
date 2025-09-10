using System.Collections.Generic;

namespace ET.Server
{
    [ComponentOf(typeof(Scene))]
    public class GateSessionKeyComponent : Entity, IAwake
    {
        public readonly Dictionary<long, string> sessionKey = new();
        public readonly Dictionary<string, long> sessionKey_AccountName = new();
    }
}