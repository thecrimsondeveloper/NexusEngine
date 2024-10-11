using System;
using System.Collections.Generic;
using System.Linq;

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

#if UNITY_EDITOR && ODIN_INSPECTOR 
        [Serializable] 
        class EventInfo
        { 
            public string eventType; 
            public int listenerCount;
            public EventInfo(string name, int listenerCount)
            {
                this.eventType =name;
                this.listenerCount = listenerCount;
            }
        }

        // bool isUnityObject => sequence is UnityEngine.Object;
        // [ShowInInspector,ShowIf(nameof(sequenceReference))]
        // UnityEngine.Object sequenceReference => isUnityObject ? sequence as UnityEngine.Object : null;

        [ShowInInspector]
        List<EventInfo> eventInfos => events.Select(e => new EventInfo(e.Key.ToString(), e.Value.GetPersistentEventCount())).ToList();
#endif

        public Dictionary<SequenceEventType, UnityEvent<ISequence>> events = new Dictionary<SequenceEventType, UnityEvent<ISequence>>();
        //  = new Dictionary<SequenceEventType, UnityEvent<ISequence>>
        // {
        //     {SequenceEventType.OnInitialize, new UnityEvent<ISequence>()},
        //     {SequenceEventType.OnBegin, new UnityEvent<ISequence>()},
        //     {SequenceEventType.OnFinished, new UnityEvent<ISequence>()},
        //     {SequenceEventType.OnUnloaded, new UnityEvent<ISequence>()}
        // };

        public void RegisterEvent(UnityAction<ISequence> action, SequenceEventType eventType)
        {
            Debug.Log("Registering Event: " + action.Method + " for " + eventType);
            if (events.TryGetValue(eventType, out UnityEvent<ISequence> unityEvent))
            {
                Debug.Log("Event: " + unityEvent == null);
                unityEvent.AddListener(action);
                Debug.Log("Actions: " + action.Method);
                Debug.Log("New Listener Count: " + unityEvent.GetPersistentMethodName(0));
            }
            else
            {
                UnityEvent<ISequence> newEvent = new UnityEvent<ISequence>();
                newEvent.AddListener(action);
                events.Add(eventType, newEvent);
                Debug.Log("Unable to Find Event: " + eventType + " in the event Dictionary, creating a new event with " + newEvent.GetPersistentEventCount());
            }



        }

        public void UnRegisterEvent(UnityAction<ISequence> action, SequenceEventType eventType)
        {
            if (events.TryGetValue(eventType, out var unityEvent))
            {
                unityEvent.RemoveListener(action);
            }
        }

        public void InvokeEvent(SequenceEventType eventType, ISequence sequence)
        {
            if (events.TryGetValue(eventType, out var unityEvent))
            {
                Debug.Log("Invoking Event: " + eventType + " on " + sequence.GetType() + " with " + unityEvent.GetPersistentEventCount() + " listeners");
                unityEvent.Invoke(sequence);
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
