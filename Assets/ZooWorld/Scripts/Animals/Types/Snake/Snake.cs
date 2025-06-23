using UnityEngine;
using Zenject;

public class Snake : Predator
{
    [Inject] IMovement    _move;
    [Inject] IHunt        _hunt;
    [Inject] IConflict    _conf;

    private void Slith()
    {
        movementBehavior.Move(this,this.transform.position + new Vector3(Random.Range(-4,4),0,Random.Range(-4,4)));
    }

    public void Awake()
    {
        movementBehavior  = _move;
        huntBehavior      = _hunt;
        conflictBehavior  = _conf;
        
        movementBehavior.OnMovementCompleted += Slith;
        Slith();
    }
}
