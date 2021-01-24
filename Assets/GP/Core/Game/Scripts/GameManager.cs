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

    public enum LoseType { PlayerSpottedByGuard = 0, PlayerSpottedByCam = 1, BodySpottedByCam = 2, BodySpottedByGuard = 3, PlayerHitTrap = 4, MissSymbols = 5 };

    public class GameManager : MonoBehaviour
    {
        public LoseType loseType { get; set; }

        public bool startGame;

        public int currentLevelIndex { get; set; }
        [SerializeField] private IntVariable _sceneID; public IntVariable SceneID { get => _sceneID; set => _sceneID = value; }

        public bool gameOver { get; set; } = false;

        public BoolVariable _isMobile;

        public Transform UICanvas;

        [SerializeField] private CallableFunction _fadeTransition;
        [SerializeField] private GameEvent _refreshScene;

        [Title("Lose")]
        public Transform loseCanvas;
        public StringVariable _loseText;
        [SerializeField] private Sprite[] deathIcons;

        [Title("Win")]
        public Transform winCanvas;

        public static GameManager instance;

        private void OnEnable()
        {
            if (instance == null) instance = this;
        }

        void Start()
        {
            winCanvas.gameObject.SetActive(false);
            loseCanvas.gameObject.SetActive(false);

            if (startGame) SceneManager.LoadScene(1, LoadSceneMode.Additive);
        }

        [Button]
        public void Lose()
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
                    case LoseType.MissSymbols:
                        loseText.text = _loseText.Value = "You failed to enter the right Code !";
                        loseCanvas.transform.Find("DeathIcon").GetComponent<Image>().overrideSprite = deathIcons[5];
                        break;
                }

                loseCanvas.GetComponentInChildren<Button>().onClick.AddListener(delegate { Restart(); });
            }
        }

        public void Win()
        {
            winCanvas.gameObject.SetActive(true);
        }

        public void Restart()
        {
            TransmitterManager.instance.SendRestartToAll();
        }

        public void LaunchSameLevel()
        {
            StartCoroutine(WaitSceneDestruction());
        }

        IEnumerator WaitSceneDestruction()
        {
            yield return new WaitUntil(() => SceneManager.UnloadScene(_sceneID.Value));
            SceneManager.LoadSceneAsync(_sceneID.Value, LoadSceneMode.Additive);
            loseCanvas.gameObject.SetActive(false);
            _refreshScene.Raise();
            yield return new WaitForSeconds(2f);
            gameOver = false;
            yield break;
        }

        IEnumerator WaitLoadNextScene(int sceneID)
        {
            _fadeTransition.Raise();
            yield return new WaitForSeconds(1.2f);
            SceneManager.UnloadSceneAsync(_sceneID.Value);
            yield return new WaitForEndOfFrame();
            _sceneID.Value = sceneID;
            SceneManager.LoadScene(_sceneID.Value, LoadSceneMode.Additive);
            yield return new WaitForSeconds(1.0f);
            _refreshScene.Raise();
            yield break;
        }

        public void LoadMainMenu()
        {
            currentLevelIndex = 0;
            StartCoroutine(WaitLoadNextScene(1));
        }

        public void LoadNextScene()
        {
            int id = _sceneID.Value + 2;
            StartCoroutine(WaitLoadNextScene(id));
        }
    }
}
