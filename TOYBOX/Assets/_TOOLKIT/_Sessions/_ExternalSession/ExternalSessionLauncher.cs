using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Toolkit.Sessions;
using UnityEngine;

namespace Toolkit.Sessions
{
    public class ExternalSessionLauncher : MonoBehaviour
    {
        public ExternalSession session;

        [Button]
        public void EnableSession()
        {
            ExternalSession.EnableSession(session);
        }
    }
}
