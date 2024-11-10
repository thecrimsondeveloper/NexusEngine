using System.Collections;
using Cysharp.Threading.Tasks;
using LuminaryLabs.NexusEngine;
using UnityEngine;

namespace LuminaryLabs.NexusEngine
{
    public class AlignSuperWithTransformHandler : BaseSequence<AlignSuperWithTransformHandler.AlignWithSuperTransformData>
    {
        private Transform transformToAlignWith;

        protected override UniTask Initialize(AlignWithSuperTransformData currentData)
        {
            // Assign transforms from the provided data
            transformToAlignWith = currentData.transformToAlignWith;
            return UniTask.CompletedTask;
        }

        protected override async void OnBegin()
        {

            // If we are aligning the super transform, align the up direction as well
            if (this.superSequence != null)
            {
                Transform superTransform = this.superSequence.GetTransform();
                if (superTransform != null)
                {
                    superTransform.forward = transformToAlignWith.forward;
                }
            }
            // Ensure the transforms are set before starting
            await UniTask.NextFrame();
            Complete();
        }

        protected override UniTask Unload()
        {
            // Clean up if necessary
            return UniTask.CompletedTask;
        }

        [System.Serializable]
        public class AlignWithSuperTransformData : BaseSequenceData
        {
            public Transform transformToAlignWith;
        }
    }
}
