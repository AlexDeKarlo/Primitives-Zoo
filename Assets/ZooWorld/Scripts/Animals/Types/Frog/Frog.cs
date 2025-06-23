using System;
using System.Collections;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

public class Frog : Prey
{
    [Inject] IMovement          _move;
    [Inject] IKnockback         _knock;
    
    private void Jump()
    {
        movementBehavior.Move(this, this.transform.position + new Vector3(Random.Range(-4, 4), 0, Random.Range(-4, 4)));
    }
    
    public void Awake()
    {
        movementBehavior = _move;
        knockbackBehavior = _knock;
        
        movementBehavior.OnMovementCompleted += Jump;
        Jump();
    }
    
}
