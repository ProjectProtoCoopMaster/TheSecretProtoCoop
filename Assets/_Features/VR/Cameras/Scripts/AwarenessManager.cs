using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace Gameplay.VR
{
    public struct OverwatchEntityInfo
    {
        public int id;
        public string objectName;
        public Transform transform;
        public float rangeOfVision;
        public float coneOfVision;
    }

    public class AwarenessManager : MonoBehaviour
    {
        [SerializeField] [FoldoutGroup("Alarm Raising")] internal List<EntityVisionDataInterface> alarmRaisers = new List<EntityVisionDataInterface>();

        [SerializeField] internal List<GameObject> deadGuards = new List<GameObject>();
        [SerializeField] [FoldoutGroup("Alarm Raising")] float alarmRaiseDuration;
        [SerializeField][FoldoutGroup("Debugging")] float timePassed = 0f;

        [SerializeField] [Tooltip("Time will slow down by x amount when the player is detected.")] [FoldoutGroup("Slow Motion")] float reflexModeMultiplier;

        [SerializeField] [FoldoutGroup("Alarm Raising")] bool raisingAlarm = false;
        [SerializeField] [FoldoutGroup("Alarm Raising")] CallableFunction gameOver;

        private void Awake()
        {

        }

        // called by Detection Behaviour
        internal void RaiseAlarm(EntityVisionDataInterface alarmRaiser)
        {
            alarmRaisers.Add(alarmRaiser);

            if (raisingAlarm != true)
            {
                raisingAlarm = true;
                Time.timeScale /= reflexModeMultiplier;
            }
            else return;
        }

        private void Update()
        {
            if (raisingAlarm)
                timePassed += Time.unscaledDeltaTime;

            if (raisingAlarm && alarmRaisers.Count == 0 || timePassed >= alarmRaiseDuration)
            {
                // if there are still entities raising the alarm, it's game over
                if(alarmRaisers.Count > 0 ) gameOver.Raise();

                // otherwise, set the world back in order
                Time.timeScale *= reflexModeMultiplier;
                raisingAlarm = false;
                timePassed = 0f;
            }

        }

        /*
        public List<OverwatchBehavior> overwatchBehaviors = new List<OverwatchBehavior>();
        public List<OverwatchEntityInfo> overwatchEntities = new List<OverwatchEntityInfo>();
        float distanceToFriendly;
        Vector3 targetDir;

        private void Awake()
        {
            overwatchBehaviors.AddRange(FindObjectsOfType<OverwatchBehavior>());

            for (int i = 0; i < overwatchBehaviors.Count; i++)
            {
                OverwatchEntityInfo info = new OverwatchEntityInfo();
                info.id = i;
                info.objectName = overwatchBehaviors[i].gameObject.name;
                info.transform = overwatchBehaviors[i].transform;
                info.coneOfVision = overwatchBehaviors[i].coneOfVision;
                info.rangeOfVision = overwatchBehaviors[i].rangeOfVision;

                overwatchEntities.Add(info);
            }

            Debug.Log("Managing " + overwatchEntities.Count + " entities");
        }

        private void Update()
        {
            frames++;
            if (frames % 2 == 0)
            {
                frames = 0;

                // checks each tntity to see if a friendly is in range
                for (int i = 0; i < overwatchEntities.Count; i++)
                {
                    for (int j = 0; j < overwatchEntities.Count; j++)
                    {
                        if (overwatchEntities[i].id != overwatchEntities[j].id)
                        {
                            //distanceToFriendly = (overwatchEntities[j].transform.position - overwatchEntities[i].transform.position).sqrMagnitude;

                            if (Vector3.Distance(overwatchEntities[i].transform.position, overwatchEntities[j].transform.position) < overwatchEntities[i].rangeOfVision)
                            {
                                // get the direction of the player's head...
                                targetDir = overwatchEntities[j].transform.position - overwatchEntities[i].transform.position;

                                //...if the angle between the looking dir of the cam and the player is less than the cone of vision, then you are inside the cone of vision
                                if (Vector3.Angle(targetDir, overwatchEntities[i].transform.forward) <= overwatchEntities[i].coneOfVision)
                                    Debug.Log(overwatchEntities[i].objectName + " can see " + overwatchEntities[j].objectName);
                            }
                        }
                    }
                }
            }
        }*/

        #region Shit
        /*
    float time;
    bool killedAlarmRaiser;
    public float timeToAlarm;
    public CallableFunction sendGameOver;
    private int frames;

    public void GE_GuardRaisingAlarm()
    {
        // TODO : Implement a progressive spotting mechanic, based on distance
        killedAlarmRaiser = false;
        StartCoroutine(AlarmRaising());
    }

    // interrupt the alarm
    public void GE_KillAlarmRaiser()
    {
        //killedAlarmRaiser = true;
    }

    // instant gameOver
    public void GE_CameraRaisedAlarm()
    {
        // TODO : Implement a progressive spotting mechanic, based on distance
        sendGameOver.Raise();
    }

    // countdown to raise the alarm, can be interrupted
    private IEnumerator AlarmRaising()
    {
        time = 0f;
        Debug.Log("Raising the Alarm !");

        /*while (time < timeToAlarm)
        {
            time += Time.deltaTime;
            if (killedAlarmRaiser)
            {
                Debug.Log("Alarm was interrupted");
                break;
            }

            // if enough time passes uninterrupted, the gameOver event is raised
            if (time >= timeToAlarm) 

            yield return null;
        }
        sendGameOver.Raise();

        yield return null;
    }*/
        #endregion

        public void GE_LoadNewLevel()
        {
            deadGuards = new List<GameObject>();
        }
    }
}