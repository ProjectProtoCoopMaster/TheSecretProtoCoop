using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Networking;
using Sirenix.OdinInspector;

namespace Gameplay
{
    public enum Platform { Mobile, VR }

    public enum LoseType { PlayerSpottedByGuard = 0, PlayerSpottedByCam = 1, BodySpottedByCam = 2, BodySpottedByGuard = 3, PlayerHitTrap = 4, MissSymbols = 5 }

    public class GameManager : MonoBehaviour
    {
        public bool startGame;

        public int currentLevelIndex { get; set; }
        private string currentScene;

        public bool gameOver { get; set; } = false;

        public BoolVariable _isMobile;

        public Transform UICanvas;

        [SerializeField] private CallableFunction _fadeTransition;
        [SerializeField] private CallableFunction _unfadeTransition;

        [Title("Lose")]
        public Transform loseCanvas;
        public StringVariable _loseText;
        [SerializeField] private Sprite[] deathIcons;

        [Title("Win")]
        public Transform winCanvas;

        public static GameManager instance;
        void OnEnable() { if (instance == null) instance = this; }

        void Start()
        {
            if (startGame)
            {
                currentScene = null;
                LoadMainMenu();
            }
        }

        [Button]
        public void Lose(LoseType loseType)
        {
            if (!gameOver)
            {
                gameOver = true;

                if (_isMobile.Value) loseCanvas.gameObject.SetActive(true);

                Text loseText = loseCanvas.Find("ExplanationText").GetComponentInChildren<Text>();

                switch (loseType)
                {
                    case LoseType.PlayerSpottedByGuard: loseText.text = _loseText.Value = "You were spotted by a Guard";
                        loseCanvas.transform.Find("DeathIcon").GetComponent<Image>().overrideSprite = deathIcons[0];
                        break;
                    case LoseType.PlayerSpottedByCam: loseText.text = _loseText.Value = "You were spotted by a Camera";
                        loseCanvas.transform.Find("DeathIcon").GetComponent<Image>().overrideSprite = deathIcons[1];
                        break;

                    case LoseType.BodySpottedByCam: loseText.text = _loseText.Value = "A dead body was spotted by a Camera";
                        loseCanvas.transform.Find("DeathIcon").GetComponent<Image>().overrideSprite = deathIcons[2];
                        break;
                    case LoseType.BodySpottedByGuard: loseText.text = _loseText.Value = "A dead body was spotted by a Guard";
                        loseCanvas.transform.Find("DeathIcon").GetComponent<Image>().overrideSprite = deathIcons[3];
                        break;

                    case LoseType.PlayerHitTrap: loseText.text = _loseText.Value = "You ran into a Hidden Trap !";
                        loseCanvas.transform.Find("DeathIcon").GetComponent<Image>().overrideSprite = deathIcons[4];
                        break;

                    case LoseType.MissSymbols: loseText.text = _loseText.Value = "You failed to enter the right Code !";
                        loseCanvas.transform.Find("DeathIcon").GetComponent<Image>().overrideSprite = deathIcons[5];
                        break;
                }

                // Reset the symbols
                TransmitterManager.instance.symbolManager.isSymbolLoaded.Value = false;
            }
        }

        [Button]
        public void Win()
        {
            if (_isMobile.Value) winCanvas.gameObject.SetActive(true);
        }

        IEnumerator WaitLoadScene(string scene)
        {
            _fadeTransition.Raise();
            yield return new WaitForSeconds(1.0f);

            if (currentScene != null) SceneManager.UnloadSceneAsync(currentScene);

            yield return new WaitForEndOfFrame();

            SceneManager.LoadScene(scene, LoadSceneMode.Additive);
            currentScene = scene;
            
            yield return new WaitForSeconds(1.0f);
            _unfadeTransition.Raise();

            yield break;
        }

        public void LoadScene(string scene)
        {
            StartCoroutine(WaitLoadScene(scene));
        }

        public void LoadMainMenu()
        {
            winCanvas.gameObject.SetActive(false);
            loseCanvas.gameObject.SetActive(false);

            currentLevelIndex = 1;
            LoadScene("MainMenu");
        }
    }
}
