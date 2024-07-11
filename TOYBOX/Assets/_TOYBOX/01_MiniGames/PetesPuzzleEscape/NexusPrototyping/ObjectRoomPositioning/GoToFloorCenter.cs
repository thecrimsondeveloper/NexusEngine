using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ToyBox
{
    public class GoToFloorCenter : MonoBehaviour
    {
        [SerializeField] OVRSceneManager sceneManager;
        [SerializeField] OVRSceneRoom room;
        // Start is called before the first frame update
        void Start()
        {
            sceneManager.SceneModelLoadedSuccessfully += OnSceneModelLoadedSuccessfully;
        }

        // Update is called once per frame
        void Update()
        {

        }

        private void OnSceneModelLoadedSuccessfully()
        {
            OVRSceneRoom room = FindObjectOfType<OVRSceneRoom>();

            if (room == null)
            {
                Debug.LogError("No room found in scene");
                return;
            }
            Transform floor = room.Floor.transform;
            Vector3 floorCenter = floor.position;

            transform.position = floorCenter;
        }
    }
}
