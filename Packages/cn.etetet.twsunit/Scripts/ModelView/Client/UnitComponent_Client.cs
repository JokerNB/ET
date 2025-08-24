using System.Collections.Generic;

namespace ET.Client
{
    [ComponentOf(typeof(Scene))]
    public class UnitComponent_Client : Entity, IAwake, IDestroy
    {
        public List<EntityRef<Unit_Client>> monsters = new List<EntityRef<Unit_Client>>();
        public List<EntityRef<Unit_Client>> units = new List<EntityRef<Unit_Client>>();
        public EntityRef<Unit_Client> Unit_Player { get; set; }
    }
}