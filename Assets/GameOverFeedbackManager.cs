using UnityEngine;

namespace Gameplay.VR
{
    public class GameOverFeedbackManager : MonoBehaviour
    {
        [SerializeField] internal Light redAlarmLight;

        private void Awake()
        {
            redAlarmLight.enabled = false;
        }

        public void GE_TurnOnAlarmLights()
        {
            redAlarmLight.enabled = true;
        }
    } 
}
