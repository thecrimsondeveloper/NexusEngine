using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Toolkit.NexusEngine;
using UnityEngine;

namespace ToyBox
{
    public class NexusConnectionController : MonoBehaviour
    {


        [Button]
        void ConnectNexusEvent(string blockGuid, string receiverGuid)
        {
            ConnectNexusEvents(new Guid(blockGuid), new Guid(receiverGuid));
        }


        void ConnectNexusEvents(Guid blockGuid, Guid receiverGuid)
        {
            // Debug.LogError("Connecting Nexus Events is current broken");
            // NexusConnection connection = new NexusConnectionHandler(receiverGuid);
            // if (Nexus.TryGetObject(blockGuid.ToString(), out NexusObject block))
            // {
            //     if (block is INexusConnector blockConnector)
            //     {
            //         blockConnector.ResolveConnections(connection);
            //     }
            // }

            // if (block is INexusConnector connector)
            // {
            //     connector.ResolveConnections(connection);
            // }
        }


    }
}
