using System;
using Toolkit.Sessions;
using UnityEngine;

public abstract class UIVisualizer : MonoBehaviour
{
    public void RefreshUI(SessionData sessionData)
    {
        OnRefresh(sessionData);
    }

    protected abstract void OnRefresh(SessionData sessionData);
    public abstract void SetUIState(UISettings settings);


}

public struct UISettings : IDisposable
{
    public string title;
    public float score;


    public void Dispose()
    {

    }

}
