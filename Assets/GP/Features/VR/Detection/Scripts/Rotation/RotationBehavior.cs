using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.VR
{
    public class RotationBehavior : RotationData
    {
        private void Awake()
        {
            // assign the camera's base rotation
            baseRotation = transform.eulerAngles;

            // assign the camera's target location
            targetRotation = new Vector3(baseRotation.x, baseRotation.y + rotationAngle, baseRotation.z);

            // calculate the increments based on the angle to travel and the time to take
            rotationIncrement = rotationAngle / rotationDuration;

            waitTime = new WaitForSeconds(holdTime);
        }

        private void Start()
        {
            isRotating = true;
            StartCoroutine(Rotate());
        }

        public void RotationOn()
        {
            isRotating = true;
            StartCoroutine(Rotate());
        }
        public void RotationOff()
        {
            isRotating = false;
            StopAllCoroutines();
        }

        private IEnumerator Rotate()
        {
            // reset the clock
            timePassed = 0f;

            // apply the rotation over time
            while (timePassed <= rotationDuration)
            {
                transform.rotation = Quaternion.Euler(new Vector3(transform.rotation.eulerAngles.x,
                                                                  transform.rotation.eulerAngles.y + rotationIncrement * Time.deltaTime,
                                                                  transform.rotation.eulerAngles.z));
                timePassed += Time.deltaTime;
                yield return null;
            }

            // wait for a set amount of time
            yield return waitTime;

            // multiply the incrementor by -1 to switch its sign for the next pass
            rotationIncrement *= -1;

            // call the function recursively
            if(isRotating) StartCoroutine(Rotate());
        }
    }
}