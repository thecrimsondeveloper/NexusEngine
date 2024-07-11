using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Oculus.Interaction.HandGrab;
using Cysharp.Threading.Tasks;
using Extras;

namespace ToyBox.Minigames.Fireworks
{
    [System.Serializable]
    public class FireworksWave
    {
        [SerializeField, FoldoutGroup("Wave Settings")] Vector2 requestRateRange = new Vector2(1f, 5f);
        [SerializeField, FoldoutGroup("Crowd Settings")] int depletionRate = 1;

        [SerializeField, FoldoutGroup("Request Settings")] GameObject requestPrefab = null;


        [SerializeField, FoldoutGroup("Wave Settings")] List<FireworkRequest> requests = null;
        [SerializeField, FoldoutGroup("Wave Settings")] int depletionAmount = 1;
        [SerializeField, FoldoutGroup("Wave Settings")] float waveDuration = 0f;

        public float Duration { get => waveDuration; }
        public Vector2 RequestRateRange => requestRateRange;
        public List<FireworkRequest> Requests => requests;
        public int DepletionRate => depletionRate;
        public int DepletionAmount => depletionAmount;

        public FireworksWave(Vector2 requestRateRange, Bounds crowdSpawnBounds, int depletionRate, Bounds requestSpawnBounds, List<FireworkRequest> requests, int depletionAmount, float waveDuration)
        {
            this.requestRateRange = requestRateRange;
            this.depletionRate = depletionRate;
            this.requests = requests;
            this.depletionAmount = depletionAmount;
            this.waveDuration = waveDuration;
        }
    }
}
