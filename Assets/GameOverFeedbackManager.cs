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
            gameOverText.enabled = true;
            if (alarmReason == EntityType.Player) gameOverText.text = "GAME OVER. A " + alarmRaiser.ToString() + " spotted You";
            else if (alarmReason == EntityType.Trap) gameOverText.text = "GAME OVER. You walked into a hidden Trap";
            else gameOverText.text = "GAME OVER. A " + alarmRaiser.ToString() + " spotted a dead body";
        }

        public void GE_TurnOnAlarmLights()
        {
            redAlarmLight.gameObject.SetActive(true);
        }
    } 
}
