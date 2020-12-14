using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.VR
{
    public class GameOverFeedbackManager : MonoBehaviour
    {
        [SerializeField] internal Light redAlarmLight;
        [SerializeField] Canvas gameOverCanvas;
        Text gameOverText;

        CallableFunction gameOver;

        private void Awake()
        {
            gameOverCanvas = GameObject.Find("Game Over Info Canvas").GetComponent<Canvas>();
            redAlarmLight.gameObject.SetActive(false);

            gameOverText = gameOverCanvas.GetComponentInChildren<Text>();
            gameOverText.enabled = false;
        }

        public void GE_TurnOnAlarmLights()
        {
            redAlarmLight.gameObject.SetActive(true);
        }

        public void GE_PlayerHitTrap()
        {
            gameOver.Raise(Gameplay.GameManager.LoseType.PlayerHitTrap);
        }
    } 
}
