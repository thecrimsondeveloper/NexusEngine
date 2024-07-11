using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;



namespace Toolkit.EditorTools
{
    public class GetAllOVRSceneObjects : Editor
    {
        [MenuItem("Toolkit/Get All OVR Scene Objects")]

        public static void GetAllOVRSceneObjectsInScene()
        {
            //get all prefabs in the project
            string[] guids = AssetDatabase.FindAssets("t:GameObject");

            foreach (string guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                GameObject obj = AssetDatabase.LoadAssetAtPath<GameObject>(path);


                //check if the prefab has an OVRSceneManager component
                if (obj.TryGetComponent(out OVRSceneManager ovrScene))
                {
                    Debug.Log(obj.name);
                }
            }
        }
    }
}
