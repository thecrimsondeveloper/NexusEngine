using UnityEngine;
using Cysharp.Threading.Tasks;

namespace LuminaryLabs.NexusEngine.UnityHandlers
{
    public class WaitForCollisionHandler : EntitySequence<WaitForCollisionHandlerData>
    {
        public enum CollisionType
        {
            Enter,
            Exit
        }

        private Collider _collider;
        private CollisionType _collisionType;

        protected override UniTask Initialize(WaitForCollisionHandlerData currentData)
        {
            // Assign the collider reference and collision type
            _collider = currentData.collider;
            _collisionType = currentData.collisionType;

            return UniTask.CompletedTask;
        }

        protected override void OnBegin()
        {
            // No need for async behavior, we will rely on OnCollisionEnter or OnCollisionExit
        }

        // Unity's OnCollisionEnter event
        private void OnCollisionEnter(Collision collision)
        {
            if (_collisionType == CollisionType.Enter && (_collider == null || collision.collider == _collider))
            {
                // Finish the sequence on collision enter
                Sequence.Finish(this);
                Sequence.Stop(this);
            }
        }

        // Unity's OnCollisionExit event
        private void OnCollisionExit(Collision collision)
        {
            if (_collisionType == CollisionType.Exit && (_collider == null || collision.collider == _collider))
            {
                // Finish the sequence on collision exit
                Sequence.Finish(this);
                Sequence.Stop(this);
            }
        }

        protected override UniTask Unload()
        {
            return UniTask.CompletedTask;
        }
    }

    [System.Serializable]
    public class WaitForCollisionHandlerData : SequenceData
    {
        public Collider collider; // Optional: the specific collider to wait for (null if any collision is acceptable)
        public WaitForCollisionHandler.CollisionType collisionType; // Specifies whether to finish on enter or exit
    }
}
