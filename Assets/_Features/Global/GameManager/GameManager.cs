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
        [System.Serializable]
        public enum LoseType 
        { 
            SpottedByGuard = 0,
            SpottedByCam = 1,
            BodySpotted = 2
        };
        [HideInInspector]
        public LoseType loseType;



        [SerializeField] private bool startGame;
        [SerializeField] private GameEvent _onLose;
        [SerializeField] private IntVariable _sceneID;
        private GameObject loseCanvas;
        private Text loseText;
        private bool isGameOver = false;

        void Start()
        {

            if(startGame)
                SceneManager.LoadScene("MainMenu", LoadSceneMode.Additive);
        }
        public void RaiseOnLose(int ID) { loseType = (LoseType)ID; _onLose.Raise(); }
        [Button]
        public void GameOver()
        {
            if (!isGameOver)
            {
                isGameOver = true;
                loseCanvas = Instantiate(Resources.Load("Lose_Canvas") as GameObject);
                

                switch (loseType)
                {
                    case LoseType.SpottedByGuard:
                        loseText = loseCanvas.GetComponentInChildren(typeof(Text)) as Text;
                        loseText.text = "Spotted By A Guard";
                        break;
                    case LoseType.SpottedByCam:
                        loseText = loseCanvas.GetComponentInChildren(typeof(Text)) as Text;
                        loseText.text = "Spotted By A Camera";
                        break;
                    case LoseType.BodySpotted:
                        loseText = loseCanvas.GetComponentInChildren(typeof(Text)) as Text;
                        loseText.text = "Body Spotted";
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

        }
    }
}

