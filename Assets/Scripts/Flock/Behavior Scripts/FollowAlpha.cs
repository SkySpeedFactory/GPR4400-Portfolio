using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Behavior/FollowAlpha")]
public class FollowAlpha : FilteredFlockBehavior
{
    public override Vector3 CalculateMove(FlockAgent agent, List<Transform> context, Flock flock)
    {
        Vector3 move = Vector3.zero;

        //if no neighbors, return no adjustment
        if (context.Count == 0)
        {
            return Vector3.zero;
        }

        List<Transform> filteredContext = (filter == null) ? context : filter.Filter(agent, context);
        for (int i = 0; i < filteredContext.Count; i++)
        {
            Transform item = filteredContext[i];
            if (!item.tag.Contains("Alpha"))
            {
                move += flock.alpha.position;                
            }                   
        }
        move.y = 0;
        return move;
    }
}
