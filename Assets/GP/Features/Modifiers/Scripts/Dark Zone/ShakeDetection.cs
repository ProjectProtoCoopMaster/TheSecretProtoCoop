using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ShakeDetection : MonoBehaviour
{
    public float timeThreshold;
    private float currentTime;

    public float shakeThreshold;
    public int shakeRate;

    private float minShakeTime { get => timeThreshold / shakeRate; }
    private float elapsedShakeTime;

    public int shakeAmount;
    private int currentAmount;

    public void DetectShake(out bool complete)
    {
        complete = false;

        currentTime += Time.unscaledDeltaTime;

        if (Input.acceleration.sqrMagnitude >= Mathf.Pow(shakeThreshold, 2f) && Time.unscaledTime >= elapsedShakeTime + minShakeTime)
        {
            currentAmount++;
            elapsedShakeTime = Time.unscaledTime;
        }

        if (currentAmount == shakeAmount)
        {
            complete = true;
            StartShake();
        }

        if (currentTime >= timeThreshold)
        {
            StartShake();
        }
    }

    public void StartShake()
    {
        currentTime = 0.0f;
        currentAmount = 0;
        elapsedShakeTime = 0.0f;
    }
}
