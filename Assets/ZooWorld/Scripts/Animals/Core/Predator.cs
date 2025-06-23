using UnityEngine;

public class Predator : Animal
{
    public IMovement movementBehavior;
    public IHunt huntBehavior;
    public IConflict conflictBehavior;
    
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.GetComponent<Prey>())
        {
            huntBehavior.Hunt(this, other.gameObject.GetComponent<Animal>());
        }
        
        else if (other.gameObject.GetComponent<Predator>())
        {
            var conflict = conflictBehavior.Conflict(this, other.gameObject.GetComponent<Animal>());
            if (conflict)
                huntBehavior.Hunt(this, other.gameObject.GetComponent<Animal>());
        }
    }
}