using System;
using System.Collections;               
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

public class AnimalFabric : MonoBehaviour, IAnimalFabric
{
    [Inject] DiContainer _container;
    [SerializeField] List<GameObject> _animalPrefabs;
    
    public event Action<Animal> OnSpawn;
    
    Coroutine _spawnCoroutine;

    void Start()
    {
        StartFabric();
    }
    
    public void StartFabric()
    {
        if (_spawnCoroutine == null)
            _spawnCoroutine = StartCoroutine(SpawnAnimals());
    }

    public void StopFabric()
    {
        if (_spawnCoroutine != null)
        {
            StopCoroutine(_spawnCoroutine);
            _spawnCoroutine = null;
        }
    }
    
    public IEnumerator SpawnAnimals()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(1f,2f));
            
            var prefabGO = _animalPrefabs[Random.Range(0, _animalPrefabs.Count)];
            var instanceGO = _container.InstantiatePrefab(prefabGO,new Vector3(Random.Range(-15f,15f),2,Random.Range(-15f,15f)),Quaternion.identity, this.transform);
            var animal = instanceGO.GetComponent<Animal>();
            OnSpawn?.Invoke(animal);
        }
    }
}