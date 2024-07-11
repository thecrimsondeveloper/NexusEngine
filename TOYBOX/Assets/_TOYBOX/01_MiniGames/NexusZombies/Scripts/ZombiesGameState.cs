using System.Collections;
using System.Collections.Generic;
using Toolkit.NexusEngine;
using UnityEngine;

namespace ToyBox.Minigames.NexusZombies
{
    [System.Serializable]
    public class ZombiesGameState : NexusGameState
    {
        public int round = 0;
        public int zombiesKilled = 0;
        public int zombiesSpawned = 0;


    }
}
