using System.Collections;
using System.Collections.Generic;

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
