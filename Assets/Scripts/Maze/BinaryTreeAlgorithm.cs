using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Maze/Algorithm/BinaryTree")]
public class BinaryTreeAlgorithm : MazeAlgorithm
{
    [Range(1, 200)]
    [SerializeField] int xSize;
    [Range(1, 200)]
    [SerializeField] int zSize;



    public override void CreateMaze()
    {
        
    }
}
