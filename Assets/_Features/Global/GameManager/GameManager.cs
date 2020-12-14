using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Sirenix.OdinInspector;
using UnityEngine.UI;

namespace Gameplay
{
    public class GameManager : MonoBehaviour
    {
        public CallableFunction sendGameOver;
        [System.Serializable]
        public enum LoseType
        {
            PlayerSpottedByGuard = 0,
            PlayerSpottedByCam = 1,
            BodySpottedByCam = 2,
            BodySpottedByGuard = 3,
            PlayerHitTrap = 4
        };
        [HideInInspector]
        public LoseType loseType;



        [SerializeField] private bool startGame;
        [SerializeField] private GameEvent _onLose;
        [SerializeField] private IntVariable _sceneID;
        private GameObject loseCanvas;
        // [SerializeField] GameObjectVariable loseTextVRObj;
        private Text loseText;
        [SerializeField] StringVariable loseTextVR;
        private bool isGameOver = false;

        [SerializeField] private GameEvent onRefreshScene;

        void Start()
        {
            if (startGame)
                SceneManager.LoadScene("MainMenu", LoadSceneMode.Additive);
        }

        public void RaiseOnLose(int ID) { Debug.Log((LoseType)ID); loseType = (LoseType)ID; _onLose.Raise(); }

        [Button]
        public void GameOver()
        {
            if (!isGameOver)
            {
                isGameOver = true;
                loseCanvas = Instantiate(Resources.Load("Lose_Canvas") as GameObject);

                switch (loseType)
                {
                    case LoseType.PlayerSpottedByGuard:
                        loseText = loseCanvas.GetComponentInChildren(typeof(Text)) as Text;
                        loseText.text = loseTextVR.Value = "You were spotted by a Guard";
                        break;
                    case LoseType.PlayerSpottedByCam:
                        loseText = loseCanvas.GetComponentInChildren(typeof(Text)) as Text;
                        loseText.text = loseTextVR.Value = "You were spotted by a Camera";
                        break;
                    case LoseType.BodySpottedByCam:
                        loseText = loseCanvas.GetComponentInChildren(typeof(Text)) as Text;
                        loseText.text = loseTextVR.Value = "A dead body was spotted by a Camera";
                        break;
                    case LoseType.BodySpottedByGuard:
                        loseText = loseCanvas.GetComponentInChildren(typeof(Text)) as Text;
                        loseText.text = loseTextVR.Value = "A dead body was spotted by a Guard";
                        break;
                    case LoseType.PlayerHitTrap:
                        loseText = loseCanvas.GetComponentInChildren(typeof(Text)) as Text;
                        loseText.text = loseTextVR.Value = "You ran into a Hidden Trap !";
                        break;
                }

                StartCoroutine(WaitGameOver());
            }
        }


        IEnumerator WaitGameOver()
        {
            yield return new WaitForSeconds(3);
            Destroy(loseCanvas);
            StartCoroutine(WaitSceneDestruction()); ;
            yield return new WaitForSeconds(.5f);
            isGameOver = false;
            yield break;
        }

        IEnumerator WaitSceneDestruction()
        {
            yield return new WaitUntil(() => SceneManager.UnloadScene(_sceneID.Value));
            SceneManager.LoadScene(_sceneID.Value, LoadSceneMode.Additive);
            onRefreshScene.Raise();
        }
    }
}

