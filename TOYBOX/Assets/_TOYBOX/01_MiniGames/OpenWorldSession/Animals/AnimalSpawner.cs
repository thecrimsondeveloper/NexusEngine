using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ToyBox.Games.OpenWorld
{
    public class AnimalSpawner : MonoBehaviour
    {
        [SerializeField] Animal animalPrefab;
        [SerializeField] Transform[] pathPoints;

        float timeLeftUntilNextSpawn = 0;

        private void OnDrawGizmos()
        {
            //draw sphere
            for (int i = 0; i < pathPoints.Length - 1; i++)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawSphere(pathPoints[i].position, 0.5f);
                Gizmos.color = Color.green;
                Gizmos.DrawLine(pathPoints[i].position, pathPoints[i + 1].position);
            }
        }

        private void Update()
        {
            if (timeLeftUntilNextSpawn <= 0)
            {
                SpawnPathAnimal();
                timeLeftUntilNextSpawn = Random.Range(0.5f, 2);
            }
            else
            {
                timeLeftUntilNextSpawn -= Time.deltaTime;
            }
        }


        public void SpawnPathAnimal()
        {
            Vector3 spawnPosition = pathPoints[0].position;


            Animal animal = Instantiate(animalPrefab, spawnPosition, Quaternion.identity);


            animal.MoveTo(pathPoints[1].position);
        }

    }
}
