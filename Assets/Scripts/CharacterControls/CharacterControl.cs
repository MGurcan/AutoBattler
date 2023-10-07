using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharacterControl : MonoBehaviour
{
    [SerializeField]
    private float _charMoveSpeed;

    //Start is for Test Purposes
    protected void Start()
    {
        StartCoroutine( RunTowardsTarget() );
    }

    public virtual IEnumerator RunTowardsTarget()
    {
        Transform target = FindTarget();
        Vector3 direction = ( target.position - transform.position ).normalized;
        float distance = Vector3.Distance( transform.position, target.position );
        while ( distance > 0.5f )
        {
            transform.Translate( direction * _charMoveSpeed * Time.deltaTime );
            distance = Vector3.Distance( transform.position, target.position );
            yield return null;
        }
    }
    public abstract Transform FindTarget();
}
