using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using LuminaryLabs.NexusEngine;
using UnityEngine;

namespace LuminaryLabs.Samples
{
    [CreateAssetMenu(menuName = "Luminary Labs/MoveHandler/CharacterControllerMoveHandler")]
    public class TransformMoveHandler : MoveHandler<TransformMoveHandlerData>
    {
        [SerializeField] Transform target;
        [SerializeField] Vector3 slide = new Vector3(0, -1, 0);
        public override UniTask Initialize(TransformMoveHandlerData currentData)
        {
            base.Initialize(currentData);

            if (currentData.target)
            {
                this.target = currentData.target;
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
            if (target)
            {

                target.Translate(slide * Time.fixedDeltaTime * speed);
            }
        }


        protected override UniTask Unload()
        {
            // Implement Unload here
            return UniTask.CompletedTask;
        }

    }

    [System.Serializable]
    public class TransformMoveHandlerData : MoveHandlerData
    {
        public Transform target;
    }
}
