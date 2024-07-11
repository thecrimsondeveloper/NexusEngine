using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ToyBox.Launcher
{
    [CreateAssetMenu(fileName = "ToySpawnerData", menuName = "TOYBOX/ToySpawnerData", order = 1)]
    public class ToySpawnerData : ScriptableObject
    {
        public List<LauncherToy> launcherToys = new List<LauncherToy>();
        public LauncherToy GetRandomToy()
        {
            int randomIndex = Random.Range(0, launcherToys.Count);
            return launcherToys[randomIndex];
        }
    }
}