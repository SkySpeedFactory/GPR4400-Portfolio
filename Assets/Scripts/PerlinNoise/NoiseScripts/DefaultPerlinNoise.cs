using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PerlinNoise/Algorithm/DefaultPerlinNoise")]
public class DefaultPerlinNoise : PerlinNoiseModel
{
    [Range(1, 100)]
    [SerializeField] float xOffset;
    [Range(1, 100)]
    [SerializeField] float zOffset;
    public override float GenerateNoise(float x, float z)
    {
        float xNoise = (x + xOffset);
        float zNoise = (z + zOffset);

        return Mathf.PerlinNoise(xNoise, zNoise);
        
    }    
}
