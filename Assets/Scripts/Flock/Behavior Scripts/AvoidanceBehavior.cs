using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Behavior/Avoidance")]
public class AvoidanceBehavior : FilteredFlockBehavior
{
    public override Vector3 CalculateMove(FlockAgent agent, List<Transform> context, Flock flock)
    {
        //if no neighbors, return no adjustment
        if (context.Count == 0)
        {
            return Vector3.zero;
        }
        //add all points together and average
        Vector3 avoidanceMove = Vector3.zero;
        int nAvoid = 0;
        List<Transform> filteredContext = (filter == null) ? context : filter.Filter(agent, context);
        for (int i = 0; i < filteredContext.Count; i++)
        {
            Transform item = filteredContext[i];
            Vector3 closestPoint = item.gameObject.GetComponent<Collider>().ClosestPoint(agent.transform.position);
            if (Vector3.SqrMagnitude(closestPoint - agent.transform.position) < flock.SquareAvoidanceRadius)
            {
                nAvoid++;
                avoidanceMove += agent.transform.position - item.position;                
            }
            Debug.Log(nAvoid);
        }
        #region Foreach
        //foreach (Transform item in filteredContext)
        //{
        //    if (Vector3.SqrMagnitude(item.position - agent.transform.position) < flock.SquareAvoidanceRadius)
        //    {
        //        nAvoid++;
        //        avoidanceMove += agent.transform.position - item.position;
        //    }
        //}
        #endregion

        if (nAvoid > 0)
        {
            avoidanceMove /= nAvoid;
        }
        avoidanceMove.y = 0;
        return avoidanceMove;
    }
}