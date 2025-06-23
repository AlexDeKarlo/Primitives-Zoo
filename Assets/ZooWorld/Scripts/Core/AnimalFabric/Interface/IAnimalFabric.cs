using System;
using UnityEngine;

public interface IAnimalFabric
{
    public event Action<Animal> OnSpawn;
    public void StartFabric();
    public void StopFabric();
}
