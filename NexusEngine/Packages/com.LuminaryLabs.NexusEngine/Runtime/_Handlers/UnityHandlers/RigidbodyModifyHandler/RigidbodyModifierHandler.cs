using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace LuminaryLabs.NexusEngine.UnityHandlers
{
    public class RigidbodyModifierHandler : EntitySequence<RigidbodyModifierData>
    {
        public enum ModifyAction
        {
            SetVelocity,
            AddForce,
            AddImpulse
        }

        private ModifyAction _action;
        public List<Rigidbody> rigidbodies;

        protected override UniTask Initialize(RigidbodyModifierData currentData)
        {
            _action = currentData.modifyAction;
            if (currentData.rigidbodies != null)
                rigidbodies = currentData.rigidbodies;

            return UniTask.CompletedTask;
        }

        protected override void OnBegin()
        {
            switch (_action)
            {
                case ModifyAction.SetVelocity:
                    foreach (var rb in rigidbodies)
                    {
                        rb.velocity = currentData.targetVelocity;
                    }
                    break;
                case ModifyAction.AddForce:
                    foreach (var rb in rigidbodies)
                    {
                        rb.AddForce(currentData.forceDirection * currentData.forceMagnitude, ForceMode.Force);
                    }
                    break;
                case ModifyAction.AddImpulse:
                    foreach (var rb in rigidbodies)
                    {
                        rb.AddForce(currentData.forceDirection * currentData.impulseMagnitude, ForceMode.Impulse);
                    }
                    break;
            }

            Sequence.Finish(this);
            Sequence.Stop(this);
        }

        protected override UniTask Unload()
        {
            return UniTask.CompletedTask;
        }
    }

    [System.Serializable]
    public class RigidbodyModifierData : SequenceData
    {
        public RigidbodyModifierHandler.ModifyAction modifyAction;
        public List<Rigidbody> rigidbodies;
        public Vector3 targetVelocity;
        public Vector3 forceDirection;
        public float forceMagnitude = 1.0f;
        public float impulseMagnitude = 1.0f;
    }
}
