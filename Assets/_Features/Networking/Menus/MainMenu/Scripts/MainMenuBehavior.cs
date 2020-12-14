
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using Sirenix.OdinInspector;

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
        private int index = -1;
        

        private void OnEnable()
        {
            vrCanvas.enabled = false;
            mobileCanvas.enabled = false;
            index = 2;
        }

        public void SetIndex(int ID) => index = ID;
        public void JoinRoom() { _JoinRoom.Raise(); }

        [Button]
        public void CreateRoom() => _CreateRoom.Raise();

        public void OpenScene()
        {
            if(_isMobile.Value)
            {
                SceneManager.LoadScene(index + 1, LoadSceneMode.Additive);
                SceneManager.UnloadScene("MainMenu");
                _sceneID.Value = index + 1;
            }
            else
            {

                SceneManager.LoadScene(index, LoadSceneMode.Additive);
                SceneManager.UnloadScene("MainMenu");
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

