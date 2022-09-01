using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flock : MonoBehaviour
{
    public  Transform alpha;
    [SerializeField] FlockAgent agentPrefab;
    List<FlockAgent> agents = new List<FlockAgent>();
    [SerializeField] FlockBehavior behavior;

    [Range(1, 250)]
    [SerializeField] int startingCount = 250;
    const float AgentDensity = 2.5f;

    [Range(1f, 100f)]
    [SerializeField] float driveFactor = 10f;
    [Range(1f, 100f)]
    [SerializeField] float maxSpeed = 5f;
    [Range(1f, 10f)]
    [SerializeField] float neighborRadius = 1.5f;
    [Range(0f, 1f)]
    [SerializeField] float avoidanceRadiusMultiplier = 0.5f;

    float squareMaxSpeed;
    float squareNeighborRadius;
    float squareAvoidanceRadius;
    public float SquareAvoidanceRadius { get { return squareAvoidanceRadius; } }
    private void Awake()
    {        
        squareMaxSpeed = maxSpeed * maxSpeed;
        squareNeighborRadius = neighborRadius * neighborRadius;
        squareAvoidanceRadius = squareNeighborRadius * avoidanceRadiusMultiplier * avoidanceRadiusMultiplier;
    }
    void Start()
    {
        CreateSwarm(startingCount);
    }

    void Update()
    {
        FlockMovement();        
    }

    List<Transform> GetNearbyObjects(FlockAgent agent)
    {
        List<Transform> context = new List<Transform>();
        Collider[] contextColliders = Physics.OverlapSphere(agent.transform.position, neighborRadius);
        foreach (Collider c in contextColliders)
        {
            if (c != agent.AgentCollider)
            {
                context.Add(c.transform);
            }
        }
        return context;
    }
    public void FlockMovement()
    {
        for (int i = 0; i < agents.Count; i++)
        {
            FlockAgent agent = agents[i];
            List<Transform> context = GetNearbyObjects(agent);

            Vector3 move = behavior.CalculateMove(agent, context, this);
            move *= driveFactor;
            if (move.sqrMagnitude > squareMaxSpeed)
            {
                move = move.normalized * maxSpeed;
            }
            agent.Move(move);
        }
        #region Foreach
        //foreach (FlockAgent agent in agents)
        //{
        //    List<Transform> context = GetNearbyObjects(agent);
        //
        //    //FOR DEMO ONLY
        //    //agent.GetComponentInChildren<MeshRenderer>().material.color = Color.Lerp(Color.white, Color.red, context.Count / 6f);
        //
        //    Vector3 move = behavior.CalculateMove(agent, context, this);
        //    move *= driveFactor;
        //    if (move.sqrMagnitude > squareMaxSpeed)
        //    {
        //        move = move.normalized * maxSpeed;
        //    }
        //    agent.Move(move);
        //}
        #endregion
    }
    
    public void CreateSwarm(int count)
    {
        for (int i = 0; i < count; i++)
        {
            Vector2 spawnArea = Random.insideUnitCircle;
            FlockAgent newAgent = Instantiate(agentPrefab, transform.position + new Vector3(spawnArea.x, 0, spawnArea.y) * count * AgentDensity, Quaternion.Euler(Vector3.forward), transform);
            newAgent.name = "Agent" + i;            
            newAgent.gameObject.layer = this.gameObject.layer;
            if (i == 0)
            {
                newAgent.tag = "Alpha";                
            }
            if (newAgent.tag.Contains("Alpha"))
            {
                alpha = newAgent.transform;                
            }            
            newAgent.Initialize(this);
            agents.Add(newAgent);            
        }
    }
    
}
