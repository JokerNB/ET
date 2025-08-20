using UnityEngine;
using UnityEngine.AI;

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
            self.UnityEventTrigger.OnTriggerEnterAction -= self.OnTriggerEnterAction;
            self.UnityEventTrigger.OnTriggerExitAction -= self.OnTriggerExitAction;
            self.UnityEventTrigger.OnFixedUpdateAction -= self.OnFixedUpdate;

            YIUIGameObjectPool.Inst.Put(self.go);
        }

        public static async ETTask CreateMonster(this MonsterGameObjectComponent self)
        {
            string resName = "Monster";
            self.go = await YIUIGameObjectPool.Inst.Get(resName, self.monsterRoot);
            var referenceCollector = self.go.GetComponent<ReferenceCollector>();
            var spriteRenderer = referenceCollector.Get<SpriteRenderer>("Sprite");
            Sprite sprite = await self.Root().CurrentScene().GetComponent<ResourcesLoaderComponent>()
                    .LoadAssetAsync<Sprite>($"Packages/cn.etetet.twsmonster/Assets/GameRes/Atlas/{self.GetParent<Unit>().Config().ResName}");
            spriteRenderer.sprite = sprite;
            self.goTr = self.go.transform;
            self.goTr.position = self.GetCreatePosition(Random.Range(-1,2),Random.Range(-1,2));
            self.goTr.name = $"{self.Id}";
            self.spriteRenderer = spriteRenderer;

            //初始化碰撞体范围
            BoxCollider2D boxCollider2D = referenceCollector.Get<BoxCollider2D>("BoxCollider");
            boxCollider2D.size = sprite.bounds.size;
            boxCollider2D.offset = sprite.bounds.center;

            self.rigidbody2D = self.go.GetComponentInChildren<Rigidbody2D>();
            self.rigidbody2D.simulated = true;

            self.InitColliderTrigger();
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

        public static void InitColliderTrigger(this MonsterGameObjectComponent self)
        {
            self.UnityEventTrigger = self.go.GetComponentInChildren<UnityEventTrigger>();
            self.UnityEventTrigger.OnTriggerEnterAction += self.OnTriggerEnterAction;
            self.UnityEventTrigger.OnTriggerExitAction += self.OnTriggerExitAction;
            self.UnityEventTrigger.OnFixedUpdateAction += self.OnFixedUpdate;
            self.UnityEventTrigger.BelongToUnitId = self.Id;
            self.UnityEventTrigger.unitType = (int)self.GetParent<Unit>().UnitType;

        }

        public static void OnTriggerEnterAction(this MonsterGameObjectComponent self, Collision2D collision2D, long unitId, int unitType)
        {
            if (unitType != (int)UnitType.Player)
                return;
            Log.Error($"Monster_EnterAction {collision2D.gameObject.name} , unitId {unitId}");
        }

        public static void OnTriggerExitAction(this MonsterGameObjectComponent self, Collision2D collision2D, long unitId, int unitType)
        {
            if (unitType != (int)UnitType.Player)
                return;
            Log.Error($"Monster_ExitAction {collision2D.gameObject.name} , unitId {unitId}");
        }

        public static void OnFixedUpdate(this MonsterGameObjectComponent self)
        {
            var unit = UnitHelper.GetMyUnitFromCurrentScene(self.Root().CurrentScene());
            if (unit == null)
                return;
            var unitTr = unit.GetComponent<GameObjectComponent>().GameObject.transform;
            if (self.rigidbody2D == null)
                return;
            Vector2 pos = new Vector2(unitTr.position.x, unitTr.position.y);
            self.rigidbody2D.MovePosition(self.rigidbody2D.position + (pos - self.rigidbody2D.position) * Time.fixedDeltaTime * 1);
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

        public static bool isInScreen(this MonsterGameObjectComponent self)
        {
            return self.UnityEventTrigger.isInCamera;
        }
    }
}