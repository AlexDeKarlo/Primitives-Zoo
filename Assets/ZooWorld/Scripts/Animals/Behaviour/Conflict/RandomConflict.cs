using System;

public class RandomConflict : IConflict
{
    public event Action OnConflictCompleted;

    public bool Conflict(Animal firstAnimal, Animal secondAnimal)
    {
        return firstAnimal.GetInstanceID() > secondAnimal.GetInstanceID();
    }
}