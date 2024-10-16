using System;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif
using UnityEngine;
using UnityEngine.Events;

namespace LuminaryLabs.NexusEngine
{
    public class SequenceEvents
    {
        public ISequence sequence;
        
        public SequenceEvents(ISequence sequence)
        {
            this.sequence = sequence;
        }

        // UnityEvents for each event type
        public UnityEvent<ISequence> OnInitializeEvent = new UnityEvent<ISequence>();
        public UnityEvent<ISequence> OnBeginEvent = new UnityEvent<ISequence>();
        public UnityEvent<ISequence> OnFinishedEvent = new UnityEvent<ISequence>();
        public UnityEvent<ISequence> OnUnloadEvent = new UnityEvent<ISequence>();
        public UnityEvent<ISequence> OnUnloadedEvent = new UnityEvent<ISequence>();

        // Register listeners using a switch case
        public void RegisterEvent(UnityAction<ISequence> action, SequenceEventType eventType)
        {
            switch (eventType)
            {
                case SequenceEventType.OnInitialize:
                    OnInitializeEvent.AddListener(action);
                    Nexus.Log("Listener added to OnInitializeEvent");
                    break;
                case SequenceEventType.OnBegin:
                    OnBeginEvent.AddListener(action);
                    Nexus.Log("Listener added to OnBeginEvent");
                    break;
                case SequenceEventType.OnFinished:
                    OnFinishedEvent.AddListener(action);
                    Nexus.Log("Listener added to OnFinishedEvent");
                    break;
                case SequenceEventType.OnUnload:
                    OnUnloadEvent.AddListener(action);
                    Nexus.Log("Listener added to OnUnloadEvent");
                    break;
                case SequenceEventType.OnUnloaded:
                    OnUnloadedEvent.AddListener(action);
                    Nexus.Log("Listener added to OnUnloadedEvent");
                    break;
                default:
                    Nexus.LogError("Unknown event type: " + eventType);
                    break;
            }
        }

        // Unregister listeners using a switch case
        public void UnRegisterEvent(UnityAction<ISequence> action, SequenceEventType eventType)
        {
            switch (eventType)
            {
                case SequenceEventType.OnInitialize:
                    OnInitializeEvent.RemoveListener(action);
                    Nexus.Log("Listener removed from OnInitializeEvent");
                    break;
                case SequenceEventType.OnBegin:
                    OnBeginEvent.RemoveListener(action);
                    Nexus.Log("Listener removed from OnBeginEvent");
                    break;
                case SequenceEventType.OnFinished:
                    OnFinishedEvent.RemoveListener(action);
                    Nexus.Log("Listener removed from OnFinishedEvent");
                    break;
                case SequenceEventType.OnUnload:
                    OnUnloadEvent.RemoveListener(action);
                    Nexus.Log("Listener removed from OnUnloadEvent");
                    break;
                case SequenceEventType.OnUnloaded:
                    OnUnloadedEvent.RemoveListener(action);
                    Nexus.Log("Listener removed from OnUnloadedEvent");
                    break;
                default:
                    Nexus.LogError("Unknown event type: " + eventType);
                    break;
            }
        }

        // Invoke events using a switch case
        public void InvokeEvent(SequenceEventType eventType, ISequence sequence)
        {
            Nexus.Log("Invoking Event: " + eventType + " on " + sequence.GetType());
            switch (eventType)
            {
                case SequenceEventType.OnInitialize:
                    OnInitializeEvent.Invoke(sequence);
                    break;
                case SequenceEventType.OnBegin:
                    OnBeginEvent.Invoke(sequence);
                    break;
                case SequenceEventType.OnFinished:
                    OnFinishedEvent.Invoke(sequence);
                    break;
                case SequenceEventType.OnUnload:
                    OnUnloadEvent.Invoke(sequence);
                    break;
                case SequenceEventType.OnUnloaded:
                    OnUnloadedEvent.Invoke(sequence);
                    break;
                default:
                    Debug.LogError("Unknown event type: " + eventType);
                    break;
            }
        }
    }

    public enum SequenceEventType
    {
        OnInitialize,
        OnBegin,
        OnFinished,
        OnUnload,
        OnUnloaded
    }
}
