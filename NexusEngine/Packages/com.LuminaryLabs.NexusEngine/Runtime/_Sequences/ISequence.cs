using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace LuminaryLabs.NexusEngine
{
    public interface ISequence
    {
        
        ISequence superSequence { get; set; }
        Guid guid { get; set; }
        object currentData { get; set; }
        public Phase phase {get; set;}

        UniTask InitializeSequence(object currentData = null);
        void OnBeginSequence();
        UniTask FinishSequence();
        UniTask UnloadSequence();

        void OnFinishedSequence();
        void OnUnloadedSequence();

        public Transform GetTransform()
        {
            return this is MonoBehaviour monoBehaviour ? monoBehaviour.transform : superSequence?.GetTransform();
        }

        public bool TryGetUnityComponent<T>(out T component) where T : Component
        {
            if (this is MonoBehaviour monoBehaviour)
            {
                component = monoBehaviour.GetComponent<T>();
                return component != null;
            }
            else if (superSequence != null)
            {
                return superSequence.TryGetUnityComponent(out component);
            }   

            component = null;
            return false;
        }

        public string name => this is UnityEngine.Object obj ? 
                            (obj.GetType().ToString()) : this.GetType().ToString();

        public async void Complete()
        {
            if (phase == Phase.Finished)
            {
                return;
            }
            phase = Phase.Finished;
            await Sequence.Finish(this);
            await Sequence.Stop(this);
        }
    }
        public enum Phase 
        {
            Idle,
            Initialization,
            Begin,
            Run,
            Finished,
            Unloading,
        }
}