using UnityEngine;

namespace ET.Client
{
    [NumericHandlerDynamic(SceneType.Current, ENumericType.AbsorptionRange0, 1)]
    public class Player_SkillTickInterval0 : NumericHandlerDynamicSystem<PlayerMoveComponent, Unit_Client, NumericChange>
    {
        protected override async ETTask Run(PlayerMoveComponent self, Unit_Client entity, NumericChange data)
        {
            self?.InitEnergyBlockCollider();
            await ETTask.CompletedTask;
        }
    }

    [EntitySystemOf(typeof(PlayerMoveComponent))]
    public static partial class PlayerMoveComponentSystem
    {
        [EntitySystem]
        private static void Awake(this ET.Client.PlayerMoveComponent self)
        {
            self.GameObject = self.GetParent<Unit_Client>().GetComponent<GameObjectComponent>().GameObject;
            self.SpriteRenderer = self.GetParent<Unit_Client>().GetComponent<GameObjectComponent>().sprite;
            //默认朝右
            self.ChangeSpriteRendererFlip(false);
            self.UnityEventTrigger = self.GetParent<Unit_Client>().GetComponent<GameObjectComponent>().unityEventTrigger;
            self.InitColliderTrigger();

            self.InitEnergyBlockCollider();
        }

        [EntitySystem]
        private static void Update(this ET.Client.PlayerMoveComponent self)
        {
            self.horizontalinput = Input.GetAxis("Horizontal");
            self.Verticalinput = Input.GetAxis("Vertical");
        }

        [EntitySystem]
        private static void Destroy(this ET.Client.PlayerMoveComponent self)
        {
            self.UnityEventTrigger.OnCollisionEnterAction -= self.OnCollisionEnterAction;
            self.UnityEventTrigger.OnCollisionExitAction -= self.OnCollisionExitAction;
            self.UnityEventTrigger.OnFixedUpdateAction -= self.OnFixedUpdate;
            self.SpriteRenderer = null;
            self.UnityEventTrigger = null;
        }

        public static void InitColliderTrigger(this PlayerMoveComponent self)
        {
            self.UnityEventTrigger.BelongToUnitId = self.GetParent<Unit_Client>().Id;
            self.UnityEventTrigger.unitType = (int)self.GetParent<Unit_Client>().UnitType;
            self.UnityEventTrigger.OnCollisionEnterAction += self.OnCollisionEnterAction;
            self.UnityEventTrigger.OnCollisionExitAction += self.OnCollisionExitAction;
            self.UnityEventTrigger.OnFixedUpdateAction += self.OnFixedUpdate;
        }

        public static void InitEnergyBlockCollider(this PlayerMoveComponent self)
        {
            var range = self.GetParent<Unit_Client>().NumericComponent.GetAsFloat(ENumericType.AbsorptionRange0);
            var collider2D = self.GameObject.Get<CircleCollider2D>("EnergyBlockCollider");
            collider2D.radius = range;
        }

        private static void OnCollisionEnterAction(this PlayerMoveComponent self, Collision2D collision2D, long unitId, int unitType)
        {
            //monster cast energyblock
            if (unitType == (int)UnitType.EnergyBlock)
            {
                var unitClient = self.Root().CurrentScene().GetComponent<UnitComponent_Client>().Get(unitId);
                unitClient.GetComponent<EnergyBlockComponent>().Collect();
            }
        }
        
        public static async ETTask DoFlash(this PlayerMoveComponent self)
        {
            if (self == null || self.IsDisposed || self.SpriteRenderer == null)
                return;
            self.SpriteRenderer.color = Color.red;
            await self.Root().GetComponent<TimerComponent>().WaitAsync(100);
            if (self == null || self.IsDisposed || self.SpriteRenderer == null)
                return;
            self.SpriteRenderer.color = Color.white;
        }

        private static void OnCollisionExitAction(this PlayerMoveComponent self, Collision2D collision2D, long unitId, int unitType)
        {
            //monster cast energyblock

        }

        public static void OnFixedUpdate(this PlayerMoveComponent self)
        {
            self.Move();
        }

        public static void Move(this ET.Client.PlayerMoveComponent self)
        {
            if (self.horizontalinput == 0 && self.Verticalinput == 0)
                return;

            if (self.horizontalinput != 0 && self.Verticalinput != 0)
            {
                self.horizontalinput *= 0.6f;
                self.Verticalinput *= 0.6f;
            }

            float speed0 = self.GetParent<Unit_Client>().NumericComponent.GetAsFloat(ENumericType.Speed0);
            var x = Vector3.right * self.horizontalinput * Time.fixedDeltaTime * speed0;
            var y = Vector3.up * self.Verticalinput * Time.fixedDeltaTime * speed0;
            self.Transform.Translate(x);
            self.Transform.Translate(y);
            //计算相机边界，调整地图
            int dirX = 0;
            if (self.horizontalinput > 0)
            {
                dirX = 1;
                if (self.SpriteRenderer.flipX)
                    self.ChangeSpriteRendererFlip(false);
            }
            else if (self.horizontalinput < 0)
            {
                dirX = -1;
                if (!self.SpriteRenderer.flipX)
                    self.ChangeSpriteRendererFlip(true);
            }

            int dirY = 0;
            if (self.Verticalinput > 0)
                dirY = 1;
            else if (self.Verticalinput < 0)
                dirY = -1;

            //须保证顺序，地图计算需要根据相机位置
            self.Root().CurrentScene().GetComponent<CameraComponent>().Translate(x);
            self.Root().CurrentScene().GetComponent<CameraComponent>().Translate(y);
            self.Root().CurrentScene().GetComponent<CameraComponent>().OrthographicCameraEdge();
            self.Root().CurrentScene().GetComponent<MapManagerComponent>().RefreshMap(dirX, dirY);
        }

        public static void ChangeSpriteRendererFlip(this ET.Client.PlayerMoveComponent self, bool flip)
        {
            self.SpriteRenderer.flipX = flip;
        }
    }
}