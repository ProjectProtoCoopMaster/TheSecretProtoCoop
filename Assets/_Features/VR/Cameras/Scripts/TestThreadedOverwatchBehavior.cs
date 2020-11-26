using Photon.Pun.Demo.Cockpit;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace Gameplay.VR
{
    public class TestThreadedOverwatchBehavior : EntityVisionDataInterface
    {
        Thread childThread = null;
        EventWaitHandle childThreadWait = new EventWaitHandle(true, EventResetMode.ManualReset);
        EventWaitHandle mainThreadWait = new EventWaitHandle(true, EventResetMode.ManualReset);

        Vector2 myTopPos, otherTopPos;
        Vector3 myPos;

        public void StartLoop()
        {
            // create a child thread and start it
            childThread = new Thread(ChildThreadLoop);
            childThread.Start();
            //StartCoroutine(Yes());
        }

        void ChildThreadLoop()
        {
            // Reset the WaitHandle
            childThreadWait.Reset();
            // request to wait on a WaitHandle that is reset, which will block the thread until Set() is called on the WaitHandle, but in another thread
            childThreadWait.WaitOne();

            while (true)
            {
                childThreadWait.Reset();

                for (int i = 0; i < guards.Count; i++)
                {
                    myTopPos.x = transform.position.x;
                    myTopPos.y = transform.position.z;

                    otherTopPos.x = guards[i].transform.position.x;
                    otherTopPos.y = guards[i].transform.position.z;

                    myPos.x = transform.position.x;
                    myPos.y = guards[i].transform.position.y;
                    myPos.z = transform.position.z;

                    // if the player is within the vision range
                    if (Vector2.Distance(myTopPos, otherTopPos) < rangeOfVision)
                    {
                        // get the direction of the player's head...
                        targetDir = guards[i].transform.position - myPos;
                        //...if the angle between the looking dir of the cam and the player is less than the cone of vision, then you are inside the cone of vision
                        if (Vector3.Angle(targetDir, transform.forward) <= coneOfVision * .5f) CheckGuardState(guards[i]);
                    }

                    else continue;
                }

                // this is the equivalent of calling Set() on the first handle, and WaitOne() on the other, effectively releasing one thread, and blocking the other
                WaitHandle.SignalAndWait(mainThreadWait, childThreadWait);
            }
        }

        private void Update()
        {
            mainThreadWait.WaitOne();
            mainThreadWait.Reset();

            childThreadWait.Set();
        }


        IEnumerator Yes()
        {
            for (int i = 0; i < guards.Count; i++)
            {
                myTopPos.x = transform.position.x;
                myTopPos.y = transform.position.z;
                otherTopPos.x = guards[i].transform.position.x;
                otherTopPos.y = guards[i].transform.position.z;
                myPos.x = transform.position.x;
                myPos.y = guards[i].transform.position.y;
                myPos.z = transform.position.z;

                // if the player is within the vision range
                if (Vector2.Distance(myTopPos, otherTopPos) < rangeOfVision)
                {
                    // get the direction of the player's head...
                    targetDir = guards[i].transform.position - myPos;
                    //...if the angle between the looking dir of the cam and the player is less than the cone of vision, then you are inside the cone of vision
                    if (Vector3.Angle(targetDir, transform.forward) <= coneOfVision * .5f) CheckGuardState(guards[i]);
                }

                else continue;
            }

            yield return null;
            StartCoroutine(Yes());
        }

        void CheckGuardState(GameObject guard)
        {
            Debug.DrawLine(this.transform.position, guard.transform.position, Color.red);
            if (guard.name == "DEAD") Debug.Log("Game over, bub");
        }
    }
}