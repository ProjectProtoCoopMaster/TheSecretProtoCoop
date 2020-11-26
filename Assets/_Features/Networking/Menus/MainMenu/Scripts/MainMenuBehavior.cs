
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
        public VisualTreeAsset visualTree;
        public VisualElement rootElement;
        private Button join;

        private void OnEnable()
        {

            rootElement = visualTree.CloneTree();

            join = rootElement.Q<Button>("JoinButton");

            //join.clickable.clicked += () => Debug.Log("Clicked");

            rootElement.Add(join);


        }

        private void Test(ChangeEvent<Button> value) => Debug.Log("Helo");
        public void JoinRoom() => _JoinRoom.Raise();

        [Button]
        public void CreateRoom() => _CreateRoom.Raise();

        public void OpenScene()
        {
            if (Application.platform == RuntimePlatform.WindowsEditor)
            {
                SceneManager.LoadScene(1, LoadSceneMode.Additive);
                SceneManager.UnloadScene(3);
            }
            else
            {
                SceneManager.LoadScene(2, LoadSceneMode.Additive);
                SceneManager.UnloadScene(3);
            }
        }

        

    }
}

