using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEditor;
using UnityEngine;

public class NexusGridGenerationHandler : NexusWorldGenerationHandler
{
    private NexusGenerationWeightedPrefabList spawnDefinitions = new NexusGenerationWeightedPrefabList(false);
    public Vector3Int spawnGrid = new Vector3Int(10, 10, 10);
    public float cellSize = 1;


    public NexusGridGenerationHandler(bool showName = true) : base(showName)
    {
        name = "Grid Generation";
    }

    protected override void OnDraw()
    {

        if (spawnDefinitions != null)
        {
            spawnDefinitions.Draw();
        }

        spawnGrid = EditorGUILayout.Vector3IntField("Spawn Grid", spawnGrid);
        cellSize = EditorGUILayout.FloatField("Cell Size", cellSize);
    }

    public override async void GenerateWorld()
    {

        //clear the parent of any existing children
        //use a while loop to ensure all children are destroyed
        await ClearParent();


        List<List<List<GameObject>>> grid = new List<List<List<GameObject>>>();

        Vector3 halfOffset = new Vector3(spawnGrid.x * cellSize / 2, spawnGrid.y * cellSize / 2, spawnGrid.z * cellSize / 2);
        // Nested loop to go through each cell in the 3D grid
        for (int x = 0; x < spawnGrid.x; x++)
        {
            List<List<GameObject>> yList = new List<List<GameObject>>();
            for (int y = 0; y < spawnGrid.y; y++)
            {
                List<GameObject> zList = new List<GameObject>();
                for (int z = 0; z < spawnGrid.z; z++)
                {
                    // Randomly choose a prefab based on weight
                    GameObject prefabToSpawn = spawnDefinitions.GetRandomPrefab();

                    if (prefabToSpawn != null)
                    {
                        // Instantiate the prefab at the correct grid position
                        Vector3 position = new Vector3(x * cellSize, y * cellSize, z * cellSize);

                        GameObject spawnedObject = (GameObject)PrefabUtility.InstantiatePrefab(prefabToSpawn);
                        spawnedObject.transform.SetParent(spawnParent);
                        spawnedObject.transform.localPosition = position - halfOffset;
                        spawnedObject.transform.localRotation = Quaternion.identity;

                        zList.Add(spawnedObject);
                    }
                    else
                    {
                        zList.Add(null);  // If no prefab, leave it empty
                    }
                }
                yList.Add(zList);
            }
            grid.Add(yList);
        }
    }





}
