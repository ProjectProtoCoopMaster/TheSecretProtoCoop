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
            PlayerSpottedByGuard = 0,
            PlayerSpottedByCam = 1,
            BodySpottedByCam = 2,
            BodySpottedByGuard = 3,
            PlayerHitTrap = 4,
            FailSymbols = 5,
        };
        [HideInInspector]
        public LoseType loseType;



        [SerializeField] private bool startGame;
        [SerializeField] private bool launchOneScene;
        [SerializeField] private GameEvent _onLose;
        [SerializeField] private IntVariable _sceneID;
        [SerializeField] private BoolVariable _isMobile;
        [SerializeField] private Sprite[] deathIcons;
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

            if(launchOneScene) SceneManager.LoadSceneAsync(1, LoadSceneMode.Additive);
        }

        public void RaiseOnLose(int ID) {  loseType = (LoseType)ID; _onLose.Raise(); }

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
                        loseText = loseCanvas.transform.Find("ExplanationText").GetComponent<Text>();
                        loseText.text = loseTextVR.Value = "You were spotted by a Guard";
                        loseCanvas.transform.Find("DeathIcon").GetComponent<Image>().overrideSprite = deathIcons[0];
                        break;
                    case LoseType.PlayerSpottedByCam:
                        loseText = loseCanvas.transform.Find("ExplanationText").GetComponent<Text>();
                        loseText.text = loseTextVR.Value = "You were spotted by a Cam";
                        loseCanvas.transform.Find("DeathIcon").GetComponent<Image>().overrideSprite = deathIcons[1];
                        break;
                    case LoseType.BodySpottedByCam:
                        loseText = loseCanvas.transform.Find("ExplanationText").GetComponent<Text>();
                        loseText.text = loseTextVR.Value = "A body was spotted by a Cam";
                        loseCanvas.transform.Find("DeathIcon").GetComponent<Image>().overrideSprite = deathIcons[2];
                        break;
                    case LoseType.BodySpottedByGuard:
                        loseText = loseCanvas.transform.Find("ExplanationText").GetComponent<Text>();
                        loseText.text = loseTextVR.Value = "A body was spotted by a Guard";
                        loseCanvas.transform.Find("DeathIcon").GetComponent<Image>().overrideSprite = deathIcons[3];
                        break;
                    case LoseType.PlayerHitTrap:
                        loseText = loseCanvas.transform.Find("ExplanationText").GetComponent<Text>();
                        loseText.text = loseTextVR.Value = "You died in a Trap";
                        loseCanvas.transform.Find("DeathIcon").GetComponent<Image>().overrideSprite = deathIcons[4];
                        break;
                    case LoseType.FailSymbols:
                        loseText = loseCanvas.transform.Find("ExplanationText").GetComponent<Text>();
                        loseText.text = loseTextVR.Value = "You failed to enter the right Code";
                        loseCanvas.transform.Find("DeathIcon").GetComponent<Image>().overrideSprite = deathIcons[5];
                        break;
                }
                if (_isMobile.Value)
                {
                    
                }

                

                //StartCoroutine(WaitGameOver());
            }
        }

        public void Victory()
        {
            if (_isMobile.Value)
            {
                Instantiate(Resources.Load("Victory_Canvas") as GameObject);
            }

        }


        //IEnumerator WaitGameOver()
        //{
        //    yield return new WaitForSeconds(3);
        //    Destroy(loseCanvas);
        //    StartCoroutine(WaitSceneDestruction()); ;

        //    yield return new WaitForSeconds(.5f);
        //    isGameOver = false;
        //    yield break;
        //}

        IEnumerator WaitSceneDestruction()
        {
            yield return new WaitUntil(() => SceneManager.UnloadScene(_sceneID.Value));
            SceneManager.LoadSceneAsync(_sceneID.Value, LoadSceneMode.Additive);

            yield return new WaitForSeconds(2f);

            onRefreshScene.Raise();
            yield break;
        }

        IEnumerator WaitLoadNextScene()
        {
            SceneManager.UnloadSceneAsync(_sceneID.Value);
            yield return new WaitForEndOfFrame();
            _sceneID.Value += 2;
            SceneManager.LoadScene(_sceneID.Value, LoadSceneMode.Additive);
            onRefreshScene.Raise();
            yield break;
        }

        public void LoadSameScene()
        {
            /*if(_isMobile.Value)*/ Destroy(loseCanvas);
            StartCoroutine(WaitSceneDestruction()); ;
            isGameOver = false;
        }
        public void LoadNextScene()
        {
            StartCoroutine(WaitLoadNextScene());

        }

        

    }
}

