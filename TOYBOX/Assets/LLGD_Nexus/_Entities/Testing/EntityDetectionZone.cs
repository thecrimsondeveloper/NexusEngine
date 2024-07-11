using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace Toolkit.NexusEngine.Flappy
{
    public class EntityDetectionZone : NexusEntity
    {
        [SerializeField] List<NexusEntity> triggerEntities;
        [SerializeField] List<NexusBlock> triggerBlocks;


        public UnityEvent OnEntityEnter = new UnityEvent();

        public override UniTask Activate()
        {
            return UniTask.CompletedTask;
        }

        public override UniTask Deactivate()
        {
            return UniTask.CompletedTask;
        }

        public void OnTriggerEnter(Collider other)
        {
            NexusEntity entity = other.GetComponent<NexusEntity>();
            if (entity != null)
            {
                Destroy(entity.gameObject);
            }
        }

        protected override void OnInitializeEntity()
        {
            throw new System.NotImplementedException();
        }
    }
}
