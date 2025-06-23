using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class OpposingKnockbackMovement : IKnockback
{
    public float force = 5f;
    public Vector2 verticalRange = new Vector2(0.5f, 1f);
    public float torqueForce = 1f;
    public float stopVelocityThreshold = 0.1f;
    public float afterStopDelay = 0.1f;

    public event Action OnMovementCompleted;

    public void Move(Animal animal, Vector3 direction)
    {
        animal.StartCoroutine(KnockbackRoutine(animal, direction));
    }

    private IEnumerator KnockbackRoutine(Animal animal, Vector3 direction)
    {
        Rigidbody rb = animal.GetComponent<Rigidbody>();
        if (rb == null) yield break;
        
        Vector3 dir = direction.normalized;
        if (dir == Vector3.zero)
        {
            dir = Random.onUnitSphere;
            dir.y = 0;
            dir.Normalize();
        }
        float v = Random.Range(verticalRange.x, verticalRange.y);
        Vector3 impulse = (dir + Vector3.up * v).normalized * force;
        rb.AddForce(impulse, ForceMode.Impulse);
        
        Vector3 torque = Random.onUnitSphere * torqueForce;
        rb.AddTorque(torque, ForceMode.Impulse);
        
        OnMovementCompleted?.Invoke();
    }
}