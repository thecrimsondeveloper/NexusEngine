using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using LuminaryLabs.NexusEngine;
using UnityEngine;



namespace LuminaryLabs.Example.FPS
{
    public class OscillationMovementHandler : ScriptableSequence
    {
        [SerializeField] Transform target;
        [SerializeField] float amplitude;
        [SerializeField] float frequency;
        [SerializeField] Vector3 initialPosition;
        protected override UniTask Initialize(object currentData = null)
        {
            if (currentData is OscillationMovementHandlerData data)
            {
                if (data.target != null)
                    target = data.target;
                amplitude = data.amplitude;
                frequency = data.frequency;

            }


            initialPosition = target == null ? Vector3.zero : target.position;
            return UniTask.CompletedTask;
        }

        protected override void OnBegin()
        {
        }

        protected override UniTask Unload()
        {
            return UniTask.CompletedTask;
        }

        public void RefreshPosition(float time)
        {
            if (target == null)
                return;

            target.position = initialPosition + Vector3.up * Mathf.Sin(time * frequency) * amplitude;
        }
    }

    [System.Serializable]
    public class OscillationMovementHandlerData
    {
        public Transform target;
        public float amplitude;
        public float frequency;
    }
}
