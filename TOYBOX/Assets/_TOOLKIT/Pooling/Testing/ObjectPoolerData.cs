using UnityEngine;
using Sirenix.OdinInspector;
using System.Collections.Generic;

namespace Toolkit.Pooling // Add the ToyBox namespace
{
    [CreateAssetMenu(fileName = "ObjectPoolerData", menuName = "ToyBox/SimonSandBox/ObjectPoolerData", order = 1)]
    public class ObjectPoolerData : ScriptableObject
    {
        public List<ObjectPoolPair> ObjectPoolPairs = new List<ObjectPoolPair>();

        [System.Serializable]
        public struct ObjectPoolPair
        {
            public GameObject pooledObject;
            public int amount;
        }
    }
}