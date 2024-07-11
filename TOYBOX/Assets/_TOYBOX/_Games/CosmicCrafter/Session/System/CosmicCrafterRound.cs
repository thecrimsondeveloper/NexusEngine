using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.VFX;

namespace ToyBox.Games.CosmicCrafter
{
    public class CosmicCrafterRound : Round
    {

        [SerializeField] CosmicCrafterTutorial tutorialHandler = null;

        protected override void OnRoundStart()
        {
            if (roundData is CosmicCrafterRoundData cosmicCrafterRound)
            {
                foreach (RoundPlacementKeyValuePair reference in cosmicCrafterRound.objectsToPlace)
                {
                    Debug.Log("Placing: " + reference.prefabKey);
                    IRoundInitializable roundInitializable = reference.placementReference.GetComponent<IRoundInitializable>();
                    if (TryGetPlacementReference(reference.prefabKey, out GameObject placementReference))
                    {
                        Debug.Log("Placing: " + reference.prefabKey);
                        roundInitializable.InitializeForRound(placementReference);
                    }
                    else
                    {
                        Debug.Log("Could not place: " + reference.prefabKey);
                    }
                }
            }
        }
    }
    public class CosmicCrafterRoundData : RoundData
    {
        public List<SpawnPair> spawnPairs = new List<SpawnPair>();
        public RoundPlacementKeyValuePair[] objectsToPlace;
        public string[] infoTexts;
    }
}
