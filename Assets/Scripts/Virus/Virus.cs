using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static VirusModel;

public class Virus : MonoBehaviour
{
    #region Not Implemented
    //public MovingState movingState;
    //public InfectionState state;
    //public VirusMaterials infectedMaterial;
    //public bool isInfected = true;
    //public bool isChasing;
    #endregion

    [SerializeField] NavMeshAgent nav;
    [SerializeField] MeshRenderer meshRenderer;
    [SerializeField] Collider col;

    private Transform target;
    [SerializeField] bool foundTarget;
    private float passedTime;

    private void Awake()
    {
        nav = this.GetComponent<NavMeshAgent>();
        meshRenderer = this.GetComponent<MeshRenderer>();
        col = this.GetComponent<Collider>();
    }
    private void Start()
    {
        meshRenderer.material.color = Color.red;
        col.isTrigger = true;
    }

    private void Update()
    {
        passedTime += Time.deltaTime;

        if (!foundTarget)
        {
            AutoFindTarget();
        }
        else if (foundTarget && passedTime <= 5)
        {
            foundTarget = false;
            passedTime = 0;
        }
    }

    private void OnTriggerEnter(Collider collision)// Virus is attached(Infected), Person script is disabled and Person is removed from PersonList
    {
        Person person = collision.GetComponent<Person>();

        if (person != null && person.enabled)
        {
            if (person.GetHealthyState() && !person.GetImmuneState())
            {
                person.SetHealthyStatus(false);
                person.enabled = false;
                collision.gameObject.AddComponent<Virus>();                
                collision.gameObject.AddComponent<EnemyHit>().particlesPrefab = GetComponent<EnemyHit>().particlesPrefab;
                PersonList.Instance.RemoveFromUnitList(collision.gameObject);
                print("if.Collision");
                foundTarget = false;
            }
            else if (person.GetImmuneState())
            {
                PersonList.Instance.RemoveFromUnitList(collision.gameObject);
                foundTarget = false;
                print("elseif.Colision");
            }
            else
            {
                foundTarget = false;
            }
        }
        print("collision");
    }

    private Vector3 GetRandomGameBoardLocation()
    {
        NavMesh.SamplePosition(Random.insideUnitSphere * 25f, out NavMeshHit hit, 50f, NavMesh.AllAreas);// get random position on Navmesh
        return hit.position;
        #region Not used
        //NavMeshTriangulation navMeshData = NavMesh.CalculateTriangulation();
        //int maxIndices = navMeshData.indices.Length - 3;
        //
        //// pick the first indice of a random triangle in the nav mesh
        //int firstVertexSelected = UnityEngine.Random.Range(0, maxIndices);
        //int secondVertexSelected = UnityEngine.Random.Range(0, maxIndices);
        //
        //// spawn on verticies
        //Vector3 point = navMeshData.vertices[navMeshData.indices[firstVertexSelected]];
        //
        //Vector3 firstVertexPosition = navMeshData.vertices[navMeshData.indices[firstVertexSelected]];
        //Vector3 secondVertexPosition = navMeshData.vertices[navMeshData.indices[secondVertexSelected]];
        //
        //// eliminate points that share a similar X or Z position to stop spawining in square grid line formations
        //if ((int)firstVertexPosition.x == (int)secondVertexPosition.x || (int)firstVertexPosition.z == (int)secondVertexPosition.z)
        //{
        //    point = GetRandomGameBoardLocation(); // re-roll a position - I'm not happy with this recursion it could be better
        //}
        //else
        //{
        //    // select a random point on it
        //    point = Vector3.Lerp(firstVertexPosition, secondVertexPosition, Random.Range(0.05f, 0.95f));
        //}
        //
        //return point;
        #endregion
    }

    void AutoFindTarget()
    {
        Transform nearestHealthyPerson = null;
        float closestDistanceSqr = Mathf.Infinity;

        foreach (UnityEngine.GameObject personGO in PersonList.Instance.GetListOfPersons())
        {
            Vector3 directionToTarget = personGO.transform.position - transform.position;
            float dSqrToTarget = directionToTarget.sqrMagnitude;
            if (dSqrToTarget < closestDistanceSqr)
            {
                closestDistanceSqr = dSqrToTarget;
                nearestHealthyPerson = personGO.transform;
            }
        }
        if (nearestHealthyPerson != null)
        {
            target = nearestHealthyPerson;
            nav.SetDestination(target.position);//target.position
            foundTarget = true;//set to true    
        }
    }
}
