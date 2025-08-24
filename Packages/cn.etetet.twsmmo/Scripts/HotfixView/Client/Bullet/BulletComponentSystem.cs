using Unity.Mathematics;

namespace ET.Client
{
    [Invoke(TimerInvokeType.BulletTick)]
    public class BulletTick_TimerHandler : ATimer<BulletComponent>
    {
        protected override void Run(BulletComponent t)
        {
            t?.Tick();
        }
    }

    [EntitySystemOf(typeof(BulletComponent))]
    public static partial class BulletComponentSystem
    {
        [EntitySystem]
        private static void Awake(this ET.Client.BulletComponent self, int args2)
        {
            self.configId = args2;
            self.AddComponent<ActionsTempComponent>();
        }

        [EntitySystem]
        private static void Destroy(this ET.Client.BulletComponent self)
        {
            self.PreDestroy();
            self.configId = default;
        }

        public static void Start(this ET.Client.BulletComponent self)
        {
            Unit_Client owner = self.GetOwner();
            if (owner == null)
            {
                self.Dispose();
                return;
            }

            Log.Error($"-> 子弹 {self.configId} Tick");

            BulletConfig bulletConfig = self.config;

            if (bulletConfig.AwakeAction.Count != 0)
            {
                foreach (int actionId in bulletConfig.AwakeAction)
                {
                    self.CreateActions(actionId, owner, owner, ActionsRunType.BulletAwake);
                }
            }

            if (bulletConfig.Interval > 0)
            {
                int interval = bulletConfig.Interval;
                if (interval <= 100)
                    interval = 100;
                self.TickTimer = self.Root().GetComponent<TimerComponent>().NewRepeatedTimer(interval, TimerInvokeType.BulletTick, self);
            }
        }

        public static void Tick(this ET.Client.BulletComponent self)
        {
            Unit_Client selfUnit = self.GetParent<Unit_Client>();
            Unit_Client owner = self.GetOwner();
            if (owner == null)
            {
                self.Dispose();
                return;
            }
            
            Log.Error($"-> 子弹 {self.configId} Tick");
            
            BulletConfig config = self.config;
            
            var allUnits = self.Root().CurrentScene().GetComponent<UnitComponent_Client>().GetAllUnits();

            using (ListComponent<Unit_Client> list = ListComponent<Unit_Client>.Create())
            {
                switch (config.Shape)
                {
                    case 1: //选择身边一定范围内的一个人
                        int range = int.Parse(config.ShapeParam[0]);
                        foreach (Unit_Client unit in allUnits)
                        {
                            if(unit == owner)
                                continue;
                            if(math.length(unit.GetUnitPosition() - selfUnit.GetUnitPosition()) < range)
                                list.Add(unit);
                        }
                        break;
                }

                if (list.Count > 0)
                {
                    foreach (var unit in list)
                    {
                        if (config.TickCastId.Count > 0)
                        {
                            foreach (var tickCastId in config.TickCastId)
                            {
                                owner.CreateAndCast(tickCastId);
                            }
                        }

                        if (config.TickAction.Count > 0)
                        {
                            foreach (int tickActionId in config.TickAction)
                            {
                                self.CreateActions(tickActionId, unit, owner, ActionsRunType.BulletTick);
                            }
                        }
                    }
                }
            }
        }

        public static void PreDestroy(this ET.Client.BulletComponent self)
        {
            self.Root().GetComponent<TimerComponent>().Remove(ref self.TickTimer);
            Unit_Client owner = self.GetOwner();
            if (owner == null)
                return;
            BulletConfig bulletConfig = self.config;
            if (bulletConfig.DestroyAction.Count == 0)
                return;
            foreach (int actionsId in bulletConfig.DestroyAction)
            {
                self.CreateActions(actionsId, owner, owner, ActionsRunType.BulletDestroy);
            }
        }

        public static Unit_Client GetOwner(this ET.Client.BulletComponent self)
        {
            return self.Root().CurrentScene().GetComponent<UnitComponent_Client>().Get(self.ownerId);
        }
    }
}