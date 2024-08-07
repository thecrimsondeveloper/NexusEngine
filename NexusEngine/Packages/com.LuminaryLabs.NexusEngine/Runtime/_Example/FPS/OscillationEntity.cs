using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using LuminaryLabs.NexusEngine;
using Sirenix.OdinInspector;
using UnityEngine;


namespace LuminaryLabs.Example.FPS
{
    public class OscillationEntity : MonoSequence
    {
        [SerializeField] OscillationMovementHandler oscillationMovementHandler;
        protected override UniTask Initialize(object currentData = null)
        {
            oscillationMovementHandler = ScriptableObject.CreateInstance<OscillationMovementHandler>();
            return UniTask.CompletedTask;
        }
        protected override void OnBegin()
        {
            if (currentData is OscillationEntityData data)
            {
                Sequence.Run(oscillationMovementHandler, new SequenceRunData
                {
                    superSequence = this,
                    sequenceData = new OscillationMovementHandlerData
                    {
                        target = transform,
                        amplitude = data.amplitude,
                        frequency = data.frequency
                    }
                });
            }
        }

        private void Update()
        {
            if (oscillationMovementHandler != null) oscillationMovementHandler.RefreshPosition(Time.time);
        }

        protected override UniTask Unload()
        {
            return UniTask.CompletedTask;
        }
    }

    [System.Serializable]
    public class OscillationEntityData
    {
        public float amplitude;
        public float frequency;
    }
}
