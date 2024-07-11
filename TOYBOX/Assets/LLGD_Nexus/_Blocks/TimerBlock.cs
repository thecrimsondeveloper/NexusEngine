using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace Toolkit.NexusEngine
{
    public class TimerBlock : NexusBlock
    {
        [SerializeField] private float time = 1;
        [SerializeField] UnityEvent OnTick = new UnityEvent();

        float timeAtLastTick = 0;
        private void Update()
        {
            if (Time.time - timeAtLastTick > time)
            {
                OnTick.Invoke();
                timeAtLastTick = Time.time;
            }
        }
    }
}
