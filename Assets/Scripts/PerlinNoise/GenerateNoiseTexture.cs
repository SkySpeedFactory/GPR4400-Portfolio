using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class GenerateNoiseTexture : MonoBehaviour
{
    public PerlinNoiseModel noise;    

    [Range(256, 512)]
    [SerializeField] int xSize;
    [Range(256, 512)]
    [SerializeField] int ySize;

    [SerializeField] float scale = 20f;
    
    void Start()
    {
        
    }

    void Update()
    {
        Renderer renderer = GetComponent<Renderer>();
        renderer.material.mainTexture = GenerateTexture();
    }

    Texture2D GenerateTexture()
    {
        Texture2D texture = new Texture2D(xSize, ySize);

        for (int x = 0; x < xSize; x++)
        {
            for (int y = 0; y < ySize; y++)
            {
                Color color = CalculateColor(x, y);
                texture.SetPixel(x, y, color);
            }
        }
        texture.Apply();
        return texture;
    }

    Color CalculateColor(int x, int y)
    {
        float xCoord = (float)x / xSize * scale;
        float yCoord = (float)y / ySize * scale;

        float sample = noise.GenerateNoise(xCoord,yCoord);
        return new Color(sample, sample, sample);
    }
}
