using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Toolkit.Extras
{
    [System.Serializable]
    public class SpawnPointHandler : MonoBehaviour
    {
        [SerializeField] List<Transform> spawnPoints;

        public Vector3 GetSpawnPoint()
        {
            return spawnPoints[Random.Range(0, spawnPoints.Count - 1)].position;
        }
#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            foreach (Transform spawnPoint in spawnPoints)
            {
                Gizmos.DrawSphere(spawnPoint.position, 0.3f);
                //draw a circle around the spawn point
                UnityEditor.Handles.DrawWireDisc(spawnPoint.position, Vector3.up, 1);
            }
        }

        Vector3 sceneCamLookPoint = Vector3.zero;
        private void OnDrawGizmosSelected()
        {
            foreach (Transform spawnPoint in spawnPoints)
            {
                UnityEditor.Handles.Label(spawnPoint.position, "    Spawn Point");
            }

            Gizmos.color = Color.green;
            Gizmos.DrawSphere(sceneCamLookPoint, 0.2f);

            //draw a label for the scene cam look point
            UnityEditor.Handles.Label(sceneCamLookPoint, "   New Spawn Point");

            //set the scene cam look point
            if (UnityEditor.SceneView.lastActiveSceneView != null)
            {
                Ray ray = new Ray(UnityEditor.SceneView.lastActiveSceneView.camera.transform.position,
                                UnityEditor.SceneView.lastActiveSceneView.camera.transform.forward);
                if (Physics.Raycast(ray, out RaycastHit hit, 1000f))
                {
                    sceneCamLookPoint = hit.point;
                }
                else
                {
                    sceneCamLookPoint = UnityEditor.SceneView.lastActiveSceneView.camera.transform.position + UnityEditor.SceneView.lastActiveSceneView.camera.transform.forward * 10f;

                }
            }
        }

#if UNITY_EDITOR

        [Button("Clear Spawn Points")]
        public async void ClearSpawnPoints()
        {
            //do you want to clear all the spawn points?
            bool clear = UnityEditor.EditorUtility.DisplayDialog("Clear Spawn Points", "Are you sure you want to clear all the spawn points?", "Yes", "No");
            if (!clear) return;
            while (transform.childCount > 0)
            {
                DestroyImmediate(transform.GetChild(0).gameObject);
                await UniTask.DelayFrame(1);
            }
            spawnPoints.Clear();

            //clear console
            Debug.ClearDeveloperConsole();
        }
#endif

        [Button("Add Spawn Point")]
        public void AddSpawnPoint()
        {
            GameObject spawnPoint = new GameObject("SpawnPoint");
            spawnPoint.transform.position = sceneCamLookPoint;
            spawnPoint.transform.SetParent(transform);
            spawnPoints.Add(spawnPoint.transform);
        }

        [Button("Search For Points In Children")]
        public void SearchForPointsInChildren()
        {
            spawnPoints.Clear();
            foreach (Transform child in transform)
            {
                spawnPoints.Add(child);
                child.name = "SpawnPoint";
            }
        }

#endif

    }
}
