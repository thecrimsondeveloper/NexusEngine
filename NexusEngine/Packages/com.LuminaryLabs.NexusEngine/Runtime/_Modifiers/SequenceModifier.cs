using System;
using Cysharp.Threading.Tasks;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif
using UnityEngine;

namespace LuminaryLabs.NexusEngine
{
    [CreateAssetMenu(fileName = "New Sequence Modifier", menuName = "LuminaryLabs/Sequence Modifier")]
    public abstract class SequenceModifier : BaseSequence<SequenceModifierData>
    {

        protected override async void OnBegin()
        {

            if (superSequence != null)
            {
                ModifySequence(superSequence);  
            }

            await UniTask.CompletedTask;
            await Sequence.Finish(this);
            await Sequence.Stop(this);
        }

        protected abstract void ModifySequence(ISequence sequence);

        protected override async UniTask Unload()
        {
            // Logic for unloading the sequence modifier
            await UniTask.CompletedTask; // Placeholder for actual unload logic
        }



    }

    public class SequenceModifierData : SequenceData
    {

    }
}
