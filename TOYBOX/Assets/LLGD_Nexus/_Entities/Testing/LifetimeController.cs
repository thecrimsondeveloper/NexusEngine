using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace Toolkit.NexusEngine
{
    public class LifetimeController : NexusBlock
    {
        [SerializeField] NexusConditionEvent deathCondition;

        public UnityEvent OnDestroy = new UnityEvent();

        public void DestroyEntity()
        {
            GameObject.Destroy(entity.gameObject);
        }
    }
}
