using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.VR.Player
{
    public class Gun_LaserPointer : MonoBehaviour
    {
        public float laserWidth;
        LineRenderer laserPointer;
        public LayerMask collisionMask;
        RaycastHit hitInfo;
        Transform barrel, target, hitPoint;

        private void Awake()
        {
            laserPointer = GetComponent<LineRenderer>();
            barrel = transform.GetChild(0);
            target = transform.GetChild(1);
        }

        private void OnEnable()
        {
            laserPointer.startWidth = laserPointer.endWidth = laserWidth;
        }

        private void FixedUpdate()
        {
            if (Physics.Raycast(transform.position, transform.GetChild(1).position - transform.position, out hitInfo, collisionMask))
            {
                if(hitPoint != null)
                    hitPoint.position = hitInfo.point;
                else hitPoint = target;
            }
        }

        private void LateUpdate()
        {
            laserPointer.SetPosition(0, barrel.position);
            laserPointer.SetPosition(1, hitPoint.position);
        }
    } 
}
