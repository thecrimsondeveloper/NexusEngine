using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using ToyBox;
using UnityEngine;
using UnityEngine.Events;

namespace Toolkit.NexusEngine
{
    [System.Serializable]
    public class NexusEventBlock : NexusPrimitive<UnityEvent>, INexusConnector
    {
        public NexusList<NexusEventReceiver> receivers = new NexusList<NexusEventReceiver>();

        public override UnityEvent value { get; protected set; } = new UnityEvent();

        [Button("Invoke Block")]
        public void InvokeBlock()
        {
            OnInvokeBlock();
            InvokeReceivers();
        }

        protected virtual void OnInvokeBlock() { }

        public void AddListener(UnityAction action)
        {
            value.AddListener(action);
        }

        public void RemoveListener(UnityAction action)
        {
            value.RemoveListener(action);
        }

        public void OnResolveConnection(NexusConnectionHandler connectionHandler)
        {
            //add receiver to list if the connection is a receiver
            if (Nexus.TryGetConnectable(connectionHandler.targetGuid, out NexusEventReceiver receiver))
            {
                receivers.Add(receiver);
            }
        }

        protected override void OnInitializeObject()
        {
            receivers.InitializeObject();
        }

        private void InvokeReceivers()
        {
            foreach (NexusEventReceiver receiver in receivers.value)
            {
                receiver.InvokeReceiver();
            }
        }
    }
}