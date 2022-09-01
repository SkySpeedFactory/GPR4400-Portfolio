using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeGenerator : MonoBehaviour
{
    //public GameObject terrain;
    //public MazeAlgorithm maze;
    //public GameObject wallPrefab;
    //
    //List<GameObject> spawnedObjects = new List<GameObject>();
    //
    //private void Awake()
    //{
    //    SpawnWalls();        
    //}
    //    
    //void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.Space))
    //    {
    //        SpawnWalls();
    //    }
    //}
    //
    //public void SpawnWalls()
    //{
    //    if (spawnedObjects != null)
    //    {
    //        foreach (var item in spawnedObjects)
    //        {
    //            Destroy(item);
    //        }
    //    }
    //    spawnedObjects = new List<GameObject>();
    //    
    //    Vector3 StartPos = new Vector3(-0.5f * maze.X + 0.5f, 0.5f, 0.5f * maze.Y - 0.5f);
    //    Vector3 spawnPosition;
    //    for (int i = 0; i < maze.X; i++)
    //    {
    //        for (int j = 0; j < maze.Y; j++)
    //        {
    //            if (maze.Maze[i, j].Number > 0)
    //            {
    //                spawnPosition = StartPos + new Vector3(i, 0, -j);
    //                GameObject wall = Instantiate(wallPrefab, spawnPosition, Quaternion.identity, this.transform);
    //                spawnedObjects.Add(wall);
    //            }
    //        }
    //    }
    //}
}
