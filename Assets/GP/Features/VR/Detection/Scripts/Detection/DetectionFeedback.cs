using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectionFeedback : MonoBehaviour
{
    public SpriteRenderer exclamationMark;

    void Awake()
    {
        exclamationMark.enabled = false;
    }

    public void PlayDetectionFeedback()
    {
        exclamationMark.enabled = true;
    }

    public void StopDetectionFeedback()
    {
        exclamationMark.enabled = false;
    }
}
