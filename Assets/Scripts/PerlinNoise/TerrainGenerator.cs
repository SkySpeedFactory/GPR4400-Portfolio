using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class TerrainGenerator : MonoBehaviour
{
    Mesh mesh;
    Vector3[] vertices;
    int[] triangles;

    [SerializeField] PerlinNoiseModel noise;

    [Range(1,10)]
    [SerializeField] float detailScale;
    [Range(1,100)]
    [SerializeField] float height;

    [Range(1,500)]
    [SerializeField] int xSize;
    [Range(1,500)]
    [SerializeField] int zSize;
    private void Awake()
    {
        mesh = new Mesh();
        mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
        GetComponent<MeshFilter>().mesh = mesh;
        //GetComponent<MeshRenderer>().material.color = Color.gray;
    }
    void Update()
    {
        CreateShape();
        UpdateMesh();
    }
    
    void CreateShape()
    {
        vertices = new Vector3[(xSize + 1) * (zSize + 1)];

        for (int i = 0, z = 0; z <= zSize; z++)
        {
            for (int x = 0; x <= xSize; x++, i++)
            {
                float y = noise.GenerateNoise(x, z)/detailScale;                
                vertices[i] = new Vector3(x, y + height, z);                
                //print(y);
            }
        }
        triangles = new int[xSize * zSize * 6];        

        for (int z = 0, vert = 0, tris = 0; z < zSize; z++, vert++)
        {
            for (int x = 0; x < xSize; x++)
            {
                triangles[tris + 0] = vert + 0;
                triangles[tris + 1] = vert + xSize + 1;
                triangles[tris + 2] = vert + 1;
                triangles[tris + 3] = vert + 1;
                triangles[tris + 4] = vert + xSize + 1;
                triangles[tris + 5] = vert + xSize + 2;

                vert++;
                tris += 6;
            }            
        }
    }
    
    private void UpdateMesh()
    {
        mesh.Clear();

        mesh.vertices = vertices;
        mesh.triangles = triangles;

        mesh.RecalculateNormals();
    }
    
}
