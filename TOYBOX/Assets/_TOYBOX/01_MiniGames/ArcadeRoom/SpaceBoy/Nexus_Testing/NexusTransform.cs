using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Toolkit.NexusEngine;
using UnityEngine;

namespace ToyBox
{
    public class NexusTransform : NexusEntity
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

        }


        //create convertion from transform to NexusTransform
        public static implicit operator Transform(NexusTransform transform)
        {
            return transform.transform;
        }

    }
}
