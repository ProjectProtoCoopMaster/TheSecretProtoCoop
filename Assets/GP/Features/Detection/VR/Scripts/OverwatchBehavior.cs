#if UNITY_STANDALONE
using UnityEngine;

namespace Gameplay.VR
{
    public class OverwatchBehavior : VisionBehavior
    {
        [SerializeField] internal Transform myDetectableBody;

        public override void Ping()
        {
            for (int i = 0; i < alertManager.deadGuards.Count; i++)
            {
                if (CanSeeTarget(alertManager.deadGuards[i].position))
                {
                    detected = true;

                    detectionFeedback.PlayDetectionFeedback();

                    if (!alertManager.alarmRaisers.Contains(this.gameObject))
                        alertManager.alarmRaisers.Add(this.gameObject);

                    if (entityType == EntityType.Guard)
                    {
                        alertManager.loseType = LoseType.BodySpottedByGuard;
                        alertManager.Alert();
                    }

                    else if (entityType == EntityType.Camera)
                    {
                        alertManager.loseType = LoseType.BodySpottedByCam;
                        alertManager.Detected();
                    }
                }
                else continue;
            }
        }

        //called by Unity Event when the guard is killed
        public void UE_GuardDied()
        {
            myDetectableBody = GetComponentInChildren<DetectableBodyBehaviour>().transform;

            // if you were detecting the player, remove this object from the list of alarm raisers
            if (alertManager.alarmRaisers.Contains(this.gameObject))
                alertManager.alarmRaisers.Remove(this.gameObject);

            // add this entity to the list of dead guards
            if (!alertManager.deadGuards.Contains(myDetectableBody))
                alertManager.deadGuards.Add(myDetectableBody);

            // change the object's tags to "Dead"
            myDetectableBody.gameObject.tag = "Dead";

            for (int i = 0; i < myDetectableBody.childCount; i++)
                myDetectableBody.GetChild(i).tag = "Dead";

            enabled = false;
        }
    }
}
#endif