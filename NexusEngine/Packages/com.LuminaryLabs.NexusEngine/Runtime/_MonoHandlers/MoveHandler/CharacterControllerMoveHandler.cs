using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using LuminaryLabs.NexusEngine;
using UnityEngine;

namespace LuminaryLabs.Samples
{
    [CreateAssetMenu(menuName = "Luminary Labs/MoveHandler/CharacterControllerMoveHandler")]
    public class CharacterControllerMoveHandler : MoveHandler<CharacterControllerMoveHandlerData>
    {
        [SerializeField] CharacterController characterController;
        [SerializeField] Vector3 velocity = Vector3.zero;
        [SerializeField] Vector3 gravity = new Vector3(0, -9.81f, 0);
        [SerializeField] Vector3 slide = new Vector3(0, -1, 0);
        [SerializeField] float drag = 0.1f;
        public override UniTask Initialize(CharacterControllerMoveHandlerData currentData)
        {
            base.Initialize(currentData);

            if (currentData.characterController)
            {
                this.characterController = currentData.characterController;
            }
            // Implement Initialize here
            return UniTask.CompletedTask;
        }
        protected override void OnBegin()
        {
            // Implement OnBegin here
        }

        private void FixedUpdate()
        {
            if (characterController)
            {

                characterController.Move(velocity * Time.fixedDeltaTime);


                velocity += gravity * Time.fixedDeltaTime;
                velocity *= 1 - drag * Time.fixedDeltaTime;

                characterController.Move(slide * Time.fixedDeltaTime);
            }
        }


        protected override UniTask Unload()
        {
            // Implement Unload here
            return UniTask.CompletedTask;
        }

        public override void Jump()
        {
            // Implement Jump here
            velocity.y = jumpForce;
        }
    }

    [System.Serializable]
    public class CharacterControllerMoveHandlerData : MoveHandlerData
    {
        public CharacterController characterController;
    }
}
