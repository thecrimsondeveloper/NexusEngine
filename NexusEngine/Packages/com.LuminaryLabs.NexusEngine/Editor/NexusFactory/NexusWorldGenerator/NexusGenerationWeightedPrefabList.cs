using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class NexusGenerationWeightedPrefabList : NexusFactorySequence
{
    public List<GridGenerationDefintion> gridDefinitions = new List<GridGenerationDefintion>();

    public NexusGenerationWeightedPrefabList(bool showName = true) : base(showName)
    {

    }

    protected override void OnDraw()
    {
        ///draw the grid settings
        foreach (var gridDefinition in gridDefinitions)
        {
            gridDefinition.Draw();
        }

        //horizontal layout for the buttons
        GUILayout.BeginHorizontal();
        //add space
        GUILayout.FlexibleSpace();
        if (gridDefinitions.Count >= 0)
        {
            //add a button to remove the last grid definition
            if (GUILayout.Button("-"))
            {
                gridDefinitions.RemoveAt(gridDefinitions.Count - 1);
            }
        }

        //add a button to add a new grid definition
        if (GUILayout.Button("+"))
        {
            gridDefinitions.Add(new GridGenerationDefintion(false));
        }

        GUILayout.EndHorizontal();
    }

    public GameObject GetRandomPrefab()
    {
        float totalWeight = 0f;
        foreach (var definition in gridDefinitions)
        {
            totalWeight += definition.weight;
        }

        float randomWeight = Random.Range(0f, totalWeight);

        foreach (var definition in gridDefinitions)
        {
            randomWeight -= definition.weight;
            if (randomWeight <= 0)
            {
                return definition.prefab;
            }
        }

        return null;
    }
}

public class GridGenerationDefintion : NexusFactorySequence
{
    public GameObject prefab;
    public float weight;

    public GridGenerationDefintion(bool showName) : base(showName)
    {

    }

    protected override void OnDraw()
    {
        GUILayout.BeginHorizontal();

        //begin vertical layout for the prefab and weight
        GUILayout.BeginVertical();
        NexusFactory.BeginContentArea();
        GUILayout.Label("Prefab");
        prefab = (GameObject)EditorGUILayout.ObjectField(prefab, typeof(GameObject), false);
        NexusFactory.EndContentArea();
        GUILayout.EndVertical();

        GUILayout.BeginVertical();
        NexusFactory.BeginContentArea();
        GUILayout.Label("Weight");
        weight = EditorGUILayout.Slider(weight, 0f, 1f);
        NexusFactory.EndContentArea();
        GUILayout.EndVertical();



        GUILayout.EndHorizontal();
    }
}
