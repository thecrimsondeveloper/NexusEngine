using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Toolkit.Sequences
{
    [CreateAssetMenu(fileName = "SequenceState", menuName = "Sequences/SequenceState")]
    public class SequenceState : ScriptableSequence
    {
        public int ID;
        public SequenceState[] SubSequenceReferences;

        [ShowInInspector, ReadOnly]
        public IBaseSequence loadedInstance;

        protected override async UniTask WhenLoad()
        {
            if (currentData is SequenceLibrary library == false)
            {
                Debug.LogError("SequenceState requires a SequenceLibrary as currentData.");
                return;
            }

            // Load logic for this sequence
            Debug.Log($"Loading SequenceState {ID}");
            Object obj = library.Get(ID);
            if (obj == null)
            {
                Debug.LogError($"SequenceState {ID} not found in SequenceLibrary");
                return;
            }

            if (obj is GameObject gameObject)
            {
                GameObject instance = Instantiate(gameObject);
                loadedInstance = instance.GetComponent<IBaseSequence>();
                if (loadedInstance == null)
                {
                    Debug.LogError($"Loaded instance from prefab does not contain an IBaseSequence component.");
                    return;
                }
            }
            else if (obj is IBaseSequence sequence)
            {
                loadedInstance = sequence;
            }
            else if (obj is ScriptableObject scriptableObject)
            {
                loadedInstance = Instantiate(scriptableObject) as IBaseSequence;
            }

            // Load sub-sequences
            foreach (var subSequenceID in SubSequenceReferences)
            {
                SequenceRunData runData = new SequenceRunData
                {
                    SuperSequence = loadedInstance,
                    InitializationData = currentData
                };
                if (subSequenceID != null)
                {
                    await Sequence.Run(subSequenceID, runData);
                }
            }

            Debug.Log($"Loaded SequenceState {ID}");
        }

        protected override void OnStart()
        {
            // Start logic for this sequence
            Debug.Log($"Starting SequenceState {ID}");
            if (loadedInstance == null)
            {
                Debug.LogError($"Loaded instance for SequenceState {ID} is null.");
                return;
            }

            SequenceRunData runData = new SequenceRunData
            {
                SuperSequence = this,
                InitializationData = currentData
            };
            Sequence.Run(loadedInstance, runData).Forget();
        }

        protected override async UniTask Finish()
        {
            // Finish logic for this sequence
            Debug.Log($"Finishing SequenceState {ID}");
            await UniTask.CompletedTask;
        }

        protected override async UniTask Unload()
        {
            // Unload logic for this sequence
            Debug.Log($"Unloading SequenceState {ID}");
            await UniTask.CompletedTask;
        }

        protected override void AfterLoad()
        {
            // Logic after load, if any
        }

        protected override void OnFinished()
        {
            // Logic when finished, if any
        }

        protected override void OnUnload()
        {
            // Logic when unloaded, if any
        }
    }
}
