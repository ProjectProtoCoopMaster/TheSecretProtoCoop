using UnityEngine;

namespace Gameplay.VR
{
    public class OverwatchBehavior : EntityVisionData
    {
        [SerializeField] Transform myDetectableBody;
        [SerializeField] public LayerMask overwatchMask;
        private RaycastHit hitInfo;
        bool detectedGuard = false;

        new void Awake()
        {
            base.Awake();
            pingFrequency = 1;
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
                targetPos.x = awarenessManager.deadGuards[i].position.x;
                targetPos.y = awarenessManager.deadGuards[i].position.z;

                myFinalPos.x = transform.position.x;
                myFinalPos.y = awarenessManager.deadGuards[i].position.y;
                myFinalPos.z = transform.position.z;

                sqrDistToTarget = (targetPos - myPos).sqrMagnitude;

                // if the target guard is within the vision range
                if (sqrDistToTarget < rangeOfVision * rangeOfVision)
                {
                    // get the entity's direction relative to you...
                    targetDir = awarenessManager.deadGuards[i].position - myFinalPos;

                    //...if the angle between the looking dir of the entity and a dead guard is less than the cone of vision, then you can see him
                    if (Vector3.Angle(targetDir, transform.forward) <= coneOfVision * 0.5f && !detectedGuard)
                    {
                        if (Physics.Linecast(this.transform.position, awarenessManager.deadGuards[i].position, out hitInfo, overwatchMask))
                        {
                            if (hitInfo.collider.gameObject.CompareTag("Dead"))
                            {
                                awarenessManager.RaiseAlarm(this);
                                detectedGuard = true;
                            }
                        }
                    }
                }
            }
        }

        //called by Unity Event when the guard is killed
        public void UE_GuardDied()
        {
            if (awarenessManager.alarmRaisers.Contains(this)) awarenessManager.alarmRaisers.Remove(this);
            awarenessManager.deadGuards.Add(myDetectableBody);
            myDetectableBody.gameObject.tag = "Dead";

            for (int i = 0; i < myDetectableBody.childCount; i++)
            {
                myDetectableBody.GetChild(i).tag = "Dead";
            }
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