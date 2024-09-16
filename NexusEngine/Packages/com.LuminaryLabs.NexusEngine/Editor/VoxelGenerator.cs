using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class VoxelWorldGenerator : MonoBehaviour
{

    // Voxel settings
    public static int width = 100;  // Horizontal size
    public static int depth = 100;  // Depth size
    public static int maxHeight = 20; // Max height of the terrain
    public static float scale = 100f; // Scale of the noise
    public static int seed = 0; // Seed for randomization


    // Material for the voxel world (assign in the editor if desired)
    public static Material voxelMaterial;



    [MenuItem("Tools/Generate Procedural Voxel World")]
    public static void GenerateVoxelWorld()
    {

        // Set random seed
        if (seed == 0)
        {
            seed = Random.Range(0, 10000);  // Generate a random seed if not set
        }
        Random.InitState(seed);

        // Create a parent object to hold the voxel mesh
        GameObject voxelWorld = new GameObject("VoxelWorld");
        MeshFilter meshFilter = voxelWorld.AddComponent<MeshFilter>();
        MeshRenderer meshRenderer = voxelWorld.AddComponent<MeshRenderer>();

        // Check if a material has been assigned; if not, use a default
        if (voxelMaterial == null)
        {
            voxelMaterial = new Material(Shader.Find("Standard"));
        }
        meshRenderer.material = voxelMaterial;

        // Generate voxel data based on Perlin noise with a random seed
        int[,,] voxelData = GenerateVoxelData(width, depth, maxHeight, scale);

        // Generate mesh from voxel data
        Mesh mesh = CreateMesh(voxelData);
        meshFilter.mesh = mesh;

        // Focus on the generated voxel world
        Selection.activeGameObject = voxelWorld;
        EditorGUIUtility.PingObject(voxelWorld);
    }

    private static int[,,] GenerateVoxelData(int width, int depth, int maxHeight, float scale)
    {
        int[,,] voxelData = new int[width, maxHeight, depth];

        // Generate voxel data using Perlin noise for heightmap generation
        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < depth; z++)
            {
                // Apply Perlin noise to determine terrain height at each (x, z) position
                float xCoord = (float)x / width * scale + seed;  // Incorporate seed into noise generation
                float zCoord = (float)z / depth * scale + seed;
                float noiseValue = Mathf.PerlinNoise(xCoord, zCoord);
                int height = Mathf.RoundToInt(noiseValue * (maxHeight - 1));

                // Set voxel as filled up to the calculated height
                for (int y = 0; y <= height; y++)
                {
                    voxelData[x, y, z] = 1;
                }
            }
        }
        return voxelData;
    }

    private static Mesh CreateMesh(int[,,] voxelData)
    {
        List<Vector3> vertices = new List<Vector3>();
        List<int> triangles = new List<int>();
        List<Vector2> uvs = new List<Vector2>();

        int width = voxelData.GetLength(0);
        int height = voxelData.GetLength(1);
        int depth = voxelData.GetLength(2);

        // Iterate through the voxel data and build the mesh
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                for (int z = 0; z < depth; z++)
                {
                    if (voxelData[x, y, z] == 1)
                    {
                        // Add cube faces if neighboring voxel is empty or out of bounds
                        Vector3 position = new Vector3(x, y, z);

                        // Left face
                        if (x == 0 || voxelData[x - 1, y, z] == 0)
                            CreateFace(vertices, triangles, uvs, position, Vector3.left);

                        // Right face
                        if (x == width - 1 || voxelData[x + 1, y, z] == 0)
                            CreateFace(vertices, triangles, uvs, position, Vector3.right);

                        // Bottom face
                        if (y == 0 || voxelData[x, y - 1, z] == 0)
                            CreateFace(vertices, triangles, uvs, position, Vector3.down);

                        // Top face
                        if (y == height - 1 || voxelData[x, y + 1, z] == 0)
                            CreateFace(vertices, triangles, uvs, position, Vector3.up);

                        // Back face
                        if (z == 0 || voxelData[x, y, z - 1] == 0)
                            CreateFace(vertices, triangles, uvs, position, Vector3.back);

                        // Front face
                        if (z == depth - 1 || voxelData[x, y, z + 1] == 0)
                            CreateFace(vertices, triangles, uvs, position, Vector3.forward);
                    }
                }
            }
        }

        // Create and return the final mesh
        Mesh mesh = new Mesh();
        mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32; // Support for more than 65k vertices
        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.uv = uvs.ToArray();
        mesh.RecalculateNormals();

        return mesh;
    }

    private static void CreateFace(List<Vector3> vertices, List<int> triangles, List<Vector2> uvs, Vector3 position, Vector3 direction)
    {
        int vertexIndex = vertices.Count;

        // Get face vertices
        Vector3[] faceVertices = GetFaceVertices(position, direction);

        // Add vertices
        vertices.AddRange(faceVertices);

        // Add triangles
        triangles.Add(vertexIndex + 0);
        triangles.Add(vertexIndex + 1);
        triangles.Add(vertexIndex + 2);
        triangles.Add(vertexIndex + 2);
        triangles.Add(vertexIndex + 1);
        triangles.Add(vertexIndex + 3);

        // Add UVs (simple UV mapping for now)
        uvs.Add(new Vector2(0, 0));
        uvs.Add(new Vector2(1, 0));
        uvs.Add(new Vector2(0, 1));
        uvs.Add(new Vector2(1, 1));
    }

    private static Vector3[] GetFaceVertices(Vector3 position, Vector3 direction)
    {
        Vector3[] faceVertices = new Vector3[4];
        Vector3 up = Vector3.up;
        Vector3 right = Vector3.right;

        if (direction == Vector3.up || direction == Vector3.down)
        {
            right = Vector3.right;
            up = Vector3.forward;
        }
        else if (direction == Vector3.left || direction == Vector3.right)
        {
            right = Vector3.forward;
            up = Vector3.up;
        }

        Vector3 normal = direction * 0.5f;
        Vector3 offset = position + normal;

        faceVertices[0] = offset - right * 0.5f - up * 0.5f;
        faceVertices[1] = offset + right * 0.5f - up * 0.5f;
        faceVertices[2] = offset - right * 0.5f + up * 0.5f;
        faceVertices[3] = offset + right * 0.5f + up * 0.5f;

        return faceVertices;
    }
}
