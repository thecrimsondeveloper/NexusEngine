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

        public string name => this is UnityEngine.Object obj ? obj.name : this.GetType().ToString();
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