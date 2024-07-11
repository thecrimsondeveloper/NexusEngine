using System.Collections.Generic;
using Toolkit.Sessions;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ToyBox.Minigames.Fireworks
{

    [CreateAssetMenu(menuName = "TOYBOX/Sessions/Fireworks Session Data")]
    public class FireworksSessionData : SessionData
    {
        [SerializeField, FoldoutGroup("Crowd Settings")] int startingCrowdSize = 10;
        [SerializeField, FoldoutGroup("Crowd Settings")] int maxCrowdSize = 20;
        [SerializeField, FoldoutGroup("Crowd Prefabs")] GameObject crowdNPCPrefab = null;
        [SerializeField, FoldoutGroup("WaveSettings")] List<FireworksWave> waves = null;

        [SerializeField, FoldoutGroup("Firework Prefabs")] GameObject redFirework = null;
        [SerializeField, FoldoutGroup("Firework Prefabs")] GameObject blueFirework = null;
        [SerializeField, FoldoutGroup("Firework Prefabs")] GameObject yellowFirework = null;

        [SerializeField, FoldoutGroup("Request Prefabs")] GameObject redRequestPrefab = null;
        [SerializeField, FoldoutGroup("Request Prefabs")] GameObject blueRequestPrefab = null;
        [SerializeField, FoldoutGroup("Request Prefabs")] GameObject yellowRequestPrefab = null;

        public int StartingCrowdSize => startingCrowdSize;
        public int MaxCrowdSize => maxCrowdSize;
        public List<FireworksWave> Waves => waves;
        public GameObject CrowdNPCPrefab => crowdNPCPrefab;

        public GameObject RedFirework => redFirework;
        public GameObject BlueFirework => blueFirework;
        public GameObject YellowFirework => yellowFirework;

        public GameObject RedRequestPrefab => redRequestPrefab;
        public GameObject BlueRequestPrefab => blueRequestPrefab;
        public GameObject YellowRequestPrefab => yellowRequestPrefab;

        public FireworksWave GetWaveAtIndex(int waveIndex)
        {
            return waves[waveIndex];
        }

        public GameObject GetFireWorkPrefab(FireworkRequest.RequestType requestType)
        {
            switch (requestType)
            {
                case FireworkRequest.RequestType.Red:
                    return redFirework;
                case FireworkRequest.RequestType.Blue:
                    return blueFirework;
                case FireworkRequest.RequestType.Yellow:
                    return yellowFirework;
                default:
                    return null;
            }
        }

        public GameObject GetRequestPrefab(FireworkRequest.RequestType requestType)
        {
            switch (requestType)
            {
                case FireworkRequest.RequestType.Red:
                    return redRequestPrefab;
                case FireworkRequest.RequestType.Blue:
                    return blueRequestPrefab;
                case FireworkRequest.RequestType.Yellow:
                    return yellowRequestPrefab;
                default:
                    return null;
            }
        }

    }
}

