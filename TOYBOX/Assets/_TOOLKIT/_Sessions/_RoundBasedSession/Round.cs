using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using Toolkit.Sequences;
using UnityEngine;

namespace ToyBox
{
    public abstract class Round : MonoSequence
    {
        [Title("Handlers")]
        [HideInEditorMode] public RoundSetupHandler setupHandler;
        [HideInEditorMode] public RoundStartHandler startHandler;

        [Title("Round Data")]
        public RoundData roundData;

        [Space(10)]
        [Title("References")]
        [SerializeField] Transform contentParent = null;
        [ListDrawerSettings(ShowFoldout = false)]
        public List<RoundPlacementKeyValuePair> roundPlacementReferences = new List<RoundPlacementKeyValuePair>();

        [HideInEditorMode] public float roundTime = 0;

        public Transform ContentParent => contentParent;
        Dictionary<string, GameObject> placementReferences = new Dictionary<string, GameObject>();

        private void OnValidate()
        {
            // Loop through all references and if the name is empty, set it to the name of the GameObject
            foreach (var reference in roundPlacementReferences)
            {
                if (string.IsNullOrEmpty(reference.prefabKey) && reference.placementReference != null)
                {
                    reference.prefabKey = reference.placementReference.name;
                }
            }
        }

        protected override UniTask Finish()
        {
            // Logic for async finish
            return UniTask.CompletedTask;
        }

        protected override UniTask WhenLoad()
        {
            // Logic for async load
            return UniTask.CompletedTask;
        }

        protected override UniTask Unload()
        {
            // Logic for async unload
            return UniTask.CompletedTask;
        }

        protected override void AfterLoad()
        {
            Debug.Log("AfterLoad");
            if (setupHandler == null)
            {
                setupHandler = ScriptableObject.CreateInstance<RoundSetupHandler>();
            }
            setupHandler.Initialize(this);

            foreach (var reference in roundPlacementReferences)
            {
                Debug.Log("Adding reference: " + reference.prefabKey);
                if (!placementReferences.ContainsKey(reference.prefabKey))
                {
                    placementReferences.Add(reference.prefabKey, reference.placementReference);
                }
            }

            contentParent.gameObject.SetActive(true);
        }

        protected override void OnStart()
        {
            OnRoundStart();
        }

        protected virtual void OnRoundStart()
        {
            var startHandlerData = new RoundStartHandler.StartHandlerData
            {
                roundPlacementReferences = roundPlacementReferences.ToArray()
            };
            startHandler.currentData = startHandlerData;
            Sequence.Run(startHandler, new SequenceRunData { SuperSequence = this }).Forget();
        }

        protected override void OnFinished()
        {
            roundTime = Time.time - roundTime;
            // Round the time to the nearest 0.1
            roundTime = Mathf.Round(roundTime * 10) / 10;
            OnRoundEnd();
        }

        protected virtual void OnRoundEnd()
        {
            // Custom logic for ending the round
        }

        protected override void OnUnload()
        {
            contentParent.gameObject.SetActive(false);
        }

        public bool TryGetPlacementReference(string positionReferenceKey, out GameObject placementReference)
        {
            Debug.Log("TryGetPlacementReference: " + positionReferenceKey);
            if (placementReferences.TryGetValue(positionReferenceKey, out var reference))
            {
                Debug.Log("Found reference: " + reference.name);
                placementReference = reference;
                return true;
            }
            else
            {
                Debug.Log("No reference found");
            }

            placementReference = null;
            return false;
        }

        public bool TryGetPlacementReferencePosition(string prefabKey, out Vector3 referencePosition)
        {
            if (TryGetPlacementReference(prefabKey, out var reference))
            {
                referencePosition = reference.transform.position;
                return true;
            }

            referencePosition = Vector3.zero;
            return false;
        }
    }

    public class RoundData
    {
    }

    [Serializable]
    public class RoundPlacementKeyValuePair
    {
        public string prefabKey;
        public GameObject placementReference;
    }

    public interface IRoundInitializable
    {
        void InitializeForRound(GameObject reference);
    }
}

