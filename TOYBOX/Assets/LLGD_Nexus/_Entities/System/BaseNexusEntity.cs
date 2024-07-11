using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Toolkit.NexusEngine.Flappy;
using UnityEngine;

namespace Toolkit.NexusEngine
{
    public class BaseNexusEntity : NexusEntity
    {
        public override UniTask Activate()
        {
            return UniTask.CompletedTask;
        }

        public override UniTask Deactivate()
        {
            return UniTask.CompletedTask;
        }

        protected override void OnInitializeEntity()
        {
            throw new System.NotImplementedException();
        }
    }
}
