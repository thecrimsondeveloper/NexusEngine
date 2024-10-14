using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace LuminaryLabs.NexusEngine
{
    public class SequenceEvent
    {
        // private readonly List<(WeakReference Target, Delegate Method)> _listeners = new List<(WeakReference, Delegate)>();

        // public void AddListener(SequenceAction action)
        // {
        //     _listeners.Add((new WeakReference(action.Target), action));
        // }

        // public void AddListener<T0>(SequenceAction<T0> action)
        // {
        //     _listeners.Add((new WeakReference(action.Target), action));
        // }

        // public void AddListener<T0, T1>(SequenceAction<T0, T1> action)
        // {
        //     _listeners.Add((new WeakReference(action.Target), action));
        // }

        // public void AddListener<T0, T1, T2>(SequenceAction<T0, T1, T2> action)
        // {
        //     _listeners.Add((new WeakReference(action.Target), action));
        // }

        // public void AddListener<T0, T1, T2, T3>(SequenceAction<T0, T1, T2, T3> action)
        // {
        //     _listeners.Add((new WeakReference(action.Target), action));
        // }

        // public void AddListener<T0, T1, T2, T3, T4>(SequenceAction<T0, T1, T2, T3, T4> action)
        // {
        //     _listeners.Add((new WeakReference(action.Target), action));
        // }

        // public void AddListener<T0, T1, T2, T3, T4, T5>(SequenceAction<T0, T1, T2, T3, T4, T5> action)
        // {
        //     _listeners.Add((new WeakReference(action.Target), action));
        // }

        // public void RemoveListener(SequenceAction action)
        // {
        //     _listeners.RemoveAll(l => l.Target.IsAlive && l.Method == action && l.Target.Target == action.Target);
        // }

        // public void RemoveListener<T0>(SequenceAction<T0> action)
        // {
        //     _listeners.RemoveAll(l => l.Target.IsAlive && l.Method == action && l.Target.Target == action.Target);
        // }

        // public void RemoveListener<T0, T1>(SequenceAction<T0, T1> action)
        // {
        //     _listeners.RemoveAll(l => l.Target.IsAlive && l.Method == action && l.Target.Target == action.Target);
        // }

        // public void RemoveListener<T0, T1, T2>(SequenceAction<T0, T1, T2> action)
        // {
        //     _listeners.RemoveAll(l => l.Target.IsAlive && l.Method == action && l.Target.Target == action.Target);
        // }

        // public void RemoveListener<T0, T1, T2, T3>(SequenceAction<T0, T1, T2, T3> action)
        // {
        //     _listeners.RemoveAll(l => l.Target.IsAlive && l.Method == action && l.Target.Target == action.Target);
        // }

        // public void RemoveListener<T0, T1, T2, T3, T4>(SequenceAction<T0, T1, T2, T3, T4> action)
        // {
        //     _listeners.RemoveAll(l => l.Target.IsAlive && l.Method == action && l.Target.Target == action.Target);
        // }

        // public void RemoveListener<T0, T1, T2, T3, T4, T5>(SequenceAction<T0, T1, T2, T3, T4, T5> action)
        // {
        //     _listeners.RemoveAll(l => l.Target.IsAlive && l.Method == action && l.Target.Target == action.Target);
        // }

        // public void Invoke()
        // {
        //     InvokeInternal();
        // }

        // public void Invoke<T0>(T0 arg0)
        // {
        //     InvokeInternal(arg0);
        // }

        // public void Invoke<T0, T1>(T0 arg0, T1 arg1)
        // {
        //     InvokeInternal(arg0, arg1);
        // }

        // public void Invoke<T0, T1, T2>(T0 arg0, T1 arg1, T2 arg2)
        // {
        //     InvokeInternal(arg0, arg1, arg2);
        // }

        // public void Invoke<T0, T1, T2, T3>(T0 arg0, T1 arg1, T2 arg2, T3 arg3)
        // {
        //     InvokeInternal(arg0, arg1, arg2, arg3);
        // }

        // public void Invoke<T0, T1, T2, T3, T4>(T0 arg0, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
        // {
        //     InvokeInternal(arg0, arg1, arg2, arg3, arg4);
        // }

        // public void Invoke<T0, T1, T2, T3, T4, T5>(T0 arg0, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
        // {
        //     InvokeInternal(arg0, arg1, arg2, arg3, arg4, arg5);
        // }

        // private void InvokeInternal(params object[] args)
        // {
        //     for (int i = _listeners.Count - 1; i >= 0; i--)
        //     {
        //         var listener = _listeners[i];
        //         if (listener.Target.IsAlive)
        //         {
        //             try
        //             {
        //                 listener.Method.DynamicInvoke(args);
        //             }
        //             catch (TargetInvocationException ex)
        //             {
        //                 Debug.LogError($"Error invoking method: {ex.InnerException?.Message}");
        //             }
        //         }
        //         else
        //         {
        //             _listeners.RemoveAt(i);
        //         }
        //     }
        // }
    }
}
