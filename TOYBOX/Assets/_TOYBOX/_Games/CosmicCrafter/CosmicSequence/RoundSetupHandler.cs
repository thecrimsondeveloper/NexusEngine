using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ToyBox
{
    public class RoundSetupHandler : ScriptableObject
    {
        Dictionary<string, GameObject> placementReferences = new Dictionary<string, GameObject>();


        private Round round;
        public void Initialize(Round round)
        {
            this.round = round;

            // Add placement references
            placementReferences = new Dictionary<string, GameObject>();
            foreach (var placementReference in round.roundPlacementReferences)
            {
                placementReferences.Add(placementReference.prefabKey, placementReference.placementReference);
            }
        }


        public bool TryGetPlacementReference(GameObject prefabKey, out GameObject placementReference)
        {
            if (placementReferences.TryGetValue(prefabKey.name, out var reference))
            {
                placementReference = reference;
                return true;
            }

            placementReference = null;
            return false;
        }

        public bool TryGetPlacementReferencePosition(GameObject prefabKey, out Vector3 referencePosition)
        {
            if (TryGetPlacementReference(prefabKey, out var reference))
            {
                referencePosition = reference.transform.position;
                return true;
            }

            referencePosition = Vector3.zero;
            return false;
        }




    }
}
