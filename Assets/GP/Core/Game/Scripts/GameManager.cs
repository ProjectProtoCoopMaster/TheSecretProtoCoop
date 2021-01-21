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

    public enum LoseType { PlayerSpottedByGuard = 0, PlayerSpottedByCam = 1, BodySpottedByCam = 2, BodySpottedByGuard = 3, PlayerHitTrap = 4 };

    public class GameManager : MonoBehaviour
    {
        public LoseType loseType { get; set; }

        public bool startGame;

        public bool gameOver { get; set; } = false;

        public Transform UICanvas;

        [Title("Lose")]
        public Transform loseCanvas;
        public StringVariable _loseText;
        [SerializeField] private Sprite[] deathIcons;

        [Title("Win")]
        public Transform winCanvas;

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

                loseCanvas.gameObject.SetActive(true);

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
    }
}
