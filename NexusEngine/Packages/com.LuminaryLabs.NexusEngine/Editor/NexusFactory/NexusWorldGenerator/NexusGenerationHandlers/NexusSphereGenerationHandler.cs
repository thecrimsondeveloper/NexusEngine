using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class NexusSphereGenerationHandler : NexusWorldGenerationHandler
{
    public float radius = 10;
    public int spawnCount = 10;
    private NexusGenerationWeightedPrefabList spawnDefinitions = new NexusGenerationWeightedPrefabList(false);


    public NexusSphereGenerationHandler(bool showName = true) : base(showName)
    {
        name = "Sphere Settings";
    }

    protected override void OnDraw()
    {
        if (spawnDefinitions != null)
        {
            spawnDefinitions.Draw();
        }

        if (spawnDefinitions.gridDefinitions.Count == 0)
        {
            return;
        }

        spawnCount = EditorGUILayout.IntField("Spawn Count", spawnCount);
        radius = EditorGUILayout.FloatField("Radius", radius);
    }

    public override async void GenerateWorld()
    {
        await ClearParent();
        Debug.Log("Generating Sphere World with radius: " + radius + " and spawn count: " + spawnCount);

        // Calculate total weight of all prefabs
        float totalWeight = 0f;
        foreach (var definition in spawnDefinitions.gridDefinitions)
        {
            totalWeight += definition.weight;
        }

        // Loop through the number of objects to spawn
        for (int i = 0; i < spawnCount; i++)
        {
            Debug.Log("Spawning object: " + i);
            // Randomly choose a prefab based on weight
            GameObject prefabToSpawn = ChoosePrefabBasedOnWeight(totalWeight);

            if (prefabToSpawn != null)
            {
                // Generate random spherical coordinates
                Vector3 randomPosition = Random.insideUnitSphere * radius;

                // Instantiate the prefab at the calculated position
                GameObject spawnedObject = (GameObject)PrefabUtility.InstantiatePrefab(prefabToSpawn);

                if (spawnedObject != null)
                {
                    spawnedObject.transform.position = randomPosition;
                    spawnedObject.transform.SetParent(spawnParent);
                    spawnedObject.name = prefabToSpawn.name + "_" + i;
                }

                spawnedObject.name = prefabToSpawn.name + "_" + i;
            }
        }
    }

    private GameObject ChoosePrefabBasedOnWeight(float totalWeight)
    {
        float randomWeight = UnityEngine.Random.Range(0f, totalWeight);
        float cumulativeWeight = 0f;

        foreach (var definition in spawnDefinitions.gridDefinitions)
        {
            cumulativeWeight += definition.weight;
            if (randomWeight <= cumulativeWeight)
            {
                return definition.prefab;
            }
        }

        return null;  // In case no prefab is selected
    }

}
