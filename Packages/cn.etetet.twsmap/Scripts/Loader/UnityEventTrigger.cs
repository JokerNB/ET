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

        public Action<Collision2D, long, int> OnCollisionEnterAction;
        public Action<Collision2D, long, int> OnCollisionExitAction;
        public Action<Collider2D, long, int> OnTriggerEnterAction;
        public Action<Collider2D, long, int> OnTriggerExitAction;
        public Action OnFixedUpdateAction;

        private void OnCollisionEnter2D(Collision2D other)
        {
            var unitId = other.gameObject.GetComponent<UnityEventTrigger>().BelongToUnitId;
            this.OnCollisionEnterAction?.Invoke(other, unitId, this.unitType);
        }

        private void OnCollisionExit2D(Collision2D other)
        {
            var unitId = other.gameObject.GetComponent<UnityEventTrigger>().BelongToUnitId;
            this.OnCollisionExitAction?.Invoke(other, unitId, this.unitType);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            var unitId = other.gameObject.GetComponent<UnityEventTrigger>().BelongToUnitId;
            this.OnTriggerEnterAction?.Invoke(other, unitId, this.unitType);
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            var unitId = other.gameObject.GetComponent<UnityEventTrigger>().BelongToUnitId;
            this.OnTriggerExitAction?.Invoke(other, unitId, this.unitType);
        }

        private void FixedUpdate()
        {
            this.OnFixedUpdateAction?.Invoke();
        }
    }
}