using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Toolkit.NexusEngine
{
    public class NexusTimer : NexusBlock
    {
        public NexusFloat tickRate;
        public NexusInt ticks;
        public NexusBool isRunning;

        public NexusEventReceiver disableTimer = new NexusEventReceiver();
        public NexusEventReceiver enableTimer = new NexusEventReceiver();
        public NexusEventReceiver forceTick = new NexusEventReceiver();
        public NexusEventBlock OnTick = new NexusEventBlock();


        protected override void OnInitializeBlock(NexusEntity entity)
        {

            if (tickRate == null)
            {
                tickRate = ScriptableObject.CreateInstance<NexusFloat>();
                tickRate.Set(1);
            }
            if (ticks == null)
            {
                ticks = ScriptableObject.CreateInstance<NexusInt>();
                ticks.Set(0);
            }
            if (isRunning == null)
            {
                isRunning = ScriptableObject.CreateInstance<NexusBool>();
                isRunning.Set(false);
            }

            disableTimer.InitializeObject();
            enableTimer.InitializeObject();
            forceTick.InitializeObject();
            OnTick.InitializeObject();


            disableTimer.AddListener(DisableTimer);
            enableTimer.AddListener(EnableTimer);
            forceTick.AddListener(Tick);
        }

        float lastTime = 0;
        private void Update()
        {
            if (isRunning.value == true)
            {
                if (Time.time - lastTime > tickRate.value)
                {
                    lastTime = Time.time;
                }
            }
        }

        void Tick()
        {
            ticks.Increment();
            OnTick.InvokeBlock();
        }

        public void DisableTimer()
        {
            isRunning.Set(false);
        }

        public void EnableTimer()
        {
            isRunning.Set(true);
        }
    }
}
