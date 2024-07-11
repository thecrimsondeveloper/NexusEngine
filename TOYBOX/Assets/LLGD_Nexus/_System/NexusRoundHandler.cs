using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using ToyBox;
using UnityEngine;

namespace Toolkit.NexusEngine
{
    public class NexusRoundHandler : NexusGameLoopHandler
    {
        public NexusInt numberOfRounds;
        public NexusInt currentRound;

        public NexusEventBlock OnRoundStart = new NexusEventBlock();
        public NexusEventBlock OnRoundEnd = new NexusEventBlock();
        public NexusEventBlock OnRoundStartChange = new NexusEventBlock();
        public NexusEventBlock AfterRoundFinishChange = new NexusEventBlock();
        public NexusEventReceiver NextRoundEventReceiver = new NexusEventReceiver();

        protected override void OnInitializeEntity()
        {
            base.OnInitializeEntity();

            if (numberOfRounds == null)
            {
                numberOfRounds = ScriptableObject.CreateInstance<NexusInt>();
                numberOfRounds.Set(0);
            }

            if (currentRound == null)
            {
                currentRound = ScriptableObject.CreateInstance<NexusInt>();
                currentRound.Set(0);
            }


            NextRoundEventReceiver.AddListener(NextRound);

            numberOfRounds.InitializeObject();
            currentRound.InitializeObject();

            OnRoundStart.InitializeObject();
            OnRoundEnd.InitializeObject();
            OnRoundStartChange.InitializeObject();
            AfterRoundFinishChange.InitializeObject();
            NextRoundEventReceiver.InitializeObject();
        }

        public void NextRound()
        {
            OnRoundEnd.InvokeBlock();
        }











    }
}
