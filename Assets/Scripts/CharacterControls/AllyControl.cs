using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllyControl : CharacterControl
{
    public override IEnumerator RunTowardsTarget()
    {
        return base.RunTowardsTarget();
    }

    public override Transform FindTarget()
    {
        Transform targetObject = null;
        GameObject[] targets = GameObject.FindGameObjectsWithTag( "Enemy" );
        if ( targets.Length > 0 )
        {
            float closestDistance = Mathf.Infinity;
            foreach( GameObject target in targets )
            {
                float distance = Vector3.Distance( transform.position, target.transform.position );
                Debug.Log( distance );
                if ( distance < closestDistance )
                {
                    closestDistance = distance;
                    targetObject = target.transform;
                }
            }
        }
        return targetObject;
    }
}
