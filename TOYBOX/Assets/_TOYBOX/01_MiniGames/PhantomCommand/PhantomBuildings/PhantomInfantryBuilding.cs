using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.VFX;

namespace ToyBox.Games.PhantomCommand
{
    public class PhantomInfantryBuilding : PhantomBuilding
    {
        [Title("Infantry Building Dependencies")]
        [Required]
        public VisualEffect spawnEffect;
        [Required]
        public AudioSource audioSource;
        [Title("Infantry Building Settings")]
        [SerializeField] List<PhantomUnitDefinition> spawnDefinitions = new List<PhantomUnitDefinition>();

        [Title("Infantry Building Debug")]
        [ShowInInspector, HideInEditorMode] Queue<SpawnRequest> spawnRequests = new Queue<SpawnRequest>();

        private void Update()
        {
            if (spawnRequests.Count <= 0)
            {
                return;
            }
            SpawnRequest spawnRequest = spawnRequests.Peek();
            if (spawnRequest != null)
            {
                if (spawnRequest.currentTrainTimeLeft <= 0)
                {
                    spawnRequest.currentTrainTimeLeft = spawnRequest.definition.trainTime;
                    SpawnUnit();
                }
                else
                {
                    spawnRequest.currentTrainTimeLeft -= Time.deltaTime;
                }
            }
            else
            {
                spawnRequests.Dequeue();
            }
        }

        void SpawnUnit()
        {
            SpawnRequest spawnRequest = spawnRequests.Peek();
            owner.unitController.Spawn(spawnRequest.definition.unitPrefab);

            if (spawnEffect != null)
            {
                spawnEffect.Play();
            }

            if (audioSource != null)
            {
                audioSource.PlayOneShot(spawnRequest.definition.spawnSound);
            }


            spawnRequest.spawnCount--;
            if (spawnRequest.spawnCount <= 0)
            {
                spawnRequests.Dequeue();
            }
        }



        [Button("Queue Unit")]
        void QueueUnit(PhantomUnitDefinition spawnDefinition)
        {
            SpawnRequest request = new SpawnRequest
            {
                definition = spawnDefinition,
                outputPosition = transform.position,
                currentTrainTimeLeft = spawnDefinition.trainTime
            };

            if (spawnRequests.Count == 0)
            {
                spawnRequests.Enqueue(request);
            }
            else
            {
                //get the last request in the queue,
                SpawnRequest lastRequest = spawnRequests.Last();

                //if it is the same type add to the count
                if (lastRequest.definition == request.definition)
                {
                    lastRequest.spawnCount++;
                }
                else
                {
                    spawnRequests.Enqueue(request);
                }
            }
        }

        [System.Serializable]
        class SpawnRequest
        {
            public PhantomUnitDefinition definition;
            public float currentTrainTimeLeft;
            public int spawnCount = 1;
            public Vector3 outputPosition;
        }
    }
}

