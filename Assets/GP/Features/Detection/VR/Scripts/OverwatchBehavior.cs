#if UNITY_STANDALONE
using UnityEngine;

namespace Gameplay.VR
{
    public class OverwatchBehavior : VisionBehavior
    {
        [SerializeField] internal Transform myDetectableBody;

        public override void Ping()
        {
            for (int i = 0; i < awarenessManager.deadGuards.Count; i++)
            {
                if (CanSeeTarget(awarenessManager.deadGuards[i].position))
                {
                    detected = true;

                    detectionFeedback.PlayDetectionFeedback();

                    if (!awarenessManager.alarmRaisers.Contains(this.gameObject))
                        awarenessManager.alarmRaisers.Add(this.gameObject);

                    spottedDeadBody.Raise();
                }
                else continue;
            }
        }

        //called by Unity Event when the guard is killed
        public void UE_GuardDied()
        {
            myDetectableBody = GetComponentInChildren<DetectableBodyBehaviour>().transform;

            // if you were detecting the player, remove this object from the list of alarm raisers
            if (awarenessManager.alarmRaisers.Contains(this.gameObject))
                awarenessManager.alarmRaisers.Remove(this.gameObject);

            // add this entity to the list of dead guards
            if (!awarenessManager.deadGuards.Contains(myDetectableBody))
                awarenessManager.deadGuards.Add(myDetectableBody);

            // change the object's tags to "Dead"
            myDetectableBody.gameObject.tag = "Dead";

            for (int i = 0; i < myDetectableBody.childCount; i++)
                myDetectableBody.GetChild(i).tag = "Dead";

            enabled = false;
        }
    }
}
#endif