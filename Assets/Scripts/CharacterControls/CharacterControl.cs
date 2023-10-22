using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharacterControl : MonoBehaviour
{
    [SerializeField]
    private float _charMoveSpeed;
    [SerializeField]
    private Animator _charAnimator;
    [SerializeField]
    private float _rotationSpeed;


    //Start is for Test Purposes
    protected void Start()
    {
        StartCoroutine( RunTowardsTarget() );
        _charAnimator.SetBool( "isRunning", true );
    }

    public virtual IEnumerator RunTowardsTarget()
    {
        Transform target = FindTarget();
        float distance = Vector3.Distance( transform.position, target.position );

        if (target != null)
        {

            while (distance > 0.5f)
            {
                Vector3 direction = (target.position - transform.position).normalized;
                Quaternion targetRotation = Quaternion.LookRotation( direction );
                transform.rotation = Quaternion.Slerp( transform.rotation , targetRotation, _rotationSpeed * Time.deltaTime );
                transform.Translate( direction * _charMoveSpeed * Time.deltaTime );
                distance = Vector3.Distance( transform.position, target.position );
                yield return null;
            }

            Attack();
        }
    }
    public virtual void Attack()
    {
        _charAnimator.SetBool( "isAttacking", true );
    }
    public abstract Transform FindTarget();
}
