using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.Reflection;
using Cysharp.Threading.Tasks;

public class NexusWorldGenerator : NexusFactoryPane
{
    public List<NexusWorldGenerationHandler> worldGenerationHandlers = new List<NexusWorldGenerationHandler>();

    public NexusWorldGenerator()
    {
        title = "World Generator";
        worldGenerationHandlers.Add(new NexusTerrainGenerationHandler());
        worldGenerationHandlers.Add(new NexusSphereGenerationHandler());
        worldGenerationHandlers.Add(new NexusGridGenerationHandler());

        currentHandler = worldGenerationHandlers[0];
    }


    NexusWorldGenerationHandler currentHandler;
    protected override void WhenDraw()
    {


        //horizontal layout for the buttons
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Previous")) { ChangeHandler(false); }
        if (GUILayout.Button("Next")) { ChangeHandler(true); }
        GUILayout.EndHorizontal();


        DrawGenerationHandler(currentHandler);
    }

    void ChangeHandler(bool add)
    {
        int index = worldGenerationHandlers.IndexOf(currentHandler);
        if (add)
        {
            index++;
            if (index >= worldGenerationHandlers.Count)
            {
                index = 0;
            }
        }
        else
        {
            index--;
            if (index < 0)
            {
                index = worldGenerationHandlers.Count - 1;
            }
        }
        currentHandler = worldGenerationHandlers[index];
    }

    void DrawGenerationHandler(NexusWorldGenerationHandler handler)
    {
        NexusFactory.BeginContentArea();
        handler.Draw();
        NexusFactory.EndContentArea();

        if (GUILayout.Button("Generate World"))
        {
            handler.GenerateWorld();
        }
    }
}

public abstract class NexusWorldGenerationHandler : NexusFactorySequence
{
    public Transform spawnParent;
    protected NexusWorldGenerationHandler(bool showName) : base(showName)
    {

    }

    public override void Draw()
    {
        DrawName();

        spawnParent = (Transform)EditorGUILayout.ObjectField("Spawn Parent", spawnParent, typeof(Transform), true);

        if (spawnParent == null)
        {
            EditorGUILayout.HelpBox("Please assign a parent transform to spawn the objects under.", MessageType.Warning);
            return;
        }

        OnDraw();

    }

    protected async UniTask ClearParent()
    {
        //clear the parent of any existing children
        //use a while loop to ensure all children are destroyed
        int awaitInXChilren = 100;

        while (spawnParent.childCount > 0)
        {
            GameObject.DestroyImmediate(spawnParent.GetChild(0).gameObject);

            if (awaitInXChilren-- <= 0)
            {
                await UniTask.DelayFrame(1);
                awaitInXChilren = 100;
            }
        }
    }

    public abstract void GenerateWorld();
}









