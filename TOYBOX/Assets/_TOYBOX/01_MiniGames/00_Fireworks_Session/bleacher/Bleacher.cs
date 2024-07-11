using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Extras;

namespace ToyBox.Minigames.Fireworks
{
    public class Bleacher : MonoBehaviour
    {
        [SerializeField] private Bounds[] bleacherSpawnBounds = new Bounds[0];
        [SerializeField] List<GameObject> npcsInBleacher = new List<GameObject>();
        private void Start()
        {
            SetupBleacherSpawnBounds();

            SpawnNPCs(FireworksSession.Data.StartingCrowdSize, transform, bleacherSpawnBounds);

            FireworkSpawner.RequestFulfilled.AddListener(OnRequestFulfilled);
            FireworksSession.DepleteCrowd.AddListener(OnDepleteCrowd);
        }

        private void SetupBleacherSpawnBounds()
        {
            //get an array of all of the bleacher's colliders (on this gameobject) and store them in an array
            BoxCollider[] bleacherColliders = GetComponents<BoxCollider>();
            //initialize the bleacher spawn bounds array to be the same size as the bleacher colliders array
            bleacherSpawnBounds = new Bounds[bleacherColliders.Length];

            //for each collider in the array
            for (int i = 0; i < bleacherColliders.Length; i++)
            {
                //set the bounds of the bleacher spawn bounds to be .2f above the collider's bounds
                bleacherSpawnBounds[i] = bleacherColliders[i].bounds;
                bleacherSpawnBounds[i].center += Vector3.up * .2f;
            }
        }

        private void OnRequestFulfilled()
        {
            SpawnNPCs(1, transform, bleacherSpawnBounds);
        }

        private void OnDepleteCrowd(int depletionAmount)
        {
            DespawnNPC(depletionAmount);
        }

        private void SpawnNPCs(int numNPCs, Transform crowdParent, Bounds[] spawnBounds)
        {
            Debug.Log("Spawning NPCs");
            for (int i = 0; i < numNPCs; i++)
            {
                //get a random position within the crowd spawn bounds
                Vector3 spawnPos = BoundsInterface.GetRandomPointInBounds(spawnBounds);
                //create a new game object at the spawn point
                GameObject npcObject = Instantiate(FireworksSession.Data.CrowdNPCPrefab, spawnPos, Quaternion.identity);
                //set the parent of the npc object to the crowd parent
                npcObject.transform.SetParent(crowdParent);
                //add the npc to the list of npcs
                npcsInBleacher.Add(npcObject);
            }
        }

        private void DespawnNPC(int numNPCs)
        {
            Debug.Log("Despawning NPCs");
            for (int i = 0; i < numNPCs; i++)
            {
                GameObject npc = npcsInBleacher[0];
                //remove the npc from the list
                npcsInBleacher.Remove(npc);
                //destroy the npc
                Destroy(npc);
            }
        }
    }
}
