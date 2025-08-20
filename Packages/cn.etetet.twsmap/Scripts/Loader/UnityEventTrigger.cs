using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ET
{
    public class UnityEventTrigger : MonoBehaviour
    {
        [ReadOnly]
        public long BelongToUnitId;
        [ReadOnly]
        public int unitType;
        [ReadOnly]
        public bool isInCamera = false;

        public Action<Collision2D, long, int> OnTriggerEnterAction;
        public Action<Collision2D, long, int> OnTriggerExitAction;
        public Action OnFixedUpdateAction;

        private void OnCollisionEnter2D(Collision2D other)
        {
            var unitId = other.gameObject.GetComponent<UnityEventTrigger>().BelongToUnitId;
            this.OnTriggerEnterAction?.Invoke(other, unitId, this.unitType);
        }

        private void OnCollisionExit2D(Collision2D other)
        {
            var unitId = other.gameObject.GetComponent<UnityEventTrigger>().BelongToUnitId;
            this.OnTriggerExitAction?.Invoke(other, unitId, this.unitType);
        }

        private void FixedUpdate()
        {
            this.OnFixedUpdateAction?.Invoke();
        }

        private void OnBecameInvisible()
        {
            this.isInCamera = false;
        }

        private void OnBecameVisible()
        {
            this.isInCamera = true;
        }
    }
}