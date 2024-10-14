using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

public interface ISequence
{
    ISequence superSequence { get; set; }
    Guid guid { get; set; }
    object currentData { get; set; }

    UniTask InitializeSequence(object currentData = null);
    void OnBeginSequence();
    UniTask FinishSequence();
    UniTask UnloadSequence();

    void OnFinishSequence();
    void OnUnloadSequence();

    public Transform GetTransform()
    {
        return this is MonoBehaviour monoBehaviour ? monoBehaviour.transform : superSequence?.GetTransform();
    }
}