using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace LuminaryLabs.NexusEngine
{
    public class BaseRigidbodyKinematicHandler : BaseSequence<BaseRigidbodyKinematicHandler.BaseRigidbodyKinematicHandlerData>
    {
        private Rigidbody rigidbody;
        private KinematicMode kindematicMode;

        public enum KinematicMode
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
                case KinematicMode.ON:
                    rigidbody.isKinematic = true;
                    break;
                case KinematicMode.OFF:
                    rigidbody.isKinematic = false;
                    break;
                case KinematicMode.TOGGLE:
                    rigidbody.isKinematic = !rigidbody.isKinematic;
                    break;
            }

            await UniTask.NextFrame();
            this.Complete();
        }

        public class BaseRigidbodyKinematicHandlerData : BaseSequenceData
        {
            public Rigidbody rigidbody;
            public KinematicMode kindematicMode;
        }
    }
}
