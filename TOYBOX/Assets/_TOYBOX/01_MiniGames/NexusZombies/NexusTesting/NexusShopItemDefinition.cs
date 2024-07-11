using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Toolkit.NexusEngine
{
    [CreateAssetMenu(fileName = "NexusShopItemDefinition", menuName = "Nexus/Shop Item Definition")]
    public class NexusShopItemDefinition : ScriptableObject
    {
        public string itemName;
        public string itemDescription;
        public Sprite itemIcon;
        public int cost;
        public GameObject prefab;
        public Vector3 spawnOffsetPosition;
        public Vector3 spawnOffsetRotation;
    }
}