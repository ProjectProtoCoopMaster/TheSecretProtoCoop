
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
        [SerializeField] CallableFunction _JoinRoom;
        [SerializeField] CallableFunction _CreateRoom;
        [SerializeField] IntVariable _sceneID;
        [SerializeField] BoolVariable _isMobile;
        public VisualTreeAsset visualTree;
        public VisualElement rootElement;
        private Button join;
        private int index = -1;


        private void OnEnable()
        {
            index = 2;
            rootElement = visualTree.CloneTree();

            join = rootElement.Q<Button>("JoinButton");

            //join.clickable.clicked += () => Debug.Log("Clicked");

            rootElement.Add(join);

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

        

    }
}

