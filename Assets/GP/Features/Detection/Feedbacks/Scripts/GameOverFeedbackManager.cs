#if UNITY_STANDALONE
using Networking;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.VR
{
    public class GameOverFeedbackManager : MonoBehaviour
    {
        [SerializeField] Light redAlarmLight;
        [SerializeField] Vector3Variable playerRigPosition;

        private void Awake()
        {
            GE_RefreshScene();
        }

        public void GE_TurnOnAlarmLights()
        {
            redAlarmLight.transform.position = playerRigPosition.Value;
            redAlarmLight.gameObject.SetActive(true);
        }

        public void GE_PlayerHitTrap()
        {
            Debug.Log((int)LoseType.PlayerHitTrap);
            TransmitterManager.instance.SendLoseToAll((int)LoseType.PlayerHitTrap);
        }

        public void GE_RefreshScene()
        {
            redAlarmLight.gameObject.SetActive(false);
        }
    }
}
#endif