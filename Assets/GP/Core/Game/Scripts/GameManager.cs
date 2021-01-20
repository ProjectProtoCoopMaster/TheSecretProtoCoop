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
        public GameObject loseCanvasPrefab; public GameObject loseCanvas { get; set; }
        public StringVariable _loseText;

        [Title("Win")]
        public GameObject winCanvasPrefab;

        void Start()
        {
            if (startGame) SceneManager.LoadScene("GameSceneMainMenu", LoadSceneMode.Additive);
        }

        public void Lose()
        {
            if (!gameOver)
            {
                gameOver = true;

                loseCanvas = Instantiate(loseCanvasPrefab);
                loseCanvas.transform.parent = UICanvas;
                Text loseText = loseCanvas.GetComponentInChildren<Text>();

                switch (loseType)
                {
                    case LoseType.PlayerSpottedByGuard: loseText.text = _loseText.Value = "You were spotted by a Guard"; break;
                    case LoseType.PlayerSpottedByCam: loseText.text = _loseText.Value = "You were spotted by a Camera"; break;

                    case LoseType.BodySpottedByCam: loseText.text = _loseText.Value = "A dead body was spotted by a Camera"; break;
                    case LoseType.BodySpottedByGuard: loseText.text = _loseText.Value = "A dead body was spotted by a Guard"; break;

                    case LoseType.PlayerHitTrap: loseText.text = _loseText.Value = "You ran into a Hidden Trap !"; break;
                }

                loseCanvas.GetComponentInChildren<Button>().onClick.AddListener(delegate { Restart(); });
            }
        }

        public void Win()
        {
            GameObject winCanvas = Instantiate(winCanvasPrefab);
            winCanvas.transform.parent = UICanvas;
        }

        public void Restart()
        {
            TransmitterManager.instance.SendRestartToAll();
        }
    }
}
