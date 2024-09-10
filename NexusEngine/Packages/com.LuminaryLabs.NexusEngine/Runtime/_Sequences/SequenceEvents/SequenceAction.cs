using System;
using System.Reflection;
using UnityEngine;


namespace LuminaryLabs.NexusEngine
{
    public delegate void SequenceAction();
    public delegate void SequenceAction<T0>(T0 arg0);
    public delegate void SequenceAction<T0, T1>(T0 arg0, T1 arg1);
    public delegate void SequenceAction<T0, T1, T2>(T0 arg0, T1 arg1, T2 arg2);
    public delegate void SequenceAction<T0, T1, T2, T3>(T0 arg0, T1 arg1, T2 arg2, T3 arg3);
    public delegate void SequenceAction<T0, T1, T2, T3, T4>(T0 arg0, T1 arg1, T2 arg2, T3 arg3, T4 arg4);
    public delegate void SequenceAction<T0, T1, T2, T3, T4, T5>(T0 arg0, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5);
}