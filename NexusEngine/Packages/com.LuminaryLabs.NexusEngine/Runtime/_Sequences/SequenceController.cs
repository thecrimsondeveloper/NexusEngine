using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters;
using UnityEngine;

namespace Toolkit.Sequences
{
    public class SequenceController : MonoBehaviour
    {
        public enum RunMode
        {
            OnAwake,
            OnStart,
            OnEnable,
            Manual
        }
        [SerializeField] UnityEngine.Object data = null;
        [SerializeField] RunMode _runMode = RunMode.OnAwake;
        [SerializeField] UnityEngine.Object _sequence = null;


        [SerializeField] UnityEngine.Object _instance;

        private void Awake()
        {
            if (_runMode == RunMode.OnAwake)
            {
                Run();
            }
        }

        private void Start()
        {
            if (_runMode == RunMode.OnStart)
            {
                Run();
            }
        }

        private void OnEnable()
        {
            if (_runMode == RunMode.OnEnable)
            {
                Run();
            }
        }

        private void OnValidate()
        {
            if (_sequence == null) return;

            if (_sequence is GameObject obj)
            {
                Debug.Log("SequenceController: GameObject");
                MonoBehaviour[] monos = obj.GetComponents<MonoBehaviour>();
                Debug.Log("SequenceController: " + monos.Length);
                for (int i = 0; i < monos.Length; i++)
                {
                    Debug.Log("SequenceController: " + monos[i].GetType());
                    if (monos[i] is ISequence)
                    {
                        _sequence = monos[i];
                        break;
                    }
                    else
                    {
                        Debug.LogWarning("SequenceController requires a MonoBehaviour that implements ISequence.");
                        _sequence = null;
                    }
                }
            }
        }


        public async virtual void Run()
        {
            if (IsSequencePrefab())
            {
                if (_sequence is GameObject obj)
                {
                    GameObject _instanceObject = Instantiate(obj, transform);
                    _instance = _instanceObject.GetComponent<ISequence>() as Object;
                }
                else if (_sequence is MonoBehaviour mono)
                {
                    GameObject _instanceObject = Instantiate(mono.gameObject, transform);
                    _instance = _instanceObject.GetComponent<ISequence>() as Object;
                }
            }
            else if (_sequence != null)
            {
                if (IsSequenceScriptable())
                {
                    //create a copy of the scriptable object
                    _instance = Instantiate(_sequence);
                }
                else if (IsSequencePrefab() == false)
                {
                    _instance = null;
                }
            }
            else
            {
                Debug.LogWarning("SequenceController has no sequence to run.");
                return;
            }

            if (_instance is IBaseSequence instanceSequence)
            {
                BeforeRun(instanceSequence);


                SequenceRunData runData = new SequenceRunData
                {
                    InitializationData = data
                };
                await Sequence.Run(instanceSequence, runData);
                AfterRun(instanceSequence);
            }
        }

        protected virtual void BeforeRun(IBaseSequence sequence)
        {

        }

        protected virtual void AfterRun(IBaseSequence sequence)
        {

        }

        public virtual void Stop()
        {
            if (_sequence && _sequence is ISequence sequence)
            {
                Sequence.Stop(sequence);
            }
        }

        bool IsSequencePrefab()
        {
            if (_sequence is GameObject obj)
            {
                return obj.scene.name == null;
            }
            if (_sequence is MonoBehaviour mono)
            {
                return mono.gameObject.scene.name == null;
            }
            return false;
        }

        bool IsSequenceScriptable()
        {
            return _sequence is ScriptableObject;
        }
    }
}
