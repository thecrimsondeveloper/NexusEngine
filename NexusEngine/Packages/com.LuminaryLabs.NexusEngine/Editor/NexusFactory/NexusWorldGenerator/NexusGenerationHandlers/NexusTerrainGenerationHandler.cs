using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Cysharp.Threading.Tasks;

[System.Serializable]
public class NoiseLayer
{
    public float frequency = 0.3f;
    public float amplitude = 1f;
}

[System.Serializable]
public class LODLevel
{
    public float resolution = 1f;
    public float screenRelativeTransitionHeight = 0.6f;
}

public class NexusTerrainGenerationHandler : NexusWorldGenerationHandler
{
    private float width = 100f;
    private float depth = 100f;
    private int maxHeight = 20;

    private Vector2Int gridSize = new Vector2Int(1, 1);

    private List<LODLevel> lodLevels = new List<LODLevel>() { new LODLevel() };
    private List<NoiseLayer> noiseLayers = new List<NoiseLayer>();

    public NexusTerrainGenerationHandler(bool showName = true) : base(showName)
    {
        name = "Terrain Generation Settings";
    }

    public override async void GenerateWorld()
    {
        // Ensure spawnParent is assigned
        if (spawnParent == null)
        {
            Debug.LogWarning("Spawn Parent is not assigned.");
            return;
        }

        // Clear existing children under spawnParent
        await ClearParent();

        // Ensure there is at least one noise layer
        if (noiseLayers.Count == 0)
        {
            Debug.LogWarning("No noise layers defined. Adding a default layer.");
            noiseLayers.Add(new NoiseLayer());
        }

        // Ensure there is at least one LOD level
        if (lodLevels.Count == 0)
        {
            Debug.LogWarning("No LOD levels defined. Adding a default LOD level.");
            lodLevels.Add(new LODLevel());
        }

        // Sort LOD levels from highest resolution to lowest resolution
        lodLevels.Sort((a, b) => b.resolution.CompareTo(a.resolution));

        // Calculate the total amplitude for blending
        float totalAmplitude = 0f;
        foreach (NoiseLayer layer in noiseLayers)
        {
            totalAmplitude += layer.amplitude;
        }

        // Calculate tile size
        float tileWidth = width / gridSize.x;
        float tileDepth = depth / gridSize.y;

        // Loop over grid and generate tiles
        for (int tileZ = 0; tileZ < gridSize.y; tileZ++)
        {
            for (int tileX = 0; tileX < gridSize.x; tileX++)
            {
                // Create a GameObject to hold the LODs
                GameObject terrainObject = new GameObject($"Terrain Tile ({tileX}, {tileZ})");
                terrainObject.transform.SetParent(spawnParent, false);
                terrainObject.transform.localScale = Vector3.one; // Ensure scale is (1,1,1)

                // Set the position of the tile GameObject
                float xOffset = width / 2f;
                float zOffset = depth / 2f;
                float tileOriginX = tileX * tileWidth - xOffset;
                float tileOriginZ = tileZ * tileDepth - zOffset;
                terrainObject.transform.position = new Vector3(tileOriginX, 0f, tileOriginZ);

                // Add LODGroup component
                LODGroup lodGroup = terrainObject.AddComponent<LODGroup>();

                List<LOD> lods = new List<LOD>();

                for (int lodIndex = 0; lodIndex < lodLevels.Count; lodIndex++)
                {
                    LODLevel lodLevel = lodLevels[lodIndex];
                    float meshResolution = lodLevel.resolution;

                    // Adjust vertices for seamless edges
                    int numVertsX = Mathf.RoundToInt(tileWidth * meshResolution) + 1;
                    int numVertsZ = Mathf.RoundToInt(tileDepth * meshResolution) + 1;

                    // For tiles not on the last column, add an extra column of vertices
                    if (tileX < gridSize.x - 1)
                    {
                        numVertsX += 1;
                    }

                    // For tiles not on the last row, add an extra row of vertices
                    if (tileZ < gridSize.y - 1)
                    {
                        numVertsZ += 1;
                    }

                    Vector3[] vertices = new Vector3[numVertsX * numVertsZ];
                    int[] triangles = new int[(numVertsX - 1) * (numVertsZ - 1) * 6];

                    int vertIndex = 0;
                    int triIndex = 0;

                    for (int z = 0; z < numVertsZ; z++)
                    {
                        for (int x = 0; x < numVertsX; x++)
                        {
                            // Calculate local positions based on mesh resolution
                            float xPos = (x / meshResolution);
                            float zPos = (z / meshResolution);

                            // Calculate world positions for noise sampling
                            float worldX = terrainObject.transform.position.x + xPos;
                            float worldZ = terrainObject.transform.position.z + zPos;

                            float y = 0f;

                            // Blend contributions from each noise layer
                            foreach (NoiseLayer layer in noiseLayers)
                            {
                                float sampleX = worldX * layer.frequency;
                                float sampleZ = worldZ * layer.frequency;

                                float noiseValue = Mathf.PerlinNoise(sampleX, sampleZ);
                                y += noiseValue * (layer.amplitude / totalAmplitude);
                            }

                            y *= maxHeight;

                            vertices[vertIndex] = new Vector3(xPos, y, zPos);

                            vertIndex++;
                        }
                    }

                    for (int z = 0; z < numVertsZ - 1; z++)
                    {
                        for (int x = 0; x < numVertsX - 1; x++)
                        {
                            int topLeft = z * numVertsX + x;
                            int bottomLeft = (z + 1) * numVertsX + x;
                            int topRight = z * numVertsX + x + 1;
                            int bottomRight = (z + 1) * numVertsX + x + 1;

                            // First triangle
                            triangles[triIndex + 0] = topLeft;
                            triangles[triIndex + 1] = bottomLeft;
                            triangles[triIndex + 2] = topRight;

                            // Second triangle
                            triangles[triIndex + 3] = topRight;
                            triangles[triIndex + 4] = bottomLeft;
                            triangles[triIndex + 5] = bottomRight;

                            triIndex += 6;
                        }
                    }

                    // Create the mesh and assign the vertices and triangles
                    Mesh mesh = new Mesh();
                    mesh.name = $"Generated Terrain Mesh ({tileX}, {tileZ}) LOD {lodIndex}";
                    mesh.vertices = vertices;
                    mesh.triangles = triangles;
                    mesh.RecalculateNormals();
                    mesh.RecalculateBounds(); // Recalculate bounds

                    // Create a child GameObject to hold the mesh
                    GameObject lodObject = new GameObject($"LOD {lodIndex}", typeof(MeshFilter), typeof(MeshRenderer));
                    lodObject.transform.SetParent(terrainObject.transform, false);
                    lodObject.transform.localScale = Vector3.one; // Ensure scale is (1,1,1)

                    MeshFilter meshFilter = lodObject.GetComponent<MeshFilter>();
                    meshFilter.mesh = mesh;

                    MeshRenderer meshRenderer = lodObject.GetComponent<MeshRenderer>();
                    meshRenderer.material = new Material(Shader.Find("Standard"));

                    // Optionally, assign a custom material
                    // meshRenderer.material = yourMaterial;

                    // Set up the LOD
                    Renderer[] renderers = new Renderer[1] { meshRenderer };
                    float lodScreenRelativeTransitionHeight = lodLevel.screenRelativeTransitionHeight;
                    LOD lod = new LOD(lodScreenRelativeTransitionHeight, renderers);
                    lods.Add(lod);
                }

                // Apply the LODs to the LODGroup
                lodGroup.SetLODs(lods.ToArray());

                // Manually set the size and reference point of the LODGroup
                lodGroup.RecalculateBounds();
                Bounds combinedBounds = new Bounds();
                Renderer[] childRenderers = terrainObject.GetComponentsInChildren<Renderer>();
                if (childRenderers.Length > 0)
                {
                    combinedBounds = childRenderers[0].bounds;
                    foreach (Renderer rend in childRenderers)
                    {
                        combinedBounds.Encapsulate(rend.bounds);
                    }
                    lodGroup.size = combinedBounds.size.magnitude;
                    lodGroup.localReferencePoint = combinedBounds.center - terrainObject.transform.position;
                }

                // Register the creation in the undo system for editor support
                Undo.RegisterCreatedObjectUndo(terrainObject, "Create Terrain");
            }
        }
    }

    protected override void OnDraw()
    {
        width = EditorGUILayout.FloatField("Width", width);
        depth = EditorGUILayout.FloatField("Depth", depth);
        maxHeight = EditorGUILayout.IntField("Max Height", maxHeight);

        gridSize = EditorGUILayout.Vector2IntField("Grid Size", gridSize);

        // LOD Levels
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("LOD Levels", EditorStyles.boldLabel);

        if (lodLevels.Count == 0)
        {
            if (GUILayout.Button("Add LOD Level"))
            {
                lodLevels.Add(new LODLevel());
            }
        }
        else
        {
            for (int i = 0; i < lodLevels.Count; i++)
            {
                EditorGUILayout.BeginVertical("box");
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField($"LOD {i}", EditorStyles.boldLabel);
                if (GUILayout.Button("Remove"))
                {
                    lodLevels.RemoveAt(i);
                    break;
                }
                EditorGUILayout.EndHorizontal();

                lodLevels[i].resolution = EditorGUILayout.FloatField("Resolution", lodLevels[i].resolution);

                // Ensure that screenRelativeTransitionHeight values are decreasing
                float previousHeight = i > 0 ? lodLevels[i - 1].screenRelativeTransitionHeight : 1f;
                lodLevels[i].screenRelativeTransitionHeight = EditorGUILayout.Slider(
                    "Screen Relative Transition Height",
                    lodLevels[i].screenRelativeTransitionHeight,
                    0f,
                    previousHeight
                );
                lodLevels[i].screenRelativeTransitionHeight = Mathf.Min(lodLevels[i].screenRelativeTransitionHeight, previousHeight);

                EditorGUILayout.EndVertical();
            }
            if (GUILayout.Button("Add LOD Level"))
            {
                lodLevels.Add(new LODLevel());
            }
        }

        // Noise Layers
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Noise Layers", EditorStyles.boldLabel);

        if (noiseLayers.Count == 0)
        {
            if (GUILayout.Button("Add Noise Layer"))
            {
                noiseLayers.Add(new NoiseLayer());
            }
        }
        else
        {
            for (int i = 0; i < noiseLayers.Count; i++)
            {
                EditorGUILayout.BeginVertical("box");
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Layer " + (i + 1), EditorStyles.boldLabel);
                if (GUILayout.Button("Remove"))
                {
                    noiseLayers.RemoveAt(i);
                    break;
                }
                EditorGUILayout.EndHorizontal();

                noiseLayers[i].frequency = EditorGUILayout.Slider("Frequency", noiseLayers[i].frequency, 0.01f, 10f);
                noiseLayers[i].amplitude = EditorGUILayout.Slider("Amplitude", noiseLayers[i].amplitude, 0f, 1f);

                EditorGUILayout.EndVertical();
            }
            if (GUILayout.Button("Add Noise Layer"))
            {
                noiseLayers.Add(new NoiseLayer());
            }
        }
    }
}
