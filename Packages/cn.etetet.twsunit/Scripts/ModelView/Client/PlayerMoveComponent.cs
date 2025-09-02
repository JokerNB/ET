using System.Collections.Generic;
using UnityEngine;

namespace ET.Client
{
    [ComponentOf(typeof(Unit_Client))]
    public class PlayerMoveComponent : Entity, IAwake, IUpdate, IDestroy, INumericHandlerDynamic<Unit_Client, NumericChange>
    {
        private GameObject gameObject;

        public GameObject GameObject
        {
            get
            {
                return this.gameObject;
            }
            set
            {
                this.gameObject = value;
                this.Transform = value.transform;
            }
        }

        public Transform Transform { get; private set; }

        public SpriteRenderer SpriteRenderer;
        public UnityEventTrigger UnityEventTrigger;

        public float horizontalinput;
        public float Verticalinput;
    }
}