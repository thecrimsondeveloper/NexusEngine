using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LuminaryLabs.Sequences
{
    public class SequenceController : MonoBehaviour
    {
        public enum RunMode { OnAwake, OnStart, OnEnable, Manual }
        [SerializeField] RunMode _runMode = RunMode.OnAwake;
        [SerializeField] UnityEngine.Object _sequenceToRun = null;
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
