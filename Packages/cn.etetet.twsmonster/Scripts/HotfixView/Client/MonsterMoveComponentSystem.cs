using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

namespace ET.Client
{
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
        }

        public static void Init(this ET.Client.MonsterMoveComponent self)
        {
            GameObject gameObject = self.GetParent<Unit_Client>().GetComponent<GameObjectComponent>().GameObject;
            self.spriteRenderer = gameObject.GetComponentInChildren<SpriteRenderer>();
            self.agent = gameObject.GetComponent<PolyNavAgent>();
            self.UnityEventTrigger = gameObject.GetComponentInChildren<UnityEventTrigger>();
            self.SetPos(self.GetCreatePosition(Random.Range(-1, 2), Random.Range(-1, 2)));
            self.InitColliderTrigger();
            self.InitAgent();
        }

        //坐标在屏幕外
        public static Vector3 GetCreatePosition(this MonsterMoveComponent self, int dirX, int dirY)
        {
            var cameraComponent = self.Root().CurrentScene().GetComponent<CameraComponent>();
            var cameraBoundPos = cameraComponent.cameraBoundPos;
            //坐标偏移量
            float offset = 10;
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

        public static void InitColliderTrigger(this MonsterMoveComponent self)
        {
            self.UnityEventTrigger.OnFixedUpdateAction += self.OnFixedUpdateAction;
            self.UnityEventTrigger.BelongToUnitId = self.Id;
            self.UnityEventTrigger.unitType = (int)self.GetParent<Unit_Client>().UnitType;
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
            var unitTr = unit_Player.GetComponent<GameObjectComponent>().GameObject.transform;
            Vector2 pos = unitTr.position;
            self.agent.SetDestination(pos);
        }

        public static void SetPos(this MonsterMoveComponent self, float3 pos)
        {
            Vector2 vector2 = new Vector2(pos.x, pos.y);
            self.agent.position = vector2;
        }
    }
}