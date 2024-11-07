using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using LuminaryLabs.NexusEngine;
using UnityEngine;

namespace LuminaryLabs.Samples
{
    public abstract class MoveHandler : MonoSequence
    {
        public float speed = 1.0f;
        public float jumpForce = 1.0f;
        public abstract void Jump();
    }

    public abstract class MoveHandler<T> : MoveHandler where T : MoveHandlerData
    {
        protected override async UniTask Initialize(object currentData)
        {
            Debug.Log("MoveHandler.Initialize");
            if (currentData is T data)
            {
                await Initialize(data);
            }
            else
            {
                Debug.LogError("MoveHandler.Initialize: currentData is not of type T");
            }
        }

        public virtual UniTask Initialize(T currentData)
        {
            Debug.Log("MoveHandler.Initialize");
            speed = currentData.speed;
            jumpForce = currentData.jumpForce;

            return UniTask.CompletedTask;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Jump();
            }
        }

        public override void Jump()
        {

        }
    }

    public class MoveHandlerData : SequenceData
    {
        // Implement data here
        public float speed = 1.0f;
        public float jumpForce = 1.0f;
    }
}


