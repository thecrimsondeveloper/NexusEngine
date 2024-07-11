using System.Collections;
using System.Collections.Generic;
using FusionDemo;
using UnityEngine;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using Toolkit.Extras;




#if UNITY_EDITOR

using UnityEditor;

#endif

namespace ToyBox
{
    public class TreeGeneration : MonoBehaviour
    {
        public GameObject treePrefab;
        public Texture2D spawnMap;
        [Range(0, 1)] public float spawnThreshold = 0.5f;
        [Range(0, 1)] public float density = 0.5f;
        public float spawnBound = 100;
        public Vector3 offset => new Vector3(spawnBound / 2, 0, spawnBound / 2);

        public NumberRange scale = new NumberRange(0.8f, 1.2f);

#if UNITY_EDITOR

        private void OnDrawGizmos()
        {
            //draw a box
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(transform.position, new Vector3(spawnBound, 1, spawnBound));
        }
        public void GenerateTree(Vector3 position, float scale = 1.0f)
        {

            GameObject tree = PrefabUtility.InstantiatePrefab(treePrefab, transform) as GameObject;
            tree.transform.position = position;
            tree.transform.localScale = Vector3.one * scale;
        }

        [Title("Debug")]
        [SerializeField] int totalTrees = 0;
        [SerializeField] int totalSpawned = 0;
        [SerializeField, ShowIf("isGenerating")] int generatedSoFar = 0;
        [SerializeField, ShowIf("isGenerating"), Range(0, 1)] float progress = 0;
        bool isGenerating = false;

        [Button, ShowIf("isGenerating")]
        public void CancelGeneration()
        {
            isGenerating = false;
        }


        [Button, HideIf("isGenerating")]
        public async void GenerateTrees()
        {
            if (isGenerating)
            {
                return;
            }

            isGenerating = true;
            float time = Time.realtimeSinceStartup;
            while (transform.childCount > 0)
            {
                DestroyImmediate(transform.GetChild(0).gameObject);
                float timeSinceStart = Time.realtimeSinceStartup - time;

                if (timeSinceStart > Time.deltaTime)
                {
                    await UniTask.Yield();
                    time = Time.realtimeSinceStartup;
                }
                if (!isGenerating)
                {
                    return;
                }
            }


            float totalSpace = spawnBound * spawnBound;
            totalTrees = Mathf.RoundToInt(totalSpace * density);
            totalSpawned = 0;
            if (totalTrees > 20000)
            {
                totalTrees = 20000;
            }
            int numberOfTreesPerSide = Mathf.RoundToInt(Mathf.Sqrt(totalTrees));

            time = Time.realtimeSinceStartup;
            for (int x = 0; x < numberOfTreesPerSide; x++)
            {
                for (int y = 0; y < numberOfTreesPerSide; y++)
                {
                    float xPos = x * (spawnBound / numberOfTreesPerSide);
                    float yPos = y * (spawnBound / numberOfTreesPerSide);

                    float normalizedX = xPos / spawnBound;
                    float normalizedY = yPos / spawnBound;

                    Color color = spawnMap.GetPixelBilinear(normalizedX, normalizedY);
                    if (color.grayscale > spawnThreshold)
                    {
                        float scaleValue = scale.Random();

                        Vector2 randomCircle = Random.insideUnitCircle;
                        Vector3 randomOffset = new Vector3(randomCircle.x, 0, randomCircle.y) * (spawnBound / numberOfTreesPerSide) * 0.5f;

                        Vector3 position = new Vector3(xPos, 0, yPos) - offset + randomOffset;
                        GenerateTree(position, scaleValue);
                        totalSpawned++;
                    }

                    if (!isGenerating)
                    {
                        return;
                    }

                    generatedSoFar++;
                    progress = (float)generatedSoFar / totalTrees;
                }

                float timeSinceStart = Time.realtimeSinceStartup - time;
                if (timeSinceStart > Time.deltaTime)
                {
                    await UniTask.Yield();
                    time = Time.realtimeSinceStartup;
                }
            }
            isGenerating = false;
        }

#endif

    }
}
