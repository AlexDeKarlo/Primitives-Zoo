using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public abstract class Prey : Animal
{
    public IMovement movementBehavior;
    public IKnockback knockbackBehavior;

    public virtual void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.GetComponent<Prey>())
        {
            Vector3 dir = transform.position - other.transform.position;
            knockbackBehavior.Move(this, dir);
        }
    }
}
