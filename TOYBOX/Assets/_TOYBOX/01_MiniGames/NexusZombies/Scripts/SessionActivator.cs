using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Toolkit.Sessions;
using UnityEngine;

namespace ToyBox
{
    public class SessionActivator : MonoBehaviour
    {
        [SerializeField] bool activateOnStart = false;
        [SerializeField] Session session;

        private void Start()
        {
            if (activateOnStart)
            {
                Activate();
            }
        }

        [Button]
        public void Activate()
        {
            Session.Load(session);
        }




    }
}
