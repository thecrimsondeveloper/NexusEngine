using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.VersionControl;

namespace Toolkit.EditorTools
{
    public class PlayspaceWindow : EditorWindow
    {
        [MenuItem("Toolkit/Playspace Window")]
        public static void ShowWindow()
        {
            GetWindow<PlayspaceWindow>("Playspace Window");
        }

        private string savePath;
        private void OnGUI()
        {
            GUILayout.Label("Playspace Window", EditorStyles.boldLabel);

            //if the application is playing
            if (Application.isPlaying == false)
            {
                //add a divider
                EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
                GUILayout.Label("Application is not playing, this is a runtime tool.");
                return;
            }


            GameObject selectedSceneObject = Selection.activeGameObject;

            //request the user to search for the saves folder


            //create an input field for the saves path
            savePath = EditorGUILayout.TextField("Saves Path", savePath);


            //save the selected object as a prefab in the project
            if (GUILayout.Button("Save Selected Object"))
            {
                if (savePath == "")
                {
                    savePath = EditorUtility.OpenFolderPanel("Select Saves Folder", "", "");
                }



                bool isValidPath = System.IO.Directory.Exists(savePath);
                //if the path is valid
                if (isValidPath)
                {
                    //get the selected object
                    GameObject selectedObject = Selection.activeGameObject;
                    OVRScenePlane[] oVRScenePlanes = selectedObject.GetComponentsInChildren<OVRScenePlane>();
                    (Mesh, string)[] meshes = new (Mesh, string)[oVRScenePlanes.Length];
                    //loop through each object and generate a mesh from the object
                    for (int i = 0; i < oVRScenePlanes.Length; i++)
                    {
                        //get the object
                        OVRScenePlane scenePlane = oVRScenePlanes[i];
                        //get the mesh filter
                        MeshFilter meshFilter = scenePlane.GetComponent<MeshFilter>();
                        //get the mesh
                        Mesh mesh = meshFilter.sharedMesh;

                        //save the mesh to the project
                        AssetDatabase.CreateAsset(mesh, savePath + "/PlayspaceSnapshot" + i + ".asset");

                        //add the mesh to the array
                        meshes[i].Item1 = mesh;
                        meshes[i].Item2 = savePath + "/PlayspaceSnapshot" + i + ".asset";
                    }


                    //if the object is not null
                    if (selectedObject != null)
                    {
                        //get the object's name
                        string objectName = selectedObject.name;

                        //create a prefab from the object
                        PrefabUtility.SaveAsPrefabAsset(selectedObject, savePath + "/PlayspaceSnapshot.prefab");

                        //get the prefab
                        GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(savePath + "/PlayspaceSnapshot.prefab");

                        //get all of the ovrplanes
                        OVRScenePlane[] ovrScenePlanes = prefab.GetComponentsInChildren<OVRScenePlane>();

                        //loop through each ovrplane and set the mesh based on the saved mesh index
                        for (int i = 0; i < ovrScenePlanes.Length; i++)
                        {
                            //get the ovrplane
                            OVRScenePlane ovrScenePlane = ovrScenePlanes[i];
                            //get the mesh filter
                            MeshFilter meshFilter = ovrScenePlane.GetComponent<MeshFilter>();
                            //set the mesh
                            meshFilter.sharedMesh = meshes[i].Item1;
                        }
                        //refresh the project window
                        AssetDatabase.Refresh();
                        AssetDatabase.SaveAssets();
                    }
                }

            }

        }


    }
}
