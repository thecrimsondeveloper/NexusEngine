using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Toolkit.NexusEngine
{
    public interface INexusTrackable
    {
        Guid guid { get; set; }
        public Guid GetGuid()
        {
            return guid;
        }

        public void SetGuid(Guid guid)
        {
            this.guid = guid;
        }

        public void InitializeTrackable()
        {
            Nexus.RegisterTrackable(this);
        }

    }
    public interface INexusConnectable : INexusTrackable
    {
    }

    public interface INexusConnector : INexusTrackable
    {
        public void ResolveConnection(NexusConnectionHandler connectionHandler)
        {
            OnResolveConnection(connectionHandler);
        }

        public void ResolveConnections(NexusMultiConnectionHandler connectionHandler)
        {
            foreach (Guid guid in connectionHandler.targetGuids)
            {
                NexusConnectionHandler connection = new NexusConnectionHandler(guid);
                OnResolveConnection(connection);
            }
        }


        void OnResolveConnection(NexusConnectionHandler connections);


    }
}
