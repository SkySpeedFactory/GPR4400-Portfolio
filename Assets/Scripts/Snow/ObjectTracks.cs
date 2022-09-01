using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectTracks : MonoBehaviour
{    
    private RenderTexture splatMap;
    public Shader drawShader;
    private Material snowMaterial, drawMaterial;
    public UnityEngine.GameObject terrain;
    public Transform[] tracks;    
    RaycastHit groundHit;
    int layerMask;
    [Range(1, 100)]
    public float brushSize;
    [Range(0, 1)]
    public float brushStrength;
    [Range(0, 10)]
    public float pressureDistance;

    // Start is called before the first frame update
    void Start()
    {
        layerMask = LayerMask.GetMask("Ground");
        drawMaterial = new Material(drawShader);
        snowMaterial = terrain.GetComponent<MeshRenderer>().material;
        splatMap = new RenderTexture(1024, 1024, 0, RenderTextureFormat.ARGBFloat);
        snowMaterial.SetTexture("_Splat", splatMap);
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < tracks.Length; i++)
        {
            if (Physics.Raycast(tracks[i].position, -Vector3.up, out groundHit, pressureDistance, layerMask))
            {
                drawMaterial.SetVector("_Coordinate", new Vector4(groundHit.textureCoord.x, groundHit.textureCoord.y, 0, 0));
                drawMaterial.SetFloat("_Strength", brushStrength);
                drawMaterial.SetFloat("_Size", brushSize);
                RenderTexture temp = RenderTexture.GetTemporary(splatMap.width, splatMap.height, 0, RenderTextureFormat.ARGBFloat);
                Graphics.Blit(splatMap, temp);
                Graphics.Blit(temp, splatMap, drawMaterial);
                RenderTexture.ReleaseTemporary(temp);
            }
        }
    }
}
