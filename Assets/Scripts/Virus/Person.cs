using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static VirusModel;

public class Person : MonoBehaviour
{
    #region Not Implemented
    //[SerializeField] InfectionState infectionState;
    #endregion

    [SerializeField] VirusMaterials healthyState;
    [SerializeField] VirusMaterials immuneState;

    [SerializeField] NavMeshAgent nav;
    [SerializeField] MeshRenderer meshRenderer;

    [SerializeField] bool isHealthy = true;
    private bool isImmune = false;

    void Awake()
    {        
        nav = GetComponent<NavMeshAgent>();
        meshRenderer = GetComponent<MeshRenderer>();
    }
    private void Start()
    {
        CalculateInfectionState();
    }

    void Update()
    {
        nav.SetDestination(GetRandomGameBoardLocation());
    }

    public void CalculateInfectionState()
    {        
        float Ro = 1 - (1 - 1 / 12f);
        float random = Random.Range(0f, 1f);
         
        if (random > Ro)
        {
            var currentState = healthyState;
            meshRenderer.material = currentState.material;
            isHealthy = true;            
        }
        else if (random < Ro)
        {            
            var currentState = immuneState;
            meshRenderer.material = currentState.material;
            isImmune = true;            
        }
    }
    private Vector3 GetRandomGameBoardLocation()
    {
        NavMesh.SamplePosition(Random.insideUnitSphere * 25f, out NavMeshHit hit, 50f, NavMesh.AllAreas);// get random position on Navmesh
        return hit.position;
    }
    
    public bool GetImmuneState()
    {
        return isImmune;
    }
    public bool GetHealthyState()
    {
        return isHealthy;
    }

    public void SetHealthyStatus(bool state)
    {
        isHealthy = state;
    }
}
