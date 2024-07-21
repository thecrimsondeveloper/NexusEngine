using System;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System.Diagnostics;
using Debug = UnityEngine.Debug;
using System.Linq;

public class DictionaryProfiler : MonoBehaviour
{
    private Dictionary<Guid, string> dictionary = new Dictionary<Guid, string>();
    private Guid testGuid;

    [SerializeField] int amount = 1000000;
    [SerializeField] int amountToExecute = 1000;


    void Start()
    {
    }

    [Button("Run Profile Test")]
    public void RunProfileTest()
    {
        // Initialize the dictionary and add entries
        dictionary = new Dictionary<Guid, string>();
        var random = new System.Random();
        for (int i = 0; i < amount; i++)
        {
            dictionary.Add(Guid.NewGuid(), "Value" + i);
        }

        // Add a test entry
        // Get a test GUID randomly from the dictionary
        int index = random.Next(0, dictionary.Count);
        testGuid = dictionary.Keys.ToArray()[index];

        // Warm up
        var value = dictionary[testGuid];
        var stopwatch = new Stopwatch();


        // Guid[] randomGuids = new Guid[amountToExecute];
        // for (int i = 0; i < amountToExecute; i++)
        // {
        //     index = random.Next(0, dictionary.Count);
        //     randomGuids[i] = dictionary.Keys.ToArray()[index];
        // }


        stopwatch.Start();

        // // Measure lookup time
        // for (int i = 0; i < lookupCount; i++)
        // {
        //     Guid key = randomGuids[i];
        //     if (dictionary.ContainsKey(key))
        //     {
        //         value = dictionary[key];
        //     }
        // }

        //add new guids
        for (int i = 0; i < amountToExecute; i++)
        {
            dictionary.Add(Guid.NewGuid(), "Value" + i);
        }


        stopwatch.Stop();

        double averageExecutionTime = stopwatch.Elapsed.TotalMilliseconds / amountToExecute;
        Debug.Log($"Average Execution time: {averageExecutionTime} ms");
    }
}
