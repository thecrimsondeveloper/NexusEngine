using System;
using System.Collections.Generic;
using UnityEngine;

namespace LuminaryLabs.NexusEngine
{
    [CreateAssetMenu(fileName = "SequenceLibrary", menuName = "Toolkit/Sequences/Sequence Library")]
    public class SequenceLibrary : ScriptableObject
    {
        public SerializableDictionary<string, UnityEngine.Object> resources = new SerializableDictionary<string, UnityEngine.Object>();

        private void OnValidate()
        {
            List<string> keys = new List<string>(resources.Keys);

            foreach (string key in keys)
            {
                UnityEngine.Object resource = resources[key];

                // Check if resource is a GameObject
                if (resource is GameObject obj)
                {
                    MonoBehaviour[] monos = obj.GetComponents<MonoBehaviour>();
                    bool foundSequence = false;

                    for (int j = 0; j < monos.Length; j++)
                    {
                        Debug.Log(monos[j].GetType());
                        if (monos[j] is ISequence)
                        {
                            Debug.Log("Sequence found");
                            resources[key] = monos[j];
                            foundSequence = true;
                            break;
                        }
                    }

                    if (!foundSequence)
                    {
                        Debug.LogWarning("SequenceController requires a MonoBehaviour that implements ISequence.");
                        resources[key] = null;
                    }
                }
                // Check if resource is directly an ISequence
                else if (resource is ISequence)
                {
                    continue;
                }
                else
                {
                    Debug.LogWarning("Sequence Library requires a MonoBehaviour or ScriptableObject that implements ISequence.");
                    resources[key] = null;
                }
            }
        }

        public UnityEngine.Object Get(string key)
        {
            if (resources.TryGetValue(key, out var resource))
            {
                return resource;
            }
            else
            {
                return null;
            }
        }

        public bool TryGet(string key, out UnityEngine.Object resource)
        {
            return resources.TryGetValue(key, out resource);
        }
    }
}
