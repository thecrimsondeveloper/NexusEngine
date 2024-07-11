using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using Toolkit.Extras;
using Toolkit.XR;
using ToyBox.Minigames.CosmicCrafter;
using UnityEngine;

namespace ToyBox.Games.CosmicCrafter
{
    public class CosmicExternalInteractions : MonoBehaviour
    {
        [SerializeField] Transform leftRotationAnchor;
        [SerializeField] Transform rightRotationAnchor;
        [SerializeField] GameObject elementHolderPrefab;
        [SerializeField] Vector3 headPosition;
        [SerializeField] NumberRange headDistanceRange;
        [SerializeField] NumberRange leftAngleOffsetRange;
        [SerializeField] NumberRange rightAngleOffsetRange;
        [SerializeField] float verticalSpawnAngle = 0;

        List<GameObject> elementParents = new List<GameObject>();
        float distanceFromHead;
        Vector3 directionToHead;

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (Application.isPlaying) return;

            Gizmos.color = Color.red;
            Gizmos.DrawRay(transform.position, directionToHead);

            //set the headPosition to the scene camera position
            headPosition = UnityEditor.SceneView.lastActiveSceneView.camera.transform.position;


            Update();
        }

#endif

        private void Update()
        {
#if UNITY_EDITOR
            //get the direction from the head to the element
            if (Application.isPlaying)
            {
                headPosition = XRPlayer.HeadPose.position;
            }
#else
            headPosition = XRPlayer.HeadPose.position;
#endif




            directionToHead = (headPosition - transform.position);
            Debug.DrawRay(transform.position, directionToHead, Color.red, 5);
            distanceFromHead = directionToHead.magnitude;
            directionToHead.Normalize();
            distanceFromHead = headDistanceRange.Clamp(distanceFromHead);




            float leftCalculatedOffset = leftAngleOffsetRange.Lerp(distanceFromHead / headDistanceRange.b);
            float rightCalculatedOffset = rightAngleOffsetRange.Lerp(distanceFromHead / headDistanceRange.b);
            float headAngle = Mathf.Atan2(directionToHead.x, directionToHead.z) * Mathf.Rad2Deg;



            Quaternion leftTargetRotation = Quaternion.Euler(0, headAngle + leftCalculatedOffset, 0);
            Quaternion rightTargetRotation = Quaternion.Euler(0, headAngle - leftCalculatedOffset, 0);

            //if the angle is too far off, snap to the target rotation
            if (Quaternion.Angle(leftRotationAnchor.rotation, leftTargetRotation) > 30)
            {
                leftRotationAnchor.rotation = leftTargetRotation;
            }
            else
            {
                leftRotationAnchor.rotation = Quaternion.Slerp(leftRotationAnchor.rotation, leftTargetRotation, Time.deltaTime * 5);
            }

            if (Quaternion.Angle(rightRotationAnchor.rotation, rightTargetRotation) > 30)
            {
                rightRotationAnchor.rotation = rightTargetRotation;
            }
            else
            {
                rightRotationAnchor.rotation = Quaternion.Slerp(rightRotationAnchor.rotation, rightTargetRotation, Time.deltaTime * 5);
            }
        }

        public async UniTask ClearExistingRoundElements()
        {
            float time = Time.time;
            //clear the existing elements
            while (leftRotationAnchor.childCount > 0)
            {
#if UNITY_EDITOR
                if (Application.isPlaying) Destroy(leftRotationAnchor.GetChild(0).gameObject);
                else DestroyImmediate(leftRotationAnchor.GetChild(0).gameObject);

#else
                Destroy(leftRotationAnchor.GetChild(0).gameObject);
#endif

                await UniTask.Yield(); //wait a frame

                if (Time.time - time > 5f) break; //if it takes too long, break
            }

            elementParents.Clear();
        }

        // [Button]
        // public async void SetupRoundElements(CosmicCrafterRoundDefinition roundDefinition)
        // {
        //     await ClearExistingRoundElements();

        //     int elementNumber = 0;

        //     for (int i = 0; i < roundDefinition.spawnPairs.Count; i++)
        //     {
        //         for (int j = 0; j < roundDefinition.spawnPairs[i].count; j++)
        //         {
        //             SetupElement(elementNumber, roundDefinition.spawnPairs[i].prefab);
        //             await UniTask.Yield();
        //             elementNumber++;
        //         }
        //     }
        // }

        [Button]
        public void SetupElement(int index = 0, GameObject prefab = null)
        {
            //create a new empty game object to act as the anchor for the element as a child of the rotation anchor
            GameObject elementAnchor = new GameObject("Element Anchor");
            elementAnchor.transform.SetParent(leftRotationAnchor);
            elementAnchor.transform.localPosition = Vector3.zero;
            elementAnchor.transform.localRotation = Quaternion.identity;
            elementParents.Add(elementAnchor);

            //instantiate the element holder prefab as a child of the element anchor
            GameObject elementHolder = Instantiate(elementHolderPrefab, elementAnchor.transform);

            //set the elements X rotation to 10 * the index
            elementAnchor.transform.localRotation = Quaternion.Euler(-verticalSpawnAngle * index + 40, 0, 0);

            //instantiate a cube as a child of the element anchor holder
            GameObject element = Instantiate(prefab, elementHolder.transform);
            element.transform.SetParent(elementHolder.transform);
            elementHolder.transform.localPosition = new Vector3(0, 0, 0.7f);
        }


    }
}
