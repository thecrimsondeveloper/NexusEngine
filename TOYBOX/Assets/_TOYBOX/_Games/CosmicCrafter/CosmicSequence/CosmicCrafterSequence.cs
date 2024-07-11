using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using TMPro;
using Toolkit.Sequences;
using UnityEngine;

namespace ToyBox.Games.CosmicCrafter
{
    public class CosmicCrafterSequence : MonoBehaviour, IAsyncSequence
    {
        public Guid guid { get; set; }
        public IBaseSequence superSequence
        {
            get;
            set;
        }

        [Title("Handlers")]
        public CosmicCrafterRoundHandler roundHandler;
        public CosmicOrbVisuals cosmicCrafterOrbVisuals;
        public CosmicCrafterIntro cosmicCrafterIntro;

        [Title("Cosmic Elements")]
        public RoundPlacementKeyValuePair[] roundPlacementReferences;
        public AnimationClip OnRoundEndClip;
        public AnimationClip OnRoundStartClip;
        public AnimationClip OnGameEndClip;

        [Title("References")]
        public GameObject roundElements;
        public GameObject roundParent;
        public Round[] rounds;
        public TMP_Text[] infoTexts;
        public TMP_Text roundNumberText;

        public void OnSequenceLoad()
        {
            cosmicCrafterIntro.OnCompleteIntro.AddListener(OnCompleteIntro);
        }

        public void OnSequenceStart()
        {

        }

        public void OnSequenceFinished()
        {

        }

        public void OnSequenceUnload()
        {
            cosmicCrafterIntro.OnCompleteIntro.RemoveListener(OnCompleteIntro);
            Sequence.Stop(roundHandler).Forget();
        }

        void OnCompleteIntro()
        {
            // Start round one
            cosmicCrafterOrbVisuals.SetStateToFullSize();
            roundHandler.StartRound(0).Forget();

            roundElements.SetActive(true);
            roundParent.SetActive(true);
        }

        public async UniTask OnLoad_Async()
        {
            // Setup orb to be the intro tutorial
            roundHandler = ScriptableObject.CreateInstance<CosmicCrafterRoundHandler>();

            // Setup the round handler data
            CosmicRoundHandlerData roundHandlerData = new CosmicRoundHandlerData
            {
                rounds = rounds,
                OnRoundEndClip = OnRoundEndClip,
                OnRoundStartClip = OnRoundStartClip,
                OnGameEndClip = OnGameEndClip,
                roundNumberText = roundNumberText,
                infoTexts = infoTexts,
                objectsForRoundToPlace = roundPlacementReferences
            };

            await Sequence.Run(roundHandler, new SequenceRunData { InitializationData = roundHandlerData });
        }

        public UniTask OnFinish_Async()
        {
            return UniTask.CompletedTask;
        }

        public UniTask UnLoad_Async()
        {
            return UniTask.CompletedTask;
        }
    }
}
