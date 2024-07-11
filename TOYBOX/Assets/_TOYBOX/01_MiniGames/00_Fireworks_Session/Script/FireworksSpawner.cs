using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace ToyBox.Minigames.Fireworks
{
    public class FireworkSpawner : MonoBehaviour
    {
        public static UnityEvent RequestFulfilled = new UnityEvent();
        public static void SpawnFirework(FireworkRequest.RequestType requestType, Vector3 position, Transform parent)
        {
            InstantiateFirework(requestType, position, parent);
            FulfillRequest(requestType);
        }

        private static void FulfillRequest(FireworkRequest.RequestType requestType)
        {
            //destroy the request objects of this type
            foreach (GameObject request in FireworksSession.spawnedRequests[requestType])
            {
                GameObject.Destroy(request);
            }

            //invoke the request fulfilled event
            RequestFulfilled.Invoke();

            //clear the list of requests in that key of the static dictionary in the session
            FireworksSession.spawnedRequests[requestType].Clear();
        }

        private static void InstantiateFirework(FireworkRequest.RequestType requestType, Vector3 position, Transform parent)
        {
            //depending on the request type, instantiate a firework of that type
            GameObject.Instantiate(FireworksSession.Data.GetFireWorkPrefab(requestType), position, Quaternion.identity, parent);
        }

    }
}
