using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Toolkit.NexusEngine
{
    [System.Serializable]
    public struct NexusConnectionHandler
    {
        public Guid targetGuid;



        public NexusConnectionHandler(Guid targetGuids)
        {
            this.targetGuid = targetGuids;
        }


    }

    public struct NexusMultiConnectionHandler
    {
        public List<Guid> targetGuids;

        public NexusMultiConnectionHandler(List<Guid> targetGuids)
        {
            this.targetGuids = targetGuids;
        }
    }
}

