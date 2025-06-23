using System;
using UnityEngine;

public interface IMovement
{
    public event Action OnMovementCompleted;
    public void Move(Animal animal, Vector3 target);
}
