using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PerlinNoise/Algorithm/OctavePerlinNoise")]
public class OctavePerlinNoise : PerlinNoiseModel
{
    [Range(1, 100)]
    [SerializeField] float xOffset;
    [Range(1, 100)]
    [SerializeField] float zOffset;
    [Range(1, 30)]
    [SerializeField] int octaves;
    [Range(0,1)]
    [SerializeField] float persistence;
    
    public override float GenerateNoise(float x, float z)
    {
        float totalX = 0;
        float totalZ = 0;
        float frequency = 1;
        float amplitude = 1;
        float maxValue = 0;

        for (int i = 0; i < octaves; i++)
        {          
            totalX = (x * frequency + xOffset) * amplitude;
            totalZ = (z * frequency + zOffset) * amplitude;
            maxValue += amplitude;
            amplitude += persistence;
            frequency *= 2;
        }
        totalX /= maxValue;
        totalZ /= maxValue;

        float xNoise = (totalX + xOffset);
        float zNoise = (totalZ + zOffset);

        return Mathf.PerlinNoise(xNoise, zNoise);
    }
}
