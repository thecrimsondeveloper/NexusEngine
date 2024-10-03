using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using LuminaryLabs.NexusEngine;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif
using UnityEngine;

namespace LuminaryLabs.Example.FPSGame
{
    public class OscillationEntityDirector : CoreSequence<OscillationEntityDirectorData>
    {
        protected override UniTask Initialize(OscillationEntityDirectorData currentData = null)
        {
            return UniTask.CompletedTask;
        }

        protected override void OnBegin()
        {
            if (currentData.oscillationEntity != null)
            {
                Debug.Log("OscillationEntityDirector Began");
                Sequence.Run(currentData.oscillationEntity, new SequenceRunData
                {
                    superSequence = this,
                    sequenceData = currentData.oscillationEntityData,
                    parent = currentData.entityParent,
                    onInitialize = (Sequence) => Debug.Log("OscillationEntity Initialized"),
                    onBegin = (Sequence) => Debug.Log("OscillationEntity Began"),
                    onFinished = (Sequence) => Debug.Log("OscillationEntity Finished"),
                });
            }
        }

        protected override UniTask Unload()
        {
            return UniTask.CompletedTask;
        }
    }

    [System.Serializable]
    public class OscillationEntityDirectorData : CoreSequenceData
    {
        public Transform entityParent;

        public OscillationEntity oscillationEntity;

        public OscillationEntityData oscillationEntityData;
    }
}
