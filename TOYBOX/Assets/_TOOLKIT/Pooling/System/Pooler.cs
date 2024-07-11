using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Toolkit.Pooling
{
    public class Pooler : MonoBehaviour
    {
        public static Dictionary<GameObject, List<GameObject>> poolDictionary = new Dictionary<GameObject, List<GameObject>>();

        public static void AddObjectsToPool(PoolDefinition poolData, float perObjectLoadTime = 0.1f)
        {
            if (!poolDictionary.ContainsKey(poolData.obj))
            {
                poolDictionary.Add(poolData.obj, new List<GameObject>());
            }

            for (int i = 0; i < poolData.amount; i++)
            {
                GameObject obj = Instantiate(poolData.obj);
                obj.SetActive(false);
                poolDictionary[poolData.obj].Add(obj);
                UniTask.Delay((int)perObjectLoadTime * 1000);
            }
        }

        public static GameObject Spawn(SpawnData data)
        {
            //get an object from the pool and set it to active instead of directly instantiating it
            GameObject spawnedObject = null;
            if (poolDictionary.ContainsKey(data.prefab))
            {
                for (int i = 0; i < poolDictionary[data.prefab].Count; i++)
                {
                    if (!poolDictionary[data.prefab][i].activeInHierarchy)
                    {
                        spawnedObject = poolDictionary[data.prefab][i];
                        spawnedObject.transform.position = data.position;
                        spawnedObject.transform.rotation = data.rotation;
                        spawnedObject.SetActive(true);
                        break;
                    }
                }
            }

            return spawnedObject;
        }
    }

    [System.Serializable]
    public struct SpawnData
    {
        public GameObject prefab { get; set; }
        public Vector3 position { get; set; }
        public Quaternion rotation { get; set; }
    }

    [System.Serializable]
    public struct PoolDefinition
    {
        public GameObject obj { get; set; }
        public int amount { get; set; }
    }

}
