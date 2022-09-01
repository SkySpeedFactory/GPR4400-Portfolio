using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class FlockAgent : MonoBehaviour
{
    Flock agentFlock;
    public Flock AgentFlock { get { return agentFlock; } }

    public Collider agentCollider;
    public Collider AgentCollider { get { return agentCollider; } }

    [SerializeField] bool isHealthy = true;
    private bool isImmune = false;
    void Start()
    {
        agentCollider = GetComponent<Collider>();
        CalculateInfectionState();
    }

    public void Initialize(Flock flock)
    {
        agentFlock = flock;
    }
    public void Move(Vector3 velocity)
    {
        transform.forward = velocity;
        transform.position += velocity * Time.deltaTime;
    }
    //Virus Implementation test
    public void CalculateInfectionState()
    {
        float Ro = 1 - (1 - 1 / 12f);
        float random = Random.Range(0f, 1f);

        if (random > Ro)
        {
            isHealthy = true;
        }
        else if (random < Ro)
        {
            isImmune = true;
        }
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
