using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using TMPro;
using Toolkit.Sequences;
using Toolkit.Sessions;
using UnityEngine;
using UnityEngine.Events;

namespace ToyBox.Games.CosmicCrafter
{
    public class CosmicCrafterRoundHandler : ScriptableSequence
    {

        protected Round currentRound = null;
        public UnityEvent FinalRoundCompleted = new UnityEvent();

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
            // Logic for post load operations
        }

        protected override void OnStart()
        {
            // Logic for starting the sequence
        }

        protected override void OnFinished()
        {
            // Logic for finishing the sequence
        }

        protected override void OnUnload()
        {
            if (currentRound != null)
                Sequence.Stop(currentRound).Forget();
        }

        public async UniTask StartRound(int round)
        {
            if (currentData is CosmicRoundHandlerData cosmicRoundHandlerData == false)
                return;





            Debug.Log("Starting Round: " + round);
            if (round >= 0 && round < cosmicRoundHandlerData.rounds.Length)
            {
                currentRound = cosmicRoundHandlerData.rounds[round];

                CosmicCrafterRound cosmicCrafterRound = currentRound as CosmicCrafterRound;

                // Setup round data
                List<SpawnPair> spawnPairs = new List<SpawnPair>();
                string[] infoTexts = new string[cosmicRoundHandlerData.infoTexts.Length];

                CosmicCrafterRoundData sequenceData = new CosmicCrafterRoundData
                {
                    spawnPairs = spawnPairs,
                    objectsToPlace = cosmicRoundHandlerData.objectsForRoundToPlace,
                    infoTexts = infoTexts
                };

                // Run the round sequence
                await Sequence.Run(currentRound, new SequenceRunData { InitializationData = sequenceData });
            }
        }
    }

    [System.Serializable]
    public class CosmicRoundHandlerData : RoundHandlerData
    {
        [Title("Asset References")]
        public AnimationClip OnRoundEndClip;
        public AnimationClip OnRoundStartClip;
        public AnimationClip OnGameEndClip;

        [Title("Runtime References")]
        public Round[] rounds;
        public TMP_Text roundNumberText;
        public TMP_Text[] infoTexts;
        public RoundPlacementKeyValuePair[] objectsForRoundToPlace;
    }
}
