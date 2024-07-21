using System;
using System.Collections.Generic;
using UnityEngine;

namespace Toolkit
{
    public class Tickables : MonoSingleton<Tickables>
    {
        private static List<TickLoop> tickLoops = new List<TickLoop>();
        private static List<IUpdateTickable> updateTickables = new List<IUpdateTickable>();

        public static void Register(ITickable tickable)
        {
            Instance.Internal_Register(tickable);
        }

        public static void Unregister(ITickable tickable)
        {
            Instance.Internal_Unregister(tickable);
        }

        public static void RegisterUpdateTickable(IUpdateTickable tickable)
        {
            Instance.Internal_RegisterUpdateTickable(tickable);
        }

        public static void UnregisterUpdateTickable(IUpdateTickable tickable)
        {
            Instance.Internal_UnregisterUpdateTickable(tickable);
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

            // If we get here, we need to create a new loop
            TickLoop tickLoop = new TickLoop
            {
                tickTime = tickable.TickRate
            };
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

        private void Internal_RegisterUpdateTickable(IUpdateTickable tickable)
        {
            if (!updateTickables.Contains(tickable))
            {
                updateTickables.Add(tickable);
            }
        }

        private void Internal_UnregisterUpdateTickable(IUpdateTickable tickable)
        {
            updateTickables.Remove(tickable);
        }

        private void Update()
        {
            float currentTime = Time.time;

            // Handle regular tickables
            for (int i = 0; i < tickLoops.Count; i++)
            {
                if (tickLoops[i].CanTick(currentTime))
                {
                    tickLoops[i].Tick();
                }
            }

            // Handle update tickables
            foreach (var tickable in updateTickables)
            {
                tickable.OnTick();
            }
        }

        [Serializable]
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

            public bool CanTick(float currentTime)
            {
                return currentTime - lastTickTime > tickTimeInSeconds;
            }
        }
    }
}
