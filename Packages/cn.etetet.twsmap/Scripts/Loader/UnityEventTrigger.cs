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
            var unitType_Collider = other.gameObject.GetComponent<UnityEventTrigger>().unitType;
            this.OnCollisionEnterAction?.Invoke(other, unitId, unitType_Collider);
        }
        
        private void OnCollisionExit2D(Collision2D other)
        {
            var unitId = other.gameObject.GetComponent<UnityEventTrigger>().BelongToUnitId;
            var unitType_Collider = other.gameObject.GetComponent<UnityEventTrigger>().unitType;
            this.OnCollisionExitAction?.Invoke(other, unitId, unitType_Collider);
        }
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            var unitId = other.gameObject.GetComponent<UnityEventTrigger>().BelongToUnitId;
            var unitType_Collider = other.gameObject.GetComponent<UnityEventTrigger>().unitType;
            this.OnTriggerEnterAction?.Invoke(other, unitId, unitType_Collider);
        }
        
        private void OnTriggerExit2D(Collider2D other)
        {
            var unitId = other.gameObject.GetComponent<UnityEventTrigger>().BelongToUnitId;
            var unitType_Collider = other.gameObject.GetComponent<UnityEventTrigger>().unitType;
            this.OnTriggerExitAction?.Invoke(other, unitId, unitType_Collider);
        }

        private void FixedUpdate()
        {
            this.OnFixedUpdateAction?.Invoke();
        }
    }
}