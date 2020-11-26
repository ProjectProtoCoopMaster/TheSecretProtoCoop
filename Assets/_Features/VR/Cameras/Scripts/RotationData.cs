using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.VR
{
    // This class will hold all of the common variables that define how entities see in the environement
    public class RotationData : MonoBehaviour
    {
        public float rotationAngle, rotationDuration,  holdTime;

        protected Vector3 baseRotation, targetRotation, currentRotation;
        protected float rotationIncrement;
        protected float timePassed;
        protected WaitForSeconds waitTime;

        protected bool isRotating;
    }
}