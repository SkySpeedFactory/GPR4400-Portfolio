using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class NewPerlinNoiseModel : ScriptableObject
{
    public abstract float[,] GenerateNoise(int mapX, int mapZ, int seed, float scale, int octaves, float persistance, float lacunarity, Vector2 offset);
}