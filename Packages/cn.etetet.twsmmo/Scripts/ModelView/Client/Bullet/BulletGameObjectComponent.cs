using UnityEngine;

namespace ET.Client
{
    [ComponentOf(typeof(Unit_Client))]
    public class BulletGameObjectComponent : Entity, IAwake, IDestroy
    {
        public Transform Transform { get; set; }
    }
}