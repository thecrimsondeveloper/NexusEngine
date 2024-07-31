using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace LuminaryLabs.Sequences
{
    public class SequenceEvents
    {
        [ShowInInspector]
        public Dictionary<SequenceEventType, UnityEvent> events = new Dictionary<SequenceEventType, UnityEvent>
        {
            {SequenceEventType.OnInitialize, new UnityEvent()},
            {SequenceEventType.OnBegin, new UnityEvent()},
            {SequenceEventType.OnFinished, new UnityEvent()},
            {SequenceEventType.OnUnloaded, new UnityEvent()}
        };
        public void RegisterEvent(UnityAction action, SequenceEventType eventType)
        {
            if (events.TryGetValue(eventType, out var unityEvent))
                unityEvent.AddListener(action);
            else
            {
                UnityEvent newEvent = new UnityEvent();
                newEvent.AddListener(action);
                events.Add(eventType, newEvent);
            }
        }

        public void UnRegisterEvent(UnityAction action, SequenceEventType eventType)
        {
            if (events.TryGetValue(eventType, out var unityEvent))
            {
                unityEvent.RemoveListener(action);
            }
        }

        public void InvokeEvent(SequenceEventType eventType)
        {
            if (events.TryGetValue(eventType, out var unityEvent))
            {
                unityEvent.Invoke();
            }
        }
    }

    public enum SequenceEventType
    {
        OnInitialize,
        OnBegin,
        OnFinished,
        OnUnloaded
    }
}
