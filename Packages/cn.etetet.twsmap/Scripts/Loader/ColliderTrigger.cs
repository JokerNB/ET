using System;
using UnityEngine;

namespace ET
{
    public class ColliderTrigger : MonoBehaviour
    {
        public long BelongToUnitId;
        public int unitType;

        public Action<Collision2D, long, int> OnTriggerEnterAction;
        public Action<Collision2D, long, int> OnTriggerExitAction;

        private void OnCollisionEnter2D(Collision2D other)
        {
            var unitId = other.gameObject.GetComponent<ColliderTrigger>().BelongToUnitId;
            this.OnTriggerEnterAction?.Invoke(other, unitId, this.unitType);
        }

        private void OnCollisionExit2D(Collision2D other)
        {
            var unitId = other.gameObject.GetComponent<ColliderTrigger>().BelongToUnitId;
            this.OnTriggerExitAction?.Invoke(other, unitId, this.unitType);
        }
    }
}