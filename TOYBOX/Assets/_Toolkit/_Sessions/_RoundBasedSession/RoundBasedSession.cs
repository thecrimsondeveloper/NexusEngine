using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using ToyBox;
using UnityEngine;
using UnityEngine.Events;

namespace Toolkit.Sessions
{
    public abstract class RoundBasedSession : Session
    {

        protected abstract RoundBasedSessionData RoundBasedSessionData { get; set; }
        public override SessionData SessionData
        {
            get => RoundBasedSessionData;
            set => RoundBasedSessionData = value as RoundBasedSessionData;
        }

        [SerializeField, FoldoutGroup("Progression", 500)] protected bool startRoundOnStart = false;
        [SerializeField, FoldoutGroup("Progression", 500)] protected RoundHandler roundHandler = null;
        [SerializeReference, FoldoutGroup("Progression", 500)] List<RoundStats> previousRoundStats = new List<RoundStats>();

        [FoldoutGroup("Progression", 500)]
        [SerializeField, ReadOnly] int currentRoundIndex = 0;

        [FoldoutGroup("Progression", 500)]
        [SerializeReference, ReadOnly] protected RoundStats currentRoundStats = null;
        public static UnityEvent<RoundStats> OnRoundEndEvent = new UnityEvent<RoundStats>();
        public static UnityEvent<RoundDefinition> OnRoundStartEvent = new UnityEvent<RoundDefinition>();
        public static UnityEvent<RoundStats> AfterRoundStartEvent = new UnityEvent<RoundStats>();

        public bool TryGetCurrentRound(out Round currentRound)
        {
            if (roundHandler)
            {
                if (roundHandler.TryGetCurrentRound(out currentRound))
                {
                    return true;
                }
                else
                {
                    currentRound = null;
                    return false;
                }
            }
            else
            {
                currentRound = null;
                return false;
            }
        }


        public async void NextRound()
        {
            //if we are not in a round, start the first round
            if (currentRoundStats == null && RoundBasedSessionData.TryGetRound(currentRoundIndex, out var first))
            {
                await StartRound(first);
                Debug.Log("Starting first round");
                return;
            }
            else if (currentRoundStats != null) //if we are in a round, end it
            {
                Debug.Log("Ending round");
                await EndRound(currentRoundStats);
            }


            if (currentRoundIndex >= RoundBasedSessionData.rounds.Count)
            {

                await ConcludeRounds();
                Debug.Log("Session complete");
            }

            currentRoundIndex++;
            if (RoundBasedSessionData.TryGetRound(currentRoundIndex, out var round))
            {
                await StartRound(round);
            }
        }

        protected async virtual UniTask SetupRound(RoundDefinition round)
        {
            currentRoundStats = SetupStats(currentRoundStats);
            currentRoundStats.roundIndex = currentRoundIndex;

            OnRoundStartEvent.Invoke(round);
            await OnRoundSetup(round);
            AfterRoundStartEvent.Invoke(currentRoundStats);
        }

        private async UniTask StartRound(RoundDefinition round)
        {
            await SetupRound(round);
            if (roundHandler) await roundHandler.StartRound(round, currentRoundStats);
        }

        private async UniTask EndRound(RoundStats round)
        {
            await OnRoundEnd(round);
            previousRoundStats.Add(round);
            if (roundHandler) await roundHandler.EndRound(round);
        }

        private async UniTask ConcludeRounds()
        {
            if (roundHandler) await roundHandler.ConcludeRounds();
            OnRoundEndEvent.Invoke(currentRoundStats);
        }



        protected abstract RoundStats SetupStats(RoundStats currentRoundStats);
        protected abstract UniTask OnRoundSetup(RoundDefinition round);
        protected abstract UniTask OnRoundStart(Round round);
        protected abstract UniTask OnRoundEnd(RoundStats round);

        public override void OnSessionStart()
        {
            OnRoundBasedSessionStart();
            if (startRoundOnStart)
            {
                NextRound();
            }
        }

        protected abstract void OnRoundBasedSessionStart();




    }
}
