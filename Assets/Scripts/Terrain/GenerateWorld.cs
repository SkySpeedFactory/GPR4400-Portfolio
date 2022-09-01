using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateWorld : MonoBehaviour
{
    [SerializeField] NewPerlinNoiseModel terrainNoise;

    [SerializeField] UnityEngine.GameObject water, ground, grass, hill, snow;

    [Header("BlockLists")]
    List<UnityEngine.GameObject> elementList = new List<UnityEngine.GameObject>();
    
    [Header("WorldSettings")]
    UnityEngine.GameObject[,,] mapSize;
    private int Y = 20;
    [SerializeField] int mapX;
    [SerializeField] int mapZ;
    [SerializeField] float gridOffset;

    [Header("TerrainNoiseSettings")]
    [SerializeField] float noiseDetail;
    [SerializeField] float noiseHeight;
    [SerializeField] float persistance;
    [SerializeField] float lacunarity;
    [SerializeField] Vector2 offset;
    [SerializeField] int octaves;
    [SerializeField] int seed;

    private void Start()
    {
        mapSize = new UnityEngine.GameObject[Y, mapX, mapZ];
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GenerateMap();            
        }
    }
    public void GenerateMap()
    {
        if (elementList != null)
        {
            foreach (var item in elementList)
            {
                Destroy(item);
            }
        }
        elementList = new List<UnityEngine.GameObject>();

        float[,] noise = terrainNoise.GenerateNoise(mapX, mapZ, seed, noiseDetail, octaves, persistance, lacunarity, offset);
        //for (int y = 0; y < mapSize.GetLength(0); y++)
        //{
            for (int x = 0; x < mapSize.GetLength(1); x++)
            {
                for (int z = 0; z < mapSize.GetLength(2); z++)
                {
                UnityEngine.GameObject element = mapSize[0, x, z];// für weiteres sind momentan 2 elemente vorhanden
                UnityEngine.GameObject element1 = mapSize[0, x, z];
                    float height = noise[x, z] * noiseHeight;

                    //if (y <= 30 && y > 0)
                    //{
                    //    Vector3 pos = new Vector3(x * gridOffset, height, z * gridOffset);
                    //    element = Instantiate(ground, pos, Quaternion.identity);
                    //    element.transform.SetParent(this.transform);
                    //    elementList.Add(element);
                    //}

                    if (height <= 30f && height >= 0f)
                    {
                        Vector3 pos = new Vector3(x * gridOffset, height + 1f, z * gridOffset);
                        element1 = Instantiate(water, pos, Quaternion.identity);
                        element1.transform.SetParent(this.transform);
                        elementList.Add(element1);
                    }

                    if (height <= 150f && height >= 31f)
                    {
                        Vector3 pos1 = new Vector3(x * gridOffset, height + 1f, z * gridOffset);
                        element = Instantiate(grass, pos1, Quaternion.identity);
                        element.transform.SetParent(this.transform);
                        elementList.Add(element);
                    }

                    if (height <= 150f && height >= 80f)
                    {
                        Vector3 pos = new Vector3(x * gridOffset, height + 2f, z * gridOffset);
                        element = Instantiate(hill, pos, Quaternion.identity);
                        element.transform.SetParent(this.transform);
                        elementList.Add(element);
                    }
                    if (height <= 150f && height >= 120f)
                    {
                        Vector3 pos = new Vector3(x * gridOffset, height + 3f, z * gridOffset);
                        element = Instantiate(snow, pos, Quaternion.identity);
                        element.transform.SetParent(this.transform);
                        elementList.Add(element);
                    }
                }
            }
        //}

    }

}

//First Try
//public void GenerateGround()
//{
//    if (groundList != null)
//    {
//        foreach (var item in groundList)
//        {
//            Destroy(item);
//        }
//    }
//    groundList = new List<GameObject>();
//            
//    for (int x = 0; x < worldSizeX; x++)
//    {
//        for (int z = 0; z < worldSizeZ; z++)
//        {
//            Vector3 pos = new Vector3(x * gridOffset, terrainNoise.GenerateNoise(x, z) / terrainNoiseDetail * terrainNoiseHeight, z * gridOffset);
//            GameObject block = Instantiate(ground, pos, Quaternion.identity);
//            block.transform.SetParent(this.transform);
//            groundList.Add(block);
//            
//        }
//    }
//}
//
//public void GenerateGrass()
//{
//    if (grassList != null)
//    {
//        foreach (var item in grassList)
//        {
//            Destroy(item);
//        }
//    }
//    grassList = new List<GameObject>();
//
//    foreach (GameObject block in groundList)
//    {
//        Vector3 pos = new Vector3(block.transform.position.x, block.transform.position.y + 1, block.transform.position.z);
//        GameObject grassBlock = Instantiate(grass, pos, Quaternion.identity);
//        grassBlock.transform.SetParent(this.transform);
//        grassList.Add(grassBlock);            
//    }
//}
//public void GenerateHills()
//{
//    if (hillList != null)
//    {
//        foreach (var item in hillList)
//        {
//            Destroy(item);
//        }
//    }
//    hillList = new List<GameObject>();
//
//    foreach (GameObject block in grassList)
//    {
//        if (block.transform.position.y <= 5f && block.transform.position.y >= 3.5f)
//        {
//            Vector3 pos = new Vector3(block.transform.position.x, block.transform.position.y + 1, block.transform.position.z);
//            GameObject hillBlock = Instantiate(hill, pos, Quaternion.identity);
//            hillBlock.transform.SetParent(this.transform);
//            hillList.Add(hillBlock);                
//        }            
//    }
//}
//
//public void TestGround()
//{
//    foreach (GameObject genesis in hillList)
//    {
//    //hillArray = new int[0];
//    for (int i = 0; i < hillList.Count; i++)
//    {            
//        hillArray = new int[i];
//        foreach (var block in hillArray)
//        {                  
//            Vector3 pos = new Vector3(genesis.transform.position.x, genesis.transform.position.y + 1, genesis.transform.position.z);
//            GameObject hillBlock = Instantiate(hill, pos, Quaternion.identity);
//            hillBlock.transform.SetParent(this.transform);
//            hillList.Add(hillBlock);
//        }
//    }
//
//    }
//}
//public void GenerateSnow()
//{
//    if (snowList != null)
//    {
//        foreach (var item in snowList)
//        {
//            Destroy(item);
//        }
//    }
//    snowList = new List<GameObject>();
//
//    foreach (GameObject block in hillList)
//    {
//        if (block.transform.position.y <= 8f && block.transform.position.y >= 4.8f)
//        {
//            Vector3 pos = new Vector3(block.transform.position.x, block.transform.position.y + 1, block.transform.position.z);
//            GameObject snowBlock = Instantiate(snow, pos, Quaternion.identity);
//            snowBlock.transform.SetParent(this.transform);
//            snowList.Add(snowBlock);
//        }
//    }
//}


//public void GenerateWater()
//{
//    vertices = new Vector3[(worldSizeX + 1) * (worldSizeZ + 1)];
//
//    for (int i = 0, z = 0; z <= worldSizeZ; z++)
//    {
//        for (int x = 0; x <= worldSizeX; x++, i++)
//        {
//            float y = waterNoise.GenerateNoise(x, z) / waterNoiseDetail * waterNoiseHeight;
//            vertices[i] = new Vector3(x, y + 2, z);
//            //print(y);
//        }
//    }
//    triangles = new int[worldSizeX * worldSizeZ * 6];
//
//    for (int z = 0, vert = 0, tris = 0; z < worldSizeZ; z++, vert++)
//    {
//        for (int x = 0; x < worldSizeX; x++)
//        {
//            triangles[tris + 0] = vert + 0;
//            triangles[tris + 1] = vert + worldSizeX + 1;
//            triangles[tris + 2] = vert + 1;
//            triangles[tris + 3] = vert + 1;
//            triangles[tris + 4] = vert + worldSizeX + 1;
//            triangles[tris + 5] = vert + worldSizeX + 2;
//
//            vert++;
//            tris += 6;
//        }
//    }
//    #region Not Used
//foreach (GameObject block in grassList)
//{            
//    if (block.transform.position.y <= 2.5f)
//    {
//        Vector3 pos = new Vector3(block.transform.position.x, block.transform.position.y + 1, block.transform.position.z);
//        GameObject waterBlock = Instantiate(water, pos, Quaternion.identity);
//        waterBlock.transform.SetParent(this.transform);
//        waterList.Add(waterBlock);
//    }
//}
//#endregion
//}
//private void UpdateMesh()
//{
//    mesh.Clear();
//
//    mesh.vertices = vertices;
//    mesh.triangles = triangles;
//
//    mesh.RecalculateNormals();
//