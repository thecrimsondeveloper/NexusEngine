using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;

namespace ToyBox
{
    public class PipeSpawner : MonoBehaviour
    {
        [SerializeField] List<GameObject> pipeConfigurations = new List<GameObject>();
        public List<GameObject> spawnedPipeConfigurations = new List<GameObject>();

        [SerializeField] float spawnRate = 2f;

        bool shouldSpawn = true;

        private void Start()
        {
            StartCoroutine(SpawnPipes());
        }

        private IEnumerator SpawnPipes()
        {
            while (true)
            {
                yield return new WaitForSeconds(spawnRate);
                SpawnPipeConfiguration();
            }
        }

        private void SpawnPipeConfiguration()
        {
            int randomIndex = Random.Range(0, pipeConfigurations.Count);
            GameObject pipe = Instantiate(pipeConfigurations[randomIndex], transform.position, Quaternion.identity);
            spawnedPipeConfigurations.Add(pipe);
            pipe.transform.lossyScale.Set(0, 0, 0);
            pipe.transform.DOScale(new Vector3(1, 1, 1), 1f);
            //pipe.transform.SetParent(transform);
        }



        public IEnumerator ResetPipes()
        {
            shouldSpawn = false;
            yield return new WaitForSeconds(5f);
            shouldSpawn = true;
        }


    }
}
