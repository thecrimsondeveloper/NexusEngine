using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using LuminaryLabs.Sequences;
using Sirenix.OdinInspector;
using UnityEngine;


namespace LuminaryLabs.Example.FPS
{

    public class OscillationEntityDirector : MonoSequence
    {

        [SerializeField] OscillationEntityDirectorData data;
        protected override UniTask Initialize(object currentData = null)
        {
            return UniTask.CompletedTask;
        }

        protected override async void OnBegin()
        {
            if (data.oscillationEntity != null)
            {

                await Sequence.Run(data.oscillationEntity, new SequenceRunData
                {
                    superSequence = this,
                    sequenceData = data.oscillationEntityData,
                    parent = data.entityParent,
                    onInitialize = () => Debug.Log("OscillationEntity Initialized"),
                    onBegin = () => Debug.Log("OscillationEntity Began"),
                    onFinished = () => Debug.Log("OscillationEntity Finished"),
                });
            }
        }

        protected override UniTask Unload()
        {
            return UniTask.CompletedTask;
        }
    }

    [System.Serializable]
    public class OscillationEntityDirectorData
    {
        public Transform entityParent;
        public OscillationEntity oscillationEntity;
        public OscillationEntityData oscillationEntityData;
    }

}