using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Toolkit.Sessions;
using Sirenix.OdinInspector;
using Extras;
using UnityEngine.Events;

namespace ToyBox.Minigames.Fireworks
{
    public class FireworksSession : Session
    {
        [SerializeField, FoldoutGroup("Debugging")] private Bounds crowdSpawnBounds;
        [SerializeField, FoldoutGroup("Debugging")] private Bounds requestSpawnBounds;
        [SerializeField, FoldoutGroup("Dependencies")] private Transform crowdParent = null;
        [SerializeField, FoldoutGroup("Dependencies")] private Transform requestParent = null;
        [SerializeField, FoldoutGroup("Dependencies")] private Transform bleacher = null;

        //make a unity event with a  parameter for the number of NPCs to despawn
        public static UnityEvent<int> DepleteCrowd;


        //reference to the session data
        [SerializeField, FoldoutGroup("Data")] static FireworksSessionData sessionData = null;
        public static FireworksSessionData Data => sessionData;

        public override SessionData SessionData
        {
            get => sessionData;
            set => sessionData = value as FireworksSessionData;
        }

        //a static variable to reference the current wave
        [SerializeField, FoldoutGroup("Data"), ShowInInspector] public static FireworksWave currWave;

        [ShowInInspector] public static Dictionary<FireworkRequest.RequestType, List<GameObject>> spawnedRequests = new Dictionary<FireworkRequest.RequestType, List<GameObject>>();



        #region Editor Debugging
        void OnDrawGizmos()
        {
            //draw the crowd spawn bounds
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(crowdSpawnBounds.center, crowdSpawnBounds.size);
            //draw the request spawn bounds
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(requestSpawnBounds.center, requestSpawnBounds.size);
        }
        #endregion

        #region Loop
        float depletionTimer = 0f;
        float spawnTimer = 0f;
        int spawnIndex = 0;
        private void Update()
        {

            if (spawnTimer <= 0 && spawnIndex < currWave.Requests.Count)
            {
                //spawn a request from the list of requests at the current spawn index
                SpawnRequest(currWave.Requests[spawnIndex], requestParent, requestSpawnBounds);
                //set the spawn timer to a random value based on the curr wave starting request rate vector2
                spawnTimer = Random.Range(currWave.RequestRateRange.x, currWave.RequestRateRange.y);
                //increment the spawn index
                spawnIndex++;
            }
            else if (spawnIndex >= currWave.Requests.Count)
            {
                NextWave();
            }

            spawnTimer -= Time.deltaTime;

            //if the time since the wave started is greater than the duration of the wave, start the next wave
            // if(Time.time - timeAtWaveStart >= currWave.Duration)
            // {
            //     NextWave();
            // }


            //if the depletion timer is greater than the depletion rate, despawn NPCs
            if (depletionTimer <= currWave.DepletionRate)
            {
                depletionTimer += Time.deltaTime;
                return;
            }
            else
            {
                depletionTimer = 0f;
                //invoke the deplete crowd event
                DepleteCrowd.Invoke(currWave.DepletionAmount);
            }

        }
        #endregion


        #region Load and Unload
        public override UniTask OnLoad()
        {
            //get the position of the OVR center eye anchor
            transform.position = GameObject.Find("CenterEyeAnchor").transform.position + new Vector3(0f, 0f, .5f);
            //initialize the spawned requests dictionary
            foreach (FireworkRequest.RequestType requestType in System.Enum.GetValues(typeof(FireworkRequest.RequestType)))
            {
                spawnedRequests.Add(requestType, new List<GameObject>());
            }
            return UniTask.CompletedTask;
        }

        public override void OnSessionStart()
        {
            // Set the current wave to the first wave in the session data
            currWave = sessionData.Waves[0];
            //Setup the crowd spawn and request bounds
            SetupBounds();
            //call the StartWave function to start the first wave
            StartWave(currWave);
        }

        public override void OnSessionEnd()
        {

        }

        public override UniTask OnUnload()
        {
            // Clean up any resources when the session is unloaded
            return UniTask.CompletedTask;
        }
        #endregion

        #region Wave Management

        float timeAtWaveStart = 0f;
        protected virtual void StartWave(FireworksWave currWave)
        {
            timeAtWaveStart = Time.time;
        }


        private void NextWave()
        {
            //if we are not on the last wave
            if (sessionData.Waves.IndexOf(currWave) + 1 < sessionData.Waves.Count)
            {
                //start the next wave
                StartWave(sessionData.GetWaveAtIndex(sessionData.Waves.IndexOf(currWave) + 1));
            }
        }

        #endregion

        #region Request Management

        private void SpawnRequest(FireworkRequest request, Transform parent, Bounds requestSpawnBounds)
        {
            Debug.Log("Spawning Request Fireworks Object");
            //get a random position within the request spawn bounds
            Vector3 spawnPos = BoundsInterface.GetRandomPointInBounds(requestSpawnBounds);
            //create a new game object at the spawn point
            GameObject requestObject = Instantiate(sessionData.GetRequestPrefab(request.Type), spawnPos, Quaternion.identity);
            //set the parent of the request object to the request parent
            requestObject.transform.SetParent(parent);
            //add the request to the dictionary of spawned requests
            if (spawnedRequests.ContainsKey(request.Type))
            {
                spawnedRequests[request.Type].Add(requestObject);
            }
            // else
            // {
            //     spawnedRequests.Add(request.Type, new List<GameObject> { requestObject });
            // }
        }


        #endregion

        public void SpawnFirework(string requestTypeString)
        {
            //get the request type from the string
            FireworkRequest.RequestType requestType = (FireworkRequest.RequestType)System.Enum.Parse(typeof(FireworkRequest.RequestType), requestTypeString);
            //spawn a firework of that type at the bleacher
            FireworkSpawner.SpawnFirework(requestType, bleacher.position, bleacher);
        }

        #region Bounds Management
        private void SetupBounds()
        {
            //set the request spawn bounds to a 2x2x2 cube
            requestSpawnBounds = new Bounds(Vector3.zero, new Vector3(2f, 2f, 2f));
            //set the request spawn bounds to be above the bleacher transform
            requestSpawnBounds.center = bleacher.position + Vector3.up * 2f;
        }
        #endregion
    }
}