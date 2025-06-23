using System;
using UnityEngine;

public interface IConflict
{
    public event Action OnConflictCompleted;
    public bool Conflict(Animal firstAnimal, Animal secondAnimal);
}
