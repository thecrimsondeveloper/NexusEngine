using System.Collections;
using System.Collections.Generic;
using Oculus.Interaction;
using Sirenix.OdinInspector;
using TMPro;
using Toolkit.Playspace;
using Toolkit.XR;
using UnityEngine;

namespace ToyBox
{
    public class HandInteractableObjectDuplicator : MonoBehaviour
    {

        [Tooltip("The prefab to spawn when the player selects an object")]
        [SerializeField] PointableUnityEventWrapper pointablePrefab;
        [SerializeField] int maxDuplicates = 10;
        [SerializeField] Animation duplicationAnimation;
        [SerializeField] TMP_Text duplicateCountText;
        [SerializeField] Transform objectParent;





        int duplicatesLeft = 10;
        PointableUnityEventWrapper currentPointable;
        List<PointableUnityEventWrapper> spawnedObjects = new List<PointableUnityEventWrapper>();
        List<PointableUnityEventWrapper> pooledObjects = new List<PointableUnityEventWrapper>();

        private void Update()
        {
            //make the text look at the player
            if (duplicateCountText)
            {
                duplicateCountText.transform.LookAt(XRPlayer.HeadPose.position);
                //rotate the text to face the player
                duplicateCountText.transform.Rotate(0, 180, 0);
            }
        }



        [Button, HideInEditorMode]
        void OnGrabCurrentObject(PointerEvent pointerEvent)
        {
            currentPointable.transform.SetParent(null);
            //results in the player holding the object they just created and a new object being spawned
            currentPointable.WhenSelect.RemoveListener(OnGrabCurrentObject);
            currentPointable = null;
            duplicationAnimation.Play();
        }



        //called by the animation event
        public void SpawnNewObject()
        {
            if (duplicatesLeft <= 0)
            {
                return;
            }


            if (pooledObjects.Count > 0)
            {
                currentPointable = pooledObjects[0];
                pooledObjects.RemoveAt(0);
                currentPointable.gameObject.SetActive(true);
                currentPointable.transform.SetParent(objectParent);
            }
            else
            {
                currentPointable = Instantiate(pointablePrefab, objectParent);
            }

            currentPointable.transform.localPosition = Vector3.zero;
            currentPointable.transform.localScale = Vector3.one;
            currentPointable.WhenSelect.AddListener(OnGrabCurrentObject);

            spawnedObjects.Add(currentPointable);



            duplicatesLeft--;

            if (duplicatesLeft <= 0)
            {
                duplicatesLeft = 0;
                if (duplicateCountText) duplicateCountText.text = "";

                return;
            }

            if (duplicateCountText) duplicateCountText.text = duplicatesLeft.ToString();
        }




        public void ClearAllObjects()
        {
            foreach (var obj in spawnedObjects)
            {
                obj.gameObject.SetActive(false);
                pooledObjects.Add(obj);
            }
            spawnedObjects.Clear();
        }

        public void SetDuplicateCount(int count)
        {
            maxDuplicates = count;
            duplicatesLeft = count;
        }

        public void ResetDuplicateLimit()
        {
            duplicatesLeft = maxDuplicates;
        }

        /// <summary>
        /// Sets the object duplicator to its initialixed state or a new max duplicate count if provided
        /// </summary>
        public void Initialize(int maxDuplicates = -1)
        {
            if (maxDuplicates > 0)
                this.maxDuplicates = maxDuplicates;
            duplicatesLeft = maxDuplicates;
            ClearAllObjects();
            SpawnNewObject();
        }
    }
}
