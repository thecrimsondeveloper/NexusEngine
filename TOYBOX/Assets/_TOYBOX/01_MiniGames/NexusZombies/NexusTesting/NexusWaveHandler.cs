using System.Collections;
using System.Collections.Generic;
using Toolkit.NexusEngine;
using UnityEngine;

namespace Toolkit.NexusEngine
{
    public class NexusWaveHandler : NexusGameLoopHandler
    {
        public NexusInt currentWave;
        public NexusEventBlock OnWaveStart = new NexusEventBlock();
        public NexusEventBlock OnWaveEnd = new NexusEventBlock();
        public NexusEventBlock OnWaveStartChange = new NexusEventBlock();
        public NexusEventBlock AfterWaveFinishChange = new NexusEventBlock();
        public NexusEventReceiver NextWaveEventReceiver = new NexusEventReceiver();

        protected override void OnInitializeEntity()
        {
            base.OnInitializeEntity();

            if (currentWave == null)
            {
                currentWave = ScriptableObject.CreateInstance<NexusInt>();
                currentWave.Set(0);
            }


            NextWaveEventReceiver.AddListener(NextWave);
            currentWave.InitializeObject();
            OnWaveStart.InitializeObject();
            OnWaveEnd.InitializeObject();
            OnWaveStartChange.InitializeObject();
            AfterWaveFinishChange.InitializeObject();
            NextWaveEventReceiver.InitializeObject();
        }

        public void NextWave()
        {
            OnWaveEnd.InvokeBlock();
            currentWave.Increment();
            OnWaveStartChange.InvokeBlock();
            OnWaveStart.InvokeBlock();
            OnWaveProgress();
            AfterWaveFinishChange.InvokeBlock();
        }

        protected virtual void OnWaveProgress()
        {
            // Overridable method for wave progression logic
            // Implement custom wave functionality here
        }
    }
}