using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Toolkit
{
    public interface ITickable
    {
        int TickRate { get; }
        public void Tick()
        {
            OnTick();
        }

        void OnTick();



        public void Register()
        {

        }
    }


}
