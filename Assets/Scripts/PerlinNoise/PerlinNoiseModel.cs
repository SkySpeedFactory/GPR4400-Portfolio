using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PerlinNoiseModel : ScriptableObject
{
    public abstract float GenerateNoise(float x, float z);
}
