using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

namespace ET.Client
{
    [EntitySystemOf(typeof(MonsterGameObjectComponent))]
    [FriendOfAttribute(typeof(ET.Client.CameraComponent))]
    [FriendOfAttribute(typeof(ET.Client.MonsterManagerComponent))]
    public static partial class MonsterGameObjectComponentSystem
    {
        [EntitySystem]
        private static void Awake(this ET.Client.MonsterGameObjectComponent self)
        {
            self.monsterRoot = self.Root().CurrentScene().GetComponent<MonsterManagerComponent>().MonsterRoot;
            self.CreateMonster().NoContext();
        }

        [EntitySystem]
        private static void Destroy(this ET.Client.MonsterGameObjectComponent self)
        {
            YIUIGameObjectPool.Inst.Put(self.go);
        }

        public static async ETTask CreateMonster(this MonsterGameObjectComponent self)
        {
            string resName = "Monster";
            self.go = await YIUIGameObjectPool.Inst.Get(resName, self.monsterRoot);
            var referenceCollector = self.go.GetComponent<ReferenceCollector>();
            var spriteRenderer = referenceCollector.Get<SpriteRenderer>("Sprite");
            Sprite sprite = await self.Root().CurrentScene().GetComponent<ResourcesLoaderComponent>()
                    .LoadAssetAsync<Sprite>($"Packages/cn.etetet.twsmonster/Assets/GameRes/Atlas/{self.GetParent<Unit_Client>().Config().ResName}");
            spriteRenderer.sprite = sprite;
            self.goTr = self.go.transform;
            self.goTr.name = $"{self.Id}";
            self.spriteRenderer = spriteRenderer;
            self.agent = self.go.GetComponent<PolyNavAgent>();
            Vector2 pos = self.GetCreatePosition(Random.Range(-1, 2), Random.Range(-1, 2));
            self.agent.position = pos;
            
            self.InitColliderTrigger();
            self.InitAgent();
        }

        //坐标在屏幕外
        public static Vector3 GetCreatePosition(this MonsterGameObjectComponent self, int dirX, int dirY)
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

        public static void InitAgent(this MonsterGameObjectComponent self)
        {
            var numericDataComponent = self.GetParent<Unit_Client>().NumericComponent;
            float speed = numericDataComponent.GetAsFloat(ENumericType.Speed0);
            self.agent.maxSpeed = speed;
        }

        public static void InitColliderTrigger(this MonsterGameObjectComponent self)
        {
            self.UnityEventTrigger = self.go.GetComponentInChildren<UnityEventTrigger>();
            self.UnityEventTrigger.OnFixedUpdateAction += self.OnFixedUpdateAction;
            self.UnityEventTrigger.BelongToUnitId = self.Id;
            self.UnityEventTrigger.unitType = (int)self.GetParent<Unit_Client>().UnitType;
        }

        public static void OnFixedUpdateAction(this MonsterGameObjectComponent self)
        {
            self.Move();
        }

        public static void TestEnterChangeColor(this MonsterGameObjectComponent self)
        {
            if (self.spriteRenderer == null)
                return;
            self.spriteRenderer.color = Color.red;
        }

        public static void TestExitChangeColor(this MonsterGameObjectComponent self)
        {
            if (self.spriteRenderer == null)
                return;
            self.spriteRenderer.color = Color.white;
        }

        public static void Move(this MonsterGameObjectComponent self)
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

        public static void SetPos(this MonsterGameObjectComponent self, float3 pos)
        {
            Vector2 vector2 = new Vector2(pos.x, pos.y);
            self.agent.position = vector2;
        }
    }
}