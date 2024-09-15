using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public abstract class NexusFactorySequence
{
    bool showName = true;

    public NexusFactorySequence()
    {
    }

    public NexusFactorySequence(bool showName)
    {
        this.showName = showName;
    }


    public string name = "Factory Sequence";
    public virtual void Draw()
    {
        if (showName)
        {
            DrawName();
        }

        OnDraw();
    }

    protected virtual void DrawName()
    {
        GUILayout.Label(name, EditorStyles.boldLabel);
    }

    protected abstract void OnDraw();
}
