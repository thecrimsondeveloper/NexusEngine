using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Toolkit.NexusEngine;
using UnityEngine;
using UnityEngine.Events;

namespace Toolkit
{
    public class Tickables : MonoSingleton<Tickables>
    {



        static List<TickLoop> tickLoops = new List<TickLoop>();



        public static void Register(ITickable tickable)
        {
            Instance.Internal_Register(tickable);
        }

        public static void Unregister(ITickable tickable)
        {
            Instance.Internal_Unregister(tickable);
        }


        private void Internal_Register(ITickable tickable)
        {

            foreach (var loop in tickLoops)
            {
                if (loop.tickTime == tickable.TickRate)
                {
                    loop.tickables.Add(tickable);
                    return;
                }
            }

            //if we get here, we need to create a new loop
            TickLoop tickLoop = new TickLoop();
            tickLoop.tickTime = tickable.TickRate;
            tickLoop.tickables.Add(tickable);
            tickLoops.Add(tickLoop);
        }

        private void Internal_Unregister(ITickable tickable)
        {
            for (int i = 0; i < tickLoops.Count; i++)
            {
                if (tickLoops[i].tickables.Contains(tickable))
                {
                    tickLoops[i].tickables.Remove(tickable);
                    if (tickLoops[i].tickables.Count == 0)
                    {
                        tickLoops.RemoveAt(i);
                    }
                    return;
                }
            }
        }

        private void Update()
        {
            for (int i = 0; i < tickLoops.Count; i++)
            {
                if (tickLoops[i].CanTick())
                {
                    tickLoops[i].Tick();

                }
            }
        }


        [System.Serializable]
        public class TickLoop
        {
            public int tickTime;
            public float lastTickTime;
            public float tickTimeInSeconds => tickTime / 1000f;
            public List<ITickable> tickables = new List<ITickable>();
            public void Tick()
            {
                for (int i = 0; i < tickables.Count; i++)
                {
                    if (tickables[i] == null)
                    {
                        tickables.RemoveAt(i);
                        i--;
                        continue;
                    }
                    tickables[i].OnTick();
                }

                lastTickTime = Time.time;
            }

            public bool CanTick()
            {
                return Time.time - lastTickTime > tickTimeInSeconds;
            }

        }
    }
}

