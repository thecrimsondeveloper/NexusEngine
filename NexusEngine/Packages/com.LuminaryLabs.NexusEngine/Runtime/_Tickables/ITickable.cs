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
            Tickables.Register(this);
        }

        public void Unregister()
        {
            Tickables.Unregister(this);
        }
    }

    public interface IUpdateTickable
    {
        public void Tick()
        {
            OnTick();
        }

        void OnTick();

        public void RegisterUpdate()
        {
            Tickables.RegisterUpdateTickable(this);
        }

        public void UnregisterUpdate()
        {
            Tickables.UnregisterUpdateTickable(this);
        }
    }


}
