using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.VR
{
    public class GameOverFeedbackManager : MonoBehaviour
    {
        [SerializeField] internal Light redAlarmLight;
        [SerializeField] Canvas gameOverCanvas;
        Text gameOverText;

        private void Awake()
        {
            redAlarmLight.gameObject.SetActive(false);

            gameOverText = gameOverCanvas.GetComponentInChildren<Text>();
            gameOverText.enabled = false;
        }

        internal void UE_GameOverExplanation(EntityType alarmRaiser, EntityType alarmReason)
        {           
            if (alarmReason == EntityType.Player) gameOverText.text = "Alarm was raised because a " + alarmRaiser.ToString() + " saw the player";
            else gameOverText.text = "Alarm was raised because a " + alarmRaiser.ToString() + " saw a dead guard";
        }

        public void GE_TurnOnAlarmLights()
        {
            redAlarmLight.gameObject.SetActive(true);
        }
    } 
}
