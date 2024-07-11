using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Toolkit.Sequences;
using UnityEngine;

namespace ToyBox
{
    public class RoundStartHandler : ScriptableSequence
    {
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
            // Logic for after load
        }

        protected override void OnStart()
        {
            if (currentData is StartHandlerData data)
            {
                foreach (var roundPlacementReference in data.roundPlacementReferences)
                {
                    // Implement the logic for handling round placement references
                    Debug.Log($"Handling round placement reference: {roundPlacementReference.prefabKey}");
                }
            }
        }

        protected override void OnFinished()
        {
            // Logic for when sequence is finished
        }

        protected override void OnUnload()
        {
            // Logic for unloading the sequence
        }

        public class StartHandlerData
        {
            public RoundPlacementKeyValuePair[] roundPlacementReferences;
        }
    }
}
