using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace LuminaryLabs.Samples
{
    [CreateAssetMenu(menuName = "Luminary Labs/MoveHandler/RigidbodyMoveHandler")]
    public class RigidbodyMoveHandler : MoveHandler<RidigbodyMoveHandlerData>
    {
        [SerializeField] Rigidbody rigidbody;
        [SerializeField] float gravityMultiplier = 1;
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

        protected override void OnBegin()
        {
        }

        protected override UniTask Unload()
        {
            return UniTask.CompletedTask;
        }

        public override void Jump()
        {
            Vector3 velocity = rigidbody.velocity;
            velocity.y = 0;

            rigidbody.velocity = velocity;
            rigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

            rigidbody.AddForce(Physics.gravity * gravityMultiplier, ForceMode.Acceleration);
        }
    }

    [System.Serializable]
    public class RidigbodyMoveHandlerData : MoveHandlerData
    {
        public Rigidbody rigidbody;
    }
}
