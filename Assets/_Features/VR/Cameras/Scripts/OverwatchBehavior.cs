using System.Collections;
using UnityEngine;
using System.Threading;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Gameplay.VR
{
    public class OverwatchBehavior : EntityVisionDataInterface
    {
        public int frames;
        public List<GameObject> visibleGuards = new List<GameObject>();
        bool raisingAlarm;

        private void Awake()
        {
            guards.AddRange(GameObject.FindGameObjectsWithTag("Enemy"));
        }

        private void Start()
        {
            isActive = true;
            PingForGuards();
        }

        // called if
        public void GE_Overwatching()
        {
            raisingAlarm = false;
            PingForGuards();
        }

        // for cameras
        // called from VR_CameraBehavior
        public void OverwatchOn()
        {
            isActive = true;
            PingForGuards();
        }

        public void OverwatchOff()
        {
            isActive = false;
            StopAllCoroutines();
        }

        private void Update()
        {
            frames++;
            if (frames % 2 == 0)
            {
                frames = 0;
                PingForGuards();
            }
        }

        void PingForGuards()
        {
            visibleGuards.Clear();
            for (int i = 0; i < guards.Count; i++)
            {
                myPos.x = transform.position.x;
                myPos.y = transform.position.z;

                targetPos.x = guards[i].transform.position.x;
                targetPos.y = guards[i].transform.position.z;

                myFinalPos.x = transform.position.x;
                myFinalPos.y = guards[i].transform.position.y;
                myFinalPos.z = transform.position.z;

                distToTarget = (targetPos - myPos).sqrMagnitude;

                // if the target guard is within the vision range
                if (distToTarget < rangeOfVision)
                {
                    // get the direction of the player's head...
                    targetDir = guards[i].transform.position - myFinalPos;
                    //...if the angle between the looking dir of the cam and the player is less than the cone of vision, then you are inside the cone of vision
                    if (Vector3.Angle(targetDir, transform.forward) <= coneOfVision * .5f) CheckGuardState(guards[i]);
                }

                else continue;
            }
        }

        void CheckGuardState(GameObject guard)
        {
#if UNITY_EDITOR
            if (!visibleGuards.Contains(guard)) visibleGuards.Add(guard);
#endif

            // if you see a dead guard
            if (guard.name == "DEAD")
            {
                Debug.Log("I see a dead friendly !");
                raisingAlarm = true;
                raiseAlarm.Raise();
            }
        }
    }
}