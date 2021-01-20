using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Gameplay
{
    public class MainMenuInterface : MonoBehaviour
    {
        public CallableFunction _JoinRoom;
        public CallableFunction _CreateRoom;

        public BoolVariable _isMobile;
        public Platform platform { get { if (_isMobile.Value) return Platform.Mobile; else return Platform.VR; } }

        public GameObject lobbyMobile;
        public GameObject lobbyVR;

        public Text codeVRComponent;
        public Text codeMobileComponent;

        private void OnEnable()
        {
            lobbyMobile.SetActive(false);
            lobbyVR.SetActive(false);
        }

        public void JoinRoom()
        {
            _JoinRoom.Raise(codeMobileComponent.text);
        }

        public void CreateRoom()
        {
            int roomName = Random.Range(0, 10);
            codeVRComponent.text = roomName.ToString();
            _CreateRoom.Raise(codeVRComponent.text);
        }

        public void OpenScene()
        {
            if (platform == Platform.Mobile) SceneManager.LoadSceneAsync("GameSceneMobile", LoadSceneMode.Additive);

            else if (platform == Platform.VR) SceneManager.LoadSceneAsync("GameSceneVR", LoadSceneMode.Additive);

            SceneManager.UnloadSceneAsync("GameSceneMainMenu");
        }

        public void OpenLobbyCanvas()
        {
            if (platform == Platform.Mobile) lobbyMobile.SetActive(true);

            else if (platform == Platform.VR) lobbyVR.SetActive(true);
        }
    }
}
