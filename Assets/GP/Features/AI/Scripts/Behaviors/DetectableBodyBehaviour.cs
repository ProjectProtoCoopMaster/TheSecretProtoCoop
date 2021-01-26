using Gameplay.VR;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectableBodyBehaviour : MonoBehaviour
{
    void Awake()
    {
        // tell the overwatch parent that this is the part of the ragdoll's body that needs to be detected
        //transform.parent.parent.parent.GetComponent<OverwatchBehavior>().myDetectableBody = this.transform;
    }
}
