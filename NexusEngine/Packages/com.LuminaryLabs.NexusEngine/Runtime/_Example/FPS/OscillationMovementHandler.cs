using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using LuminaryLabs.NexusEngine;
using UnityEngine;



namespace LuminaryLabs.Example.FPSGame
{
    public class OscillationMovementHandler : MonoSequence<OscillationMovementHandlerData>
    {
        [SerializeField] Transform target;
        [SerializeField] float amplitude;
        [SerializeField] float frequency;

        protected override UniTask Initialize(OscillationMovementHandlerData currentData = null)
        {
            if (currentData.target != null)
                target = currentData.target;
            amplitude = currentData.amplitude;
            frequency = currentData.frequency;
            return UniTask.CompletedTask;
        }

        protected override void OnBegin()
        {

        }

        protected override UniTask Unload()
        {
            return UniTask.CompletedTask;
        }

        public void Update()
        {
            if (target == null)
                return;

            float time = Time.time;
            target.localPosition = Vector3.up * Mathf.Sin(time * frequency) * amplitude;
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
