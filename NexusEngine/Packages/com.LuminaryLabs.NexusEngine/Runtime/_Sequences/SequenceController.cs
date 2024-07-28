using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters;
using UnityEngine;
using Sirenix.OdinInspector;

namespace LuminaryLabs.Sequences
{
    public class SequenceController : MonoBehaviour
    {

        [SerializeField] UnityEngine.Object data = null;

        public enum RunMode { OnAwake, OnStart, OnEnable, Manual }
        public enum SequenceSource { Object, Library }
        [SerializeField] RunMode _runMode = RunMode.OnAwake;
        [SerializeField] SequenceSource _source = SequenceSource.Object;


        [ShowIf(nameof(_source), SequenceSource.Library)]
        [SerializeField] SequenceLibrary _library = null;
        [ShowIf(nameof(_source), SequenceSource.Library)]
        [SerializeField] string sequenceName = null;
        [ShowIf(nameof(_source), SequenceSource.Library)]


#if UNITY_EDITOR

        [ShowIf(nameof(_source), SequenceSource.Library)]
        [ShowIf(nameof(_library)), ShowIf(nameof(currentLibraryObject))]
        [ShowInInspector, ReadOnly, LabelText("Current Object")]
        UnityEngine.Object currentLibraryObject => _library.Get(sequenceName);

#endif

        [ShowIf(nameof(_source), SequenceSource.Object)]
        [SerializeField] UnityEngine.Object _sequenceToRun = null;

        [ShowIf(nameof(_instance))]
        [SerializeField] UnityEngine.Object _instance;

        private void Awake() { if (_runMode == RunMode.OnAwake) Run(); }
        private void Start() { if (_runMode == RunMode.OnStart) Run(); }
        private void OnEnable() { if (_runMode == RunMode.OnEnable) Run(); }

        private void OnValidate()
        {
            if (_sequenceToRun is GameObject obj)
            {
                foreach (var mono in obj.GetComponents<MonoBehaviour>())
                {
                    if (mono is ISequence)
                    {
                        _sequenceToRun = mono;
                        return;
                    }
                }
                Debug.LogWarning("SequenceController requires a MonoBehaviour that implements ISequence.");
                _sequenceToRun = null;
            }
        }

        public virtual void Run()
        {
            if (_source == SequenceSource.Library)
            {
                if (_library == null)
                {
                    Debug.LogWarning("SequenceController has no library to get sequence from.");
                    return;
                }

                if (_library.TryGet(sequenceName, out var librarySequence))
                {
                    _sequenceToRun = librarySequence;
                }
                else
                {
                    Debug.LogWarning($"SequenceController could not find sequence with name {sequenceName} in library.");
                    return;
                }
            }




            Object temp = _sequenceToRun;
            if (IsSequencePrefab() && _instance == null)
            {
                temp = Instantiate((_sequenceToRun as MonoBehaviour)?.gameObject ?? _sequenceToRun as GameObject, transform);
            }
            else if (_sequenceToRun == null)
            {
                Debug.LogWarning("SequenceController has no sequence to run.");
                return;
            }

            if (_sequenceToRun is ISequence sequence)
            {
                BeforeRun(sequence);
                Sequence.Run(sequence);
                AfterRun(sequence);

                _instance = temp;
            }
        }

        protected virtual void BeforeRun(ISequence sequence) { }
        protected virtual void AfterRun(ISequence sequence) { }

        public virtual void Stop()
        {
            if (_sequenceToRun is ISequence sequence)
            {
                Sequence.Stop(sequence);
            }
        }

        bool IsSequencePrefab() => _sequenceToRun is MonoBehaviour mono && mono.gameObject.scene.name == null;
        bool IsSequenceScriptable() => _sequenceToRun is ScriptableObject;
    }
}
