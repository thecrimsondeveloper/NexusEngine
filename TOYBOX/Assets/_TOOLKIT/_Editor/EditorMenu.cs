using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace LLGD.EditorSystems
{
    public class EditorMenu : object
    {
        [MenuItem("LLGD/Editor/Find All Broken Prefabs")]
        public static void FindAllBrokenPrefabs()
        {

            //get all in project
            GameObject[] gameObjectsInProject = Resources.FindObjectsOfTypeAll<GameObject>();

            bool noBrokenPrefabs = true;
            //filter prefabs
            foreach (GameObject go in gameObjectsInProject)
            {

                PrefabAssetType type = PrefabUtility.GetPrefabAssetType(go);
                if (type == PrefabAssetType.NotAPrefab)
                {
                    continue;
                }

                string[] pathInclusions = new string[] { "Assets/_TOOLKIT", "Assets/LLGD_Nexus", "Assets/LLGD_Samples", "Assets/_TOYBOX" };


                string path = AssetDatabase.GetAssetPath(go);

                bool isIncluded = false;
                foreach (string pathInclusion in pathInclusions)
                {
                    if (path.Contains(pathInclusion))
                    {
                        isIncluded = true;
                        break;
                    }
                }

                if (!isIncluded)
                {
                    continue;
                }

                Component[] components = go.GetComponentsInChildren<Component>(true);

                bool noBrokenComponents = true;
                foreach (Component component in components)
                {
                    try
                    {

                        string thistype = component.GetType().ToString();
                    }
                    catch
                    {
                        Debug.LogError("BROKEN PREFAB: " + go.name, go);
                        noBrokenPrefabs = false;
                    }
                }
            }

            if (noBrokenPrefabs)
            {
                Debug.Log("No broken prefabs found");
            }

        }

    }
}
