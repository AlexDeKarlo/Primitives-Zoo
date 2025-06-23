using UnityEngine;
using System;
using System.Collections;

public class JumpMovement : IMovement
{
    public float arcHeight = 4f;
    public float preJumpDuration = 0.2f;
    public float rotationDuration = 0.5f;
    
    public float stopVelocityThreshold = 0.1f;
    public float stopWaitTimeout = 2f;
    public float afterStopDelay = 0.5f;
    
    public float recoveryAngleThreshold = 25f;
    public float recoveryTotalDuration = 0.8f;
    public float recoveryLift = 1f;
    public float recoveryFallTimeout = 1.5f;

    public event Action OnMovementCompleted;

    public void Move(Animal animal, Vector3 target)
    {
        animal.StartCoroutine(JumpRoutine(animal, target));
    }

    private IEnumerator JumpRoutine(Animal animal, Vector3 target)
    {
        Transform tr = animal.transform;
        Rigidbody rb = animal.GetComponent<Rigidbody>();
        Vector3 startPos = tr.position;
        Quaternion startRot = tr.rotation;
        Vector3 startScale = tr.localScale;

        Quaternion desiredRot = startRot;
        Vector3 flatDir = target - startPos;
        flatDir.y = 0;
        if (flatDir.sqrMagnitude > 0.001f)
            desiredRot = Quaternion.LookRotation(flatDir);

        float tRot = 0f;
        while (tRot < rotationDuration)
        {
            tr.rotation = Quaternion.Slerp(startRot, desiredRot, tRot / rotationDuration);
            tRot += Time.deltaTime;
            yield return null;
        }
        tr.rotation = desiredRot;

        float elapsed = 0f;
        while (elapsed < preJumpDuration)
        {
            float t = elapsed / preJumpDuration;
            tr.localScale = Vector3.Lerp(
                startScale,
                new Vector3(startScale.x * 1.1f, startScale.y * 0.5f, startScale.z * 1.1f),
                t
            );
            elapsed += Time.deltaTime;
            yield return null;
        }
        tr.localScale = startScale;

        Vector3 toTarget = target - startPos;
        float g = Physics.gravity.y;
        float peak = Mathf.Max(startPos.y, target.y) + arcHeight;
        float vY = Mathf.Sqrt(-2f * g * (peak - startPos.y));
        float timeUp = -vY / g;
        float timeDown = Mathf.Sqrt(2f * (peak - target.y) / -g);
        float totalTime = timeUp + timeDown;
        Vector3 velXZ = new Vector3(toTarget.x, 0, toTarget.z) / totalTime;
        Vector3 launchVel = velXZ + Vector3.up * vY;
        
        rb.linearVelocity = launchVel;

        yield return new WaitForSeconds(totalTime + 0.1f);

        float threshSq = stopVelocityThreshold * stopVelocityThreshold;
        while (rb.linearVelocity.sqrMagnitude > threshSq)
            yield return null;
        
        yield return new WaitForSeconds(afterStopDelay);

        float tiltAngle = Vector3.Angle(tr.up, Vector3.up);
        if (tiltAngle > recoveryAngleThreshold)
        {
            Vector3 landingPos = tr.position;
            Quaternion startRecRot = tr.rotation;
            Quaternion targetRecRot = desiredRot;
            float upTime = recoveryTotalDuration * 0.5f;
            Vector3 liftPos = landingPos + Vector3.up * recoveryLift;

            elapsed = 0f;
            while (elapsed < upTime)
            {
                float t = elapsed / upTime;
                tr.position = Vector3.Lerp(landingPos, liftPos, t);
                tr.rotation = Quaternion.Slerp(startRecRot, targetRecRot, t);
                elapsed += Time.deltaTime;
                yield return null;
            }
            tr.position = liftPos;
            tr.rotation = targetRecRot;

            rb.linearVelocity = Vector3.zero;

            bool grounded = false;
            float fallTimer = 0f;
            while (!grounded && fallTimer < recoveryFallTimeout)
            {
                if (Physics.Raycast(tr.position + Vector3.up * 0.1f, Vector3.down, 0.2f))
                    grounded = true;
                else
                {
                    fallTimer += Time.deltaTime;
                    yield return null;
                }
            }
        }

        yield return new WaitForSeconds(0.1f);
        OnMovementCompleted?.Invoke();
    }
}
