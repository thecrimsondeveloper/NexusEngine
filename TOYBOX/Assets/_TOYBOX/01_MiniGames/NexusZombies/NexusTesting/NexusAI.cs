using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Toolkit.NexusEngine;
using UnityEngine;

namespace ToyBox
{
    public class NexusAI : NexusEntity
    {
        public NexusMovement movement;
        protected override void OnInitializeEntity()
        {
            ResolveComponent(out movement);


            movement.InitializeBlock(this);
        }

        public override UniTask Activate()
        {
            return UniTask.CompletedTask;
        }

        public override UniTask Deactivate()
        {
            return UniTask.CompletedTask;
        }

    }
}
