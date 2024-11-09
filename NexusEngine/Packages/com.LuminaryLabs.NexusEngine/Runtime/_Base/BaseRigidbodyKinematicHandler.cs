using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace LuminaryLabs.NexusEngine
{
    public class BaseRigidbodyKinematicHandler : BaseSequence<BaseRigidbodyKinematicHandler.BaseRigidbodyKinematicHandlerData>
    {
        private Rigidbody rigidbody;
        private KindematicMode kindematicMode;

        public enum KindematicMode
        {
            ON,
            OFF,
            TOGGLE,
        }

        protected override UniTask Initialize(BaseRigidbodyKinematicHandlerData currentData)
        {
            rigidbody = currentData.rigidbody;
            kindematicMode = currentData.kindematicMode;

            return UniTask.CompletedTask;
        }

        protected override async void OnBegin()
        {
            switch (kindematicMode)
            {
                case KindematicMode.ON:
                    rigidbody.isKinematic = true;
                    break;
                case KindematicMode.OFF:
                    rigidbody.isKinematic = false;
                    break;
                case KindematicMode.TOGGLE:
                    rigidbody.isKinematic = !rigidbody.isKinematic;
                    break;
            }

            await UniTask.NextFrame();
            this.Complete();
        }

        public class BaseRigidbodyKinematicHandlerData : BaseSequenceData
        {
            public Rigidbody rigidbody;
            public KindematicMode kindematicMode;
        }
    }
}
