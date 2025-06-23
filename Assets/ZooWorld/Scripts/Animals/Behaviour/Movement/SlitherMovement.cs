using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class SlitherMovement : IMovement
{
    public float rotationDuration = 0.2f;
    public float slitherForce = 10f;
    
    public event Action OnMovementCompleted;

    public void Move(Animal animal, Vector3 target)
    {
        animal.StartCoroutine(SlitherRoutine(animal, target));
    }

    private IEnumerator SlitherRoutine(Animal animal, Vector3 target)
    {
        Transform tr = animal.transform;
        Rigidbody rb = animal.GetComponent<Rigidbody>();

        Quaternion startRot = tr.rotation;
        Vector3 dir = target - tr.position;
        dir.y = 0;
        Quaternion desiredRot = dir.sqrMagnitude > 0.001f
            ? Quaternion.LookRotation(dir)
            : startRot;

        float t = 0f;
        while (t < rotationDuration)
        {
            tr.rotation = Quaternion.Slerp(startRot, desiredRot, t / rotationDuration);
            t += Time.deltaTime;
            yield return null;
        }
        tr.rotation = desiredRot;

        rb.isKinematic = false;
        rb.AddForce(dir.normalized * slitherForce, ForceMode.Impulse);

        float slitherDuration = Random.Range(0.3f,4);
        float timer = 0f;
        while (timer < slitherDuration)
        {
            timer += Time.deltaTime;
            yield return null;
        }

        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        OnMovementCompleted?.Invoke();
    }
}