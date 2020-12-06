using UnityEngine;

namespace Gameplay.VR
{
    public class GameOverFeedbackManager : MonoBehaviour
    {
        [SerializeField] internal Light redAlarmLight;

        private void Awake()
        {
            redAlarmLight.gameObject.SetActive(false);
        }

        public void GE_TurnOnAlarmLights()
        {
            redAlarmLight.gameObject.SetActive(true);
        }
    } 
}
