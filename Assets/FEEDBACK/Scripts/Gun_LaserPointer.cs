using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.VR.Player
{
    public class Gun_LaserPointer : MonoBehaviour
    {
        public float laserWidth;
        LineRenderer laserPointer;

        private void Awake()
        {
            laserPointer = GetComponent<LineRenderer>();
        }

        private void OnEnable()
        {
            laserPointer.startWidth = laserPointer.endWidth = laserWidth;
        }

        private void LateUpdate()
        {
            laserPointer.SetPosition(0, transform.GetChild(0).position);
            laserPointer.SetPosition(1, transform.GetChild(1).position);
        }
    } 
}
