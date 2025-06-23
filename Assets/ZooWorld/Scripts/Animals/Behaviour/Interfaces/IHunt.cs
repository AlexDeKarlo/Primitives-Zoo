using System;
using UnityEngine;

public interface IHunt
{
    public event Action<Animal, Animal> OnHuntCompleted;
    public void Hunt(Animal hunter, Animal prey);
}