using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour
{
    public UnityEngine.GameObject patientZero;
    
    void Start()
    {
        Instantiate(patientZero, this.transform.position, Quaternion.identity);
    }

    
}
