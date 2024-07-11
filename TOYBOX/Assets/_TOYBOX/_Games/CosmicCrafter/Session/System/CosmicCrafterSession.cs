using Cysharp.Threading.Tasks;
using UnityEngine;
using Toolkit.Sessions;
using Toolkit.Entity;
using Sirenix.OdinInspector;

namespace ToyBox.Games.CosmicCrafter
{

    public class CosmicCrafterSession : RoundBasedSession
    {
        [SerializeField] Transform orb;
        [SerializeField] float orbRadius = 0.5f;

        [SerializeField, FoldoutGroup("Cosmic Crafter")] CosmicBlackHole blackHole;
        [SerializeField, FoldoutGroup("Cosmic Crafter")] Transform starShot;
        [SerializeField] Animation anim;
        public Transform cosmicCenterOrb;
        CosmicCrafterSessionData sessionData = null;

        protected override RoundBasedSessionData RoundBasedSessionData
        {
            get => sessionData;
            set => sessionData = value as CosmicCrafterSessionData;
        }

        public override UniTask OnLoad()
        {
            blackHole.WhenConsumedStar.AddListener(OnBlackHoleConsumeStar);
            roundHandler.FinalRoundCompleted.AddListener(PlayCredits);
            return UniTask.CompletedTask;
        }

        public override void OnSessionStart()
        {
            //play the intro animation
            //anim.Play("OnCosmicCrafterGameStart")
            //wait for the animation to complete
        }

        public override void OnSessionEnd()
        {

        }

        protected override async UniTask OnRoundSetup(RoundDefinition round)
        {
            try
            {
                if (blackHole is ICompletable targetCompletable)
                {
                    targetCompletable.Reset();
                }
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
            }
        }

        protected async override UniTask OnRoundStart(Round round)
        {
            // if (cannon is IEntity cannonEntity)
            // {
            //     await cannonEntity.Activate();
            // }
        }

        protected override UniTask OnRoundEnd(RoundStats round)
        {
            return UniTask.CompletedTask;
        }

        protected override RoundStats SetupStats(RoundStats currentRoundStats)
        {
            return new CosmicCrafterRoundStats();
        }


        [Button("Play Credits")]
        void PlayCredits()
        {
            anim.Play("OnCosmicCrafterGameEnd");
        }




        public void AddStarStats(StarStats starStats)
        {
            if (currentRoundStats is CosmicCrafterRoundStats cosmicCrafterRound)
            {
                cosmicCrafterRound.totalTries++;

                if ((Session.CurrentSession as RoundBasedSession).TryGetCurrentRound(out Round currentRound))
                {
                    cosmicCrafterRound.totalTime = currentRound as CosmicCrafterRound != null ? (currentRound as CosmicCrafterRound).roundTime : 0;
                }

                float airtime = starStats.airTime;
                float bounces = starStats.bounces;

                cosmicCrafterRound.totalAirTime += airtime;

                if (airtime > cosmicCrafterRound.maxAirTime)
                {
                    cosmicCrafterRound.maxAirTime = airtime;
                }

                // if (bounceHeight > cosmicCrafterRound.maxBounceHeight)
                // {
                //     cosmicCrafterRound.maxBounceHeight = bounceHeight;
                // }

                //cosmicCrafterRound.totalBounces += hits;
            }
        }



        #region Listeners
        void OnBlackHoleConsumeStar(CosmicStar star)
        {
            //set the stats to have the star stats
            if (currentRoundStats is CosmicCrafterRoundStats cosmicCrafterRoundStats)
            {
                cosmicCrafterRoundStats.totalStarsCollected = blackHole.numberOfStars;
            }

            NextRound();
        }


        protected override void OnRoundBasedSessionStart()
        {

        }

        #endregion
    }

}