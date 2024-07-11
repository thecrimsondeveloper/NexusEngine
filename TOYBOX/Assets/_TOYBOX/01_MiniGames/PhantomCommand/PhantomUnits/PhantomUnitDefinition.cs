using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ToyBox.Games.PhantomCommand
{
    [System.Serializable]

    [CreateAssetMenu(fileName = "Phantom Unit Definition", menuName = "TOYBOX/Games/Phantom Command/Units/Phantom Unit Definition", order = 1)]
    public class PhantomUnitDefinition : ScriptableObject
    {
        public PhantomUnit unitPrefab;
        public string unitName;
        public float trainTime;
        public AudioClip spawnSound;
    }
}
