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
        private GameObject loseCanvas;
        private bool isGameOver = false;
        void Start()
        {
            if(startGame)
                SceneManager.LoadScene(3, LoadSceneMode.Additive);
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
            if (Application.platform == RuntimePlatform.Android)
            {
                SceneManager.UnloadSceneAsync(2);
                SceneManager.LoadScene(2,LoadSceneMode.Additive);
            }
            else
            {
                SceneManager.UnloadSceneAsync(1);
                SceneManager.LoadScene(1, LoadSceneMode.Additive);
            }
            yield return new WaitForSeconds(.1f);
            isGameOver = false;
            yield break;
        }
    }
}

