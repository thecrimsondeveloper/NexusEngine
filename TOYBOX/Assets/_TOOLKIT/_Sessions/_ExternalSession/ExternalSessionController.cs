using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Toolkit.Sessions;
using UnityEngine;

namespace Toolkit.NexusEngine
{
    public class ExternalSessionController : NexusBlock
    {
        public ExternalSession session;
        public NexusEventBlock OnEnableSession = new NexusEventBlock();

        protected override void OnInitializeBlock(NexusEntity parentEntity)
        {
            base.OnInitializeBlock(parentEntity);
            OnEnableSession.AddListener(EnableSession);
        }

        [Button]
        public void EnableSession()
        {
            ExternalSession.EnableSession(session);
        }
    }
}
