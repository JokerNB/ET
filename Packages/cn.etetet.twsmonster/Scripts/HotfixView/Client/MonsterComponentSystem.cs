using UnityEngine;

namespace ET.Client
{
    [EntitySystemOf(typeof(MonsterComponent))]
    [FriendOfAttribute(typeof(ET.Client.CameraComponent))]
    public static partial class MonsterComponentSystem
    {
        [EntitySystem]
        private static void Awake(this ET.Client.MonsterComponent self, int configId, int dirX, int dirY)
        {
            self.ConfigId = configId;
            self.dirX = dirX;
            self.dirY = dirY;

            self.CreateMonster(configId).NoContext();
        }
        
        [EntitySystem]
        private static void Destroy(this ET.Client.MonsterComponent self)
        {
            self.UnityEventTrigger.OnTriggerEnterAction -= self.OnTriggerEnterAction;
            self.UnityEventTrigger.OnTriggerExitAction -= self.OnTriggerExitAction;
            self.UnityEventTrigger.OnFixedUpdateAction -= self.OnFixedUpdate;

            YIUIGameObjectPool.Inst.Put(self.go);
        }

        public static async ETTask CreateMonster(this MonsterComponent self, int monsterConfigId)
        {
            string resName = "Monster";
            self.go = await YIUIGameObjectPool.Inst.Get(resName);
            var referenceCollector = self.go.GetComponent<ReferenceCollector>();
            var spriteRenderer = referenceCollector.Get<SpriteRenderer>("Sprite");
            MonsterConfig monsterConfig = MonsterConfigCategory.Instance.Get(monsterConfigId);
            Sprite sprite = await self.Root().CurrentScene().GetComponent<ResourcesLoaderComponent>()
                    .LoadAssetAsync<Sprite>($"Packages/cn.etetet.twsmonster/Assets/GameRes/Atlas/{monsterConfig.ResName}");
            spriteRenderer.sprite = sprite;
            self.go.transform.position = self.GetCreatePosition(self.dirX, self.dirY);
            self.go.transform.name = $"{self.Id}";
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
        public static Vector3 GetCreatePosition(this MonsterComponent self, int dirX, int dirY)
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

        public static void InitColliderTrigger(this MonsterComponent self)
        {
            self.UnityEventTrigger = self.go.GetComponentInChildren<UnityEventTrigger>();
            self.UnityEventTrigger.OnTriggerEnterAction += self.OnTriggerEnterAction;
            self.UnityEventTrigger.OnTriggerExitAction += self.OnTriggerExitAction;
            self.UnityEventTrigger.BelongToUnitId = self.Id;
            self.UnityEventTrigger.unitType = (int)self.UnitType;

            self.UnityEventTrigger.OnFixedUpdateAction += self.OnFixedUpdate;
        }

        public static void OnTriggerEnterAction(this MonsterComponent self, Collision2D collision2D, long unitId, int unitType)
        {
            if (unitType != (int)UnitType.Player)
                return;
            Log.Error($"Monster_EnterAction {collision2D.gameObject.name} , unitId {unitId}");
        }

        public static void OnTriggerExitAction(this MonsterComponent self, Collision2D collision2D, long unitId, int unitType)
        {
            if (unitType != (int)UnitType.Player)
                return;
            Log.Error($"Monster_ExitAction {collision2D.gameObject.name} , unitId {unitId}");
        }

        public static void OnFixedUpdate(this MonsterComponent self)
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

        public static void TestEnterChangeColor(this MonsterComponent self)
        {
            if (self.spriteRenderer == null)
                return;
            self.spriteRenderer.color = Color.red;
        }

        public static void TestExitChangeColor(this MonsterComponent self)
        {
            if (self.spriteRenderer == null)
                return;
            self.spriteRenderer.color = Color.white;
        }
    }
}