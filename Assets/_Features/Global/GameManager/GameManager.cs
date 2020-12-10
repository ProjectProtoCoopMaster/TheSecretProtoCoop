using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Sirenix.OdinInspector;
namespace Gameplay
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private bool startGame;
        [SerializeField] private GameEvent _onLose;
        [SerializeField] private IntVariable _sceneID;
        private GameObject loseCanvas;
        private bool isGameOver = false;
        void Start()
        {
            if(startGame)
                SceneManager.LoadScene(6, LoadSceneMode.Additive);
        }
        public void RaiseOnLose()=> _onLose.Raise();
        [Button]
        public void GameOver()
        {
            if (!isGameOver)
            {
                loseCanvas = Instantiate(Resources.Load("Lose_Canvas") as GameObject);
                StartCoroutine(WaitGameOver());
                isGameOver = true;
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

