using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using ToyBox;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

namespace Toolkit.Sessions
{
    public abstract class RoundHandler : MonoBehaviour
    {
        protected Round currentRound = null;
        protected RoundDefinition currentDefinition = null;
        [SerializeField] Round[] rounds = null;

        public UnityEvent FinalRoundCompleted = new UnityEvent();

        public bool TryGetCurrentRound(out Round round)
        {
            if (currentRound)
            {
                round = currentRound;
            }
            else
            {
                round = null;
                return false;
            }

            return true;
        }

        public async UniTask StartRound(RoundDefinition definition, RoundStats stats)
        {
            //if we are in a round, end it
            if (currentRound != null)
            {
                // await currentRound.Deactivate();
            }



            //if the index is out of bounds, return
            if (stats.roundIndex >= 0 && stats.roundIndex < rounds.Length)
            {
                currentRound = rounds[stats.roundIndex];
                currentDefinition = definition;
                // currentRound.definition = definition;
                // await currentRound.Activate();
                await OnRoundStart(currentRound, stats);
            }
        }

        public async UniTask EndRound(RoundStats stats)
        {
            // if (currentRound) await currentRound.Deactivate();
            await OnRoundEnd(stats);
        }

        public async UniTask ConcludeRounds()
        {
            FinalRoundCompleted.Invoke();
            await OnConcludeRounds();
        }

        protected abstract UniTask OnRoundStart(Round round, RoundStats stats);
        protected abstract UniTask OnRoundEnd(RoundStats round);
        protected abstract UniTask OnConcludeRounds();


    }

    public class RoundHandlerData : object
    {

    }
}
