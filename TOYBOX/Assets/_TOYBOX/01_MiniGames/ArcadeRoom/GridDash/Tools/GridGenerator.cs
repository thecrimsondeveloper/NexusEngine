using UnityEditor;
using UnityEngine;

namespace Toolkit.EditorTools
{
#if UNITY_EDITOR
    public class GridGeneratorWindow : EditorWindow
    {

        private int gridSizeX = 10;
        private int gridSizeY = 10;
        private int gridSizeZ = 10;
        private float cellSize = 1f;
        private float offsetX = 0f;
        private float offsetY = 0f;
        private float offsetZ = 0f;
        private GameObject prefab;
        private Transform parentTransform;

        [MenuItem("Window/Grid Generator")]
        public static void ShowWindow()
        {
            GetWindow<GridGeneratorWindow>("Grid Generator");
        }

        private void OnGUI()
        {
            GUILayout.Label("Grid Settings", EditorStyles.boldLabel);

            gridSizeX = EditorGUILayout.IntField("Grid Size X", gridSizeX);
            gridSizeY = EditorGUILayout.IntField("Grid Size Y", gridSizeY);
            gridSizeZ = EditorGUILayout.IntField("Grid Size Z", gridSizeZ);
            cellSize = EditorGUILayout.FloatField("Cell Size", cellSize);
            offsetX = EditorGUILayout.FloatField("Offset X", offsetX);
            offsetY = EditorGUILayout.FloatField("Offset Y", offsetY);
            offsetZ = EditorGUILayout.FloatField("Offset Z", offsetZ);
            prefab = EditorGUILayout.ObjectField("Prefab", prefab, typeof(GameObject), true) as GameObject;
            parentTransform = EditorGUILayout.ObjectField("Parent Transform", parentTransform, typeof(Transform), true) as Transform;

            if (GUILayout.Button("Generate Grid"))
            {
                GenerateGrid();
            }
        }

        private void GenerateGrid()
        {
            // Clear existing grid
            if (parentTransform != null)
            {
                for (int i = parentTransform.childCount - 1; i >= 0; i--)
                {
                    Transform child = parentTransform.GetChild(i);
                    if (child.name == "GridCell")
                    {
                        DestroyImmediate(child.gameObject);
                    }
                }
            }

            // Generate new grid
            Vector3 parentPosition = parentTransform.position;
            for (int x = 0; x < gridSizeX; x++)
            {
                for (int y = 0; y < gridSizeY; y++)
                {
                    for (int z = 0; z < gridSizeZ; z++)
                    {
                        GameObject cell = PrefabUtility.InstantiatePrefab(prefab) as GameObject;
                        cell.transform.SetParent(parentTransform);
                        cell.transform.localPosition = new Vector3((x + offsetX) * cellSize, (y + offsetY) * cellSize, (z + offsetZ) * cellSize);
                        cell.name = "GridCell";
                    }
                }
            }

            parentTransform.position = parentPosition;
            Debug.Log("Grid generated!");
        }
    }
#endif
}
