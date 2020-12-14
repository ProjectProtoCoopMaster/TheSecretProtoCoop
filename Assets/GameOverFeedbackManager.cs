using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.VR
{
    public class GameOverFeedbackManager : MonoBehaviour
    {
        /*[SerializeField] GameObjectVariable playerHead;
        [SerializeField] GameObjectVariable gameOverTextObj;*/
        [SerializeField] CallableFunction gameOver;

        [SerializeField] Text gameOverText;
        [SerializeField] Light redAlarmLight;

        private void Awake()
        {
            GE_RefreshScene();
        }

        public void GE_TurnOnAlarmLights()
        {
            redAlarmLight.gameObject.SetActive(true);
        }

        public void GE_PlayerHitTrap()
        {
            Debug.Log((int)Gameplay.GameManager.LoseType.PlayerHitTrap);
            gameOver.Raise((int)Gameplay.GameManager.LoseType.PlayerHitTrap);
        }

        public void GE_GameOver()
        {
            gameOverText.enabled = true;
        }

        public void GE_RefreshScene()
        {
            redAlarmLight.gameObject.SetActive(false);
            gameOverText.enabled = false;
        }
    }
}
