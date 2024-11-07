using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using LuminaryLabs.NexusEngine;
using UnityEngine;

public class AddPositionToTransform : SequenceModifier
{
        private Vector3 positionToAdd;

    protected override UniTask Initialize(SequenceModifierData currentData)
    {
        if(currentData is AddPositionToTransformData addPositionData)
        {
            positionToAdd = addPositionData.positionToAdd;
        }
        return UniTask.CompletedTask;
    }

    protected override void ModifySequence(ISequence sequence)
    {
        if(sequence is MonoBehaviour monoSequence)
        {
            monoSequence.transform.position += positionToAdd;
        }
    }
}

public class AddPositionToTransformData : SequenceModifierData
{
    public Vector3 positionToAdd;

    // Data for the sequence modifier
}
