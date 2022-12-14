using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Behavior/SteeredCohesion")]
public class SteeredCohesionBehavior : FilteredFlockBehavior
{

    Vector3 currentVelocity;
    public float agentSmoothtime = 0.4f;

    public override Vector3 CalculateMove(FlockAgent agent, List<Transform> context, Flock flock)
    {
        //if no neighbors, return no adjustment
        if (context.Count == 0)
        {
            return Vector3.zero;
        }

        //add all points together and average
        Vector3 cohesionMove = Vector3.zero;
        List<Transform> filteredContext = (filter == null) ? context : filter.Filter(agent, context);
        for (int i = 0; i < filteredContext.Count; i++)
        {
            Transform item = filteredContext[i];
            cohesionMove += item.position;
        }
        #region Foreach
        //foreach (Transform item in filteredContext)
        //{
        //    cohesionMove += item.position;
        //}
        #endregion
        cohesionMove /= context.Count;

        //create offset from agent position
        cohesionMove -= agent.transform.position;
        cohesionMove = Vector3.SmoothDamp(agent.transform.forward, cohesionMove, ref currentVelocity, agentSmoothtime);
        cohesionMove.y = 0;
        return cohesionMove;
    }
}
