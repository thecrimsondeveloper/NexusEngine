using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Toolkit.NexusEngine
{
    public class NexusGameLoopHandler : NexusEntity
    {
        public override UniTask Activate()
        {
            gameObject.SetActive(true);
            return UniTask.CompletedTask;
        }

        public override UniTask Deactivate()
        {
            gameObject.SetActive(false);
            return UniTask.CompletedTask;
        }

        protected override void OnInitializeEntity()
        {
        }
    }
}
