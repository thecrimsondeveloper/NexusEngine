using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace LuminaryLabs.Samples
{
    [CreateAssetMenu(menuName = "Luminary Labs/MoveHandler/RigidbodyMoveHandler")]
    public class RigidbodyMoveHandler : MoveHandler<RidigbodyMoveHandlerData>
    {
        [SerializeField] Rigidbody rigidbody;
        public override UniTask Initialize(RidigbodyMoveHandlerData currentData)
        {
            Debug.Log("RigidbodyMoveHandler.Initialize");
            base.Initialize(currentData);
            if (currentData != null)
            {
                this.rigidbody = currentData.rigidbody;
            }

            return UniTask.CompletedTask;
        }

        public override void OnBegin()
        {
        }

        public override UniTask Unload()
        {
            return UniTask.CompletedTask;
        }

        public override void Jump()
        {
            Vector3 velocity = rigidbody.velocity;
            velocity.y = 0;

            rigidbody.velocity = velocity;
            rigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    [System.Serializable]
    public class RidigbodyMoveHandlerData : MoveHandlerData
    {
        public Rigidbody rigidbody;
    }
}
