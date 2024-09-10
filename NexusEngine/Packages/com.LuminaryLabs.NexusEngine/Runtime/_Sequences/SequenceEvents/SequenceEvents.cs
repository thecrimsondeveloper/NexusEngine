using System.Collections.Generic;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif
using UnityEngine;
using UnityEngine.Events;

namespace LuminaryLabs.NexusEngine
{
    public class SequenceEvents
    {
#if ODIN_INSPECTOR
        [ShowInInspector]
#endif
        public Dictionary<SequenceEventType, UnityEvent<ISequence>> events = new Dictionary<SequenceEventType, UnityEvent<ISequence>>
        {
            {SequenceEventType.OnInitialize, new UnityEvent<ISequence>()},
            {SequenceEventType.OnBegin, new UnityEvent<ISequence>()},
            {SequenceEventType.OnFinished, new UnityEvent<ISequence>()},
            {SequenceEventType.OnUnloaded, new UnityEvent<ISequence>()}
        };

        public void RegisterEvent(UnityAction<ISequence> action, SequenceEventType eventType)
        {
            if (events.TryGetValue(eventType, out var unityEvent))
                unityEvent.AddListener(action);
            else
            {
                UnityEvent<ISequence> newEvent = new UnityEvent<ISequence>();
                newEvent.AddListener(action);
                events.Add(eventType, newEvent);
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
