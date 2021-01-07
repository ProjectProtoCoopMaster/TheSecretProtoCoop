
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Photon.Pun;
using UnityEngine.UI;
namespace Gameplay
{
    public class MainMenuBehavior : MonoBehaviour
    {
        public Transform playerVR;
        public Vector3Variable playerVRPos;
        [SerializeField] private CallableFunction _JoinRoom;
        [SerializeField] private CallableFunction _CreateRoom;
        [SerializeField] private IntVariable _sceneID;
        [SerializeField] private BoolVariable _isMobile;
        [SerializeField] private Canvas mobileCanvas;
        [SerializeField] private Canvas vrCanvas;
        [SerializeField] private Text codeVR;
        [SerializeField] private Text codeMobile;
        private int index = -1;
        

        private void OnEnable()
        {
            vrCanvas.enabled = false;
            mobileCanvas.enabled = false;
            index = 2;
        }

        public void SetIndex(int ID) => index = ID;
        public void JoinRoom() { _JoinRoom.Raise(codeMobile.text); }

        [Button]
        public void CreateRoom()
        {
            int roomName = Random.Range(1000000, 10000000);
            codeVR.text = roomName.ToString();
            _CreateRoom.Raise(roomName.ToString());

        }

        public void OpenScene()
        {
            if(_isMobile.Value)
            {
                SceneManager.LoadSceneAsync(index + 1, LoadSceneMode.Additive);
                SceneManager.UnloadSceneAsync("MainMenu");
                _sceneID.Value = index + 1;
            }
            else
            {

                SceneManager.LoadSceneAsync(index, LoadSceneMode.Additive);
                SceneManager.UnloadSceneAsync("MainMenu");
                _sceneID.Value = index;

            }

        }

        public void OpenCanvas()
        {
            if (_isMobile.Value)
            {
                mobileCanvas.enabled = true;
            }
            else
            {
                vrCanvas.enabled = true;

            }
        }

        

    }
}

