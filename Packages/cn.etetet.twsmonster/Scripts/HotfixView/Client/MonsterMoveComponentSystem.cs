using UnityEngine;
using Random = UnityEngine.Random;

namespace ET.Client
{
    [Invoke(TimerInvokeType.MonsterCheckTimer)]
    public class MonsterCheckTimer_Handler : ATimer<MonsterMoveComponent>
    {
        protected override void Run(MonsterMoveComponent t)
        {
            t?.CheckIsInCamera();
        }
    }

    [Invoke(TimerInvokeType.MonsterAttackRepeatedTimer)]
    public class MonsterAttackRepeatedTimer_Handler : ATimer<MonsterMoveComponent>
    {
        protected override void Run(MonsterMoveComponent t)
        {
            t?.AttackPlayer();
        }
    }

    [EntitySystemOf(typeof(MonsterMoveComponent))]
    [FriendOfAttribute(typeof(ET.Client.CameraComponent))]
    public static partial class MonsterMoveComponentSystem
    {
        [EntitySystem]
        private static void Awake(this ET.Client.MonsterMoveComponent self)
        {
            self.Init();
        }

        [EntitySystem]
        private static void Destroy(this ET.Client.MonsterMoveComponent self)
        {
            self.UnityEventTrigger.OnFixedUpdateAction -= self.OnFixedUpdateAction;
            self.UnityEventTrigger.OnCollisionEnterAction -= self.OnCollisionEnterAction;
            self.UnityEventTrigger.OnCollisionExitAction -= self.OnCollisionExitAction;
            self.Root().GetComponent<TimerComponent>().Remove(ref self.InScreenTimer);
            self.Root().GetComponent<TimerComponent>().Remove(ref self.AttackTimer);
        }

        public static void Init(this ET.Client.MonsterMoveComponent self)
        {
            var gameObjectComponent = self.GetParent<Unit_Client>().GetComponent<GameObjectComponent>();
            self.GameObject = gameObjectComponent.GameObject;
            self.spriteRenderer = gameObjectComponent.sprite;
            self.agent = self.GameObject.Get<PolyNavAgent>("agent");
            self.UnityEventTrigger = gameObjectComponent.unityEventTrigger;
            self.SetPos(self.GetCreatePosition(Random.Range(-1, 2), Random.Range(-1, 2)));
            self.InitColliderTrigger();
            self.InitAgent();
            self.InitTimer();
        }

        //坐标在屏幕外
        public static Vector3 GetCreatePosition(this MonsterMoveComponent self, int dirX, int dirY)
        {
            var cameraComponent = self.Root().CurrentScene().GetComponent<CameraComponent>();
            var cameraBoundPos = cameraComponent.cameraBoundPos;
            //坐标偏移量
            float offset = 1;
            float posX_Min = dirX <= 0 ? cameraBoundPos.x - offset : cameraBoundPos.z;
            float posX_Max = dirX <= 0 ? cameraBoundPos.x : cameraBoundPos.z + offset;
            float posY_Min = dirY <= 0 ? cameraBoundPos.y - offset : cameraBoundPos.w;
            float posY_Max = dirY <= 0 ? cameraBoundPos.y : cameraBoundPos.w + offset;

            return new Vector3(Random.Range(posX_Min, posX_Max), Random.Range(posY_Min, posY_Max));
        }

        public static void InitAgent(this MonsterMoveComponent self)
        {
            var numericDataComponent = self.GetParent<Unit_Client>().NumericComponent;
            float speed = numericDataComponent.GetAsFloat(ENumericType.Speed0);
            self.agent.maxSpeed = speed;
        }

        public static void InitTimer(this MonsterMoveComponent self)
        {
            self.InScreenTimer = self.Root().GetComponent<TimerComponent>().NewRepeatedTimer(2000, TimerInvokeType.MonsterCheckTimer, self);
        }

        public static void CheckIsInCamera(this MonsterMoveComponent self)
        {
            //每2s检测一次，如果不在屏幕里则移除
            bool isVisible = self.spriteRenderer.isVisible;

            if (!isVisible)
            {
                self.Root().CurrentScene().GetComponent<UnitComponent_Client>().Remove(self.GetParent<Unit_Client>().Id);
            }
        }

        public static void InitColliderTrigger(this MonsterMoveComponent self)
        {
            self.UnityEventTrigger.BelongToUnitId = self.Id;
            self.UnityEventTrigger.unitType = (int)self.GetParent<Unit_Client>().UnitType;
            self.UnityEventTrigger.OnFixedUpdateAction += self.OnFixedUpdateAction;
            self.UnityEventTrigger.OnCollisionEnterAction += self.OnCollisionEnterAction;
            self.UnityEventTrigger.OnCollisionExitAction += self.OnCollisionExitAction;
        }

        public static void OnCollisionEnterAction(this MonsterMoveComponent self, Collision2D collision, long unitId, int unitType)
        {
            Log.Error($"OnCollisionEnterAction : {(UnitType)unitType}");
            if (unitType == (int)UnitType.Player)
            {
                //普通攻击直接走伤害流程，不走技能系统
                self.AttackTimer = self.Root().GetComponent<TimerComponent>().NewRepeatedTimer(200, TimerInvokeType.MonsterAttackRepeatedTimer, self);
            }
        }

        public static void OnCollisionExitAction(this MonsterMoveComponent self, Collision2D collision, long unitId, int unitType)
        {
            Log.Error($"Exit : {(UnitType)unitType}");
            if (unitType == (int)UnitType.Player)
            {
                self.Root().GetComponent<TimerComponent>().Remove(ref self.AttackTimer);
            }
        }

        public static void AttackPlayer(this MonsterMoveComponent self)
        {
            Unit_Client unitClient = self.Root().CurrentScene().GetComponent<UnitComponent_Client>().Unit_Player;
            var monsterUnit = self.GetParent<Unit_Client>();
            int damage = monsterUnit.NumericComponent.GetAsInt(ENumericType.NormalDamage0);
            var oldHp = unitClient.NumericComponent.GetAsInt(ENumericType.Hp0);
            unitClient.NumericComponent.Change(ENumericType.Hp0, -damage);
            var newHp = unitClient.NumericComponent.GetAsInt(ENumericType.Hp0);
            unitClient.GetComponent<PlayerMoveComponent>().DoFlash().NoContext();
            Log.Error($"oldHp : {oldHp} , newHp : {newHp}");
            if (oldHp >= 0 && newHp <= 0)
            {
                //死亡
                self.Root().CurrentScene().GetComponent<UnitComponent_Client>().Remove(unitClient.Id);
            }
        }

        public static async ETTask DOFlash(this MonsterMoveComponent self)
        {
            if (self.spriteRenderer == null)
                return;
            self.spriteRenderer.color = Color.red;
            await self.Root().GetComponent<TimerComponent>().WaitAsync(100);
            self.spriteRenderer.color = Color.white;
        }

        public static void OnFixedUpdateAction(this MonsterMoveComponent self)
        {
            self.Move();
        }

        public static void TestEnterChangeColor(this MonsterMoveComponent self)
        {
            if (self.spriteRenderer == null)
                return;
            self.spriteRenderer.color = Color.red;
        }

        public static void TestExitChangeColor(this MonsterMoveComponent self)
        {
            if (self.spriteRenderer == null)
                return;
            self.spriteRenderer.color = Color.white;
        }

        public static void Move(this MonsterMoveComponent self)
        {
            if (self == null)
                return;
            Unit_Client unit_Player = self.Root().CurrentScene().GetComponent<UnitComponent_Client>().Unit_Player;
            if (unit_Player == null)
                return;
            var unitTr = unit_Player.GetComponent<GameObjectComponent>().Transform;
            Vector2 pos = unitTr.position;
            self.agent.SetDestination(pos);
            self.spriteRenderer.flipX = pos.x > self.Transform.position.x;
        }

        public static void SetPos(this MonsterMoveComponent self, Vector2 pos)
        {
            self.agent.position = pos;
            self.spriteRenderer.flipX = pos.x > self.Transform.position.x;
        }
    }
}