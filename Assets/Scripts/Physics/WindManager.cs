using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindManager : MonoBehaviour
{
    public float windDelta = 0.1f;
    public float windUpdateTime = 0.5f;
    public float windMaxMagnitude = 25f;    

    [SerializeField] Vector2 wind;
    private Vector2 windUpdated;
    private float time;
    
    public Vector2 GetWind() 
    {
        return wind;
    }

    private Vector2 GetUpdatedWind()
    {
        Vector2 result = wind += Random.insideUnitCircle * windDelta;
        result = result.normalized * Mathf.Min(result.magnitude, windMaxMagnitude);        
        return result;
    }
    void Start()
    {
        SetWind(Random.insideUnitCircle * 5);
    }
       
    void FixedUpdate()
    {
        UpdateWind();

        time += Time.deltaTime;
        if (time > windUpdateTime)
        {
            time = 0f;

            windUpdated = GetUpdatedWind();
        }
    }

    private void UpdateWind()
    {
        wind = Vector2.Lerp(wind, windUpdated, Time.deltaTime);
    }
    public void SetWind(Vector2 newWind)
    {
        wind = newWind;
        windUpdated = wind;
        time = 0f;
    }
}
