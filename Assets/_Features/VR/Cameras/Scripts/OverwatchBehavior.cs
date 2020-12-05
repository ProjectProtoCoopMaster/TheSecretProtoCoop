using UnityEngine;

namespace Gameplay.VR
{
    public class OverwatchBehavior : EntityVisionData
    {
        [SerializeField] public LayerMask overwatchMask;
        bool detectedGuard = false;

        new void Awake()
        {
            base.Awake();
            pingFrequency = 20;
        }

        private void Start()
        {
            /*
             * 1. Best practices pour shaders sur mobile et sur VR
             * 
             * VR shaders -> hardware can change how shaders render (multi, single pass), forward, deffered rendering, etc...
             * you can post-process everything at once, not camera by camera
             * VFX -> animation culling, frustrum culling also work like this
             * 
             * See performance on URP 
             * 
             * Pick your hardware, pick your pipeline, and THEN start coding your shaders
             * 
             * OVERDRAW -> enemy of performance for VR and mobile
             * 
             * always work up from minimal specs
             * 
             * check shaders on right eye and left eye, off screen
             * 
             * watch out for off-screen post processing
             * 
             * start with shader graph -> preview node is important (haha)
             * look at compiled version after 
             * 
             * Shader Graph has a "master shader" in which it "plugs in" sub-shaders
             */
        }

        // every couple of frames, ping for dead guards
        private void Update()
        {
            if (poweredOn)
            {
                framesPassed++;
                if (framesPassed % pingFrequency == 0)
                {
                    PingForDeadGuards();
                    framesPassed = 0;
                }
            }

            else return;
        }

        private void PingForDeadGuards()
        {
            myPos.x = transform.position.x;
            myPos.y = transform.position.z;

            for (int i = 0; i < awarenessManager.deadGuards.Count; i++)
            {
                targetPos.x = awarenessManager.deadGuards[i].transform.position.x;
                targetPos.y = awarenessManager.deadGuards[i].transform.position.z;

                myFinalPos.x = transform.position.x;
                myFinalPos.y = awarenessManager.deadGuards[i].transform.position.y;
                myFinalPos.z = transform.position.z;

                sqrDistToTarget = (targetPos - myPos).sqrMagnitude;

                // if the target guard is within the vision range
                if (sqrDistToTarget < rangeOfVision * rangeOfVision)
                {
                    // get the entity's direction relative to you...
                    targetDir = awarenessManager.deadGuards[i].transform.position - myFinalPos;
                    Debug.DrawLine(transform.forward, targetDir, Color.green);
                    //...if the angle between the looking dir of the tneity and a dead guard is less than the cone of vision, then you can see him
                    if (Vector3.Angle(targetDir, transform.forward) <= coneOfVision * .5f && !detectedGuard)
                    {
                        Debug.Log(gameObject.name + " can see a dead friendly");
                        awarenessManager.RaiseAlarm(this);
                        detectedGuard = true;
                    }
                }
            }
        }

        //called by Unity Event when the guard is killed
        public void UE_GuardDied()
        {
            if (awarenessManager.alarmRaisers.Contains(this)) awarenessManager.alarmRaisers.Remove(this);
            awarenessManager.deadGuards.Add(gameObject);
            enabled = false;
        }

        #region Mobile Camera Power
        // called from VR_CameraBehavior
        public void UE_OverwatchOn()
        {
            poweredOn = true;
        }

        public void UE_OverwatchOff()
        {
            poweredOn = false;
        }
        #endregion
    }
}