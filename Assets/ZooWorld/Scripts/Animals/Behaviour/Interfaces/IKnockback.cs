using System;
using UnityEngine;

public interface IKnockback
{
    public event Action OnMovementCompleted;
    public void Move(Animal animal, Vector3 direction);
}
