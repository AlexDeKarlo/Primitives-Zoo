using System;
using System.Collections.Generic;
using UnityEngine;

public class PredatorHunt : IHunt
{
    public event Action<Animal, Animal> OnHuntCompleted;
    
    [SerializeField] private float _cooldown = 0.5f;
    private readonly Dictionary<int, float> _lastHuntTime = new Dictionary<int, float>();

    public void Hunt(Animal hunter, Animal prey)
    {
        int id = prey.GetInstanceID();
        float now = Time.time;
        
        if (_lastHuntTime.TryGetValue(id, out float last) && now - last < _cooldown)
            return;
        
        _lastHuntTime[id] = now;
        
        VisualEffects.Create3DText(
            "Tasty!",
            hunter.transform.position + Vector3.up * 2f,
            Color.red,
            2f,
            Quaternion.LookRotation(
                (hunter.transform.position + Vector3.up * 2f) 
                - Camera.main.transform.position
            )
        );
        
        OnHuntCompleted?.Invoke(hunter, prey);
        GameObject.Destroy(prey.gameObject);
    }
}