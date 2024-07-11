using System.Collections;
using System.Collections.Generic;
using Toolkit.Sessions;
using UnityEngine;

namespace ToyBox.Games.CosmicCrafter
{
    [CreateAssetMenu(menuName = "TOYBOX/Sessions/Cannon Bounce Session Data")]
    public class CosmicCrafterSessionData : RoundBasedSessionData
    {

    }

    public class CosmicCrafterRoundStats : RoundStats
    {
        public int totalTries;
        public float totalAirTime;
        public float maxAirTime;
        public float maxBounceHeight;
        public int totalBounces;
        public float totalTime;
        public int totalStarsCollected;

    }

    [System.Serializable]
    public struct SpawnPair
    {
        public GameObject prefab;
        public int count;
    }
}

