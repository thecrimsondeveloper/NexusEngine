using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Toolkit.NexusEngine
{
    public class NexusEntitySpawner : NexusEntity
    {
        [SerializeField] NexusEntity entityReference;

        public override UniTask Activate()
        {
            throw new System.NotImplementedException();
        }

        public override UniTask Deactivate()
        {
            throw new System.NotImplementedException();
        }

        public void Spawn()
        {
            NexusEntity entity = Instantiate(entityReference, transform.position, Quaternion.identity, transform);
            entity.InitializeEntity(new NexusEntity.InitializeData
            {
                parent = entity.transform,
                space = Space.Self,
            });
        }

        protected override void OnInitializeEntity()
        {
        }
    }
}
