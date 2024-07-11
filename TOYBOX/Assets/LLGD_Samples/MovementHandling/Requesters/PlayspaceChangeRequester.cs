using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Toolkit.Playspace
{
    public abstract class PlayspaceChangeRequester<T> : MonoBehaviour where T : IPlayspace
    {
        public bool requestOnStart = true;
        protected T playspace = default;

        protected virtual void Start()
        {
            // playspace = DependencyResolver.Resolve<T>();
            if (requestOnStart)
                SendRequest();
        }

        public void SendRequest()
        {
            OnSendRequest();
        }
        abstract protected void OnSendRequest();
    }
}
