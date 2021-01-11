using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Gameplay.VR.UI
{
    public class UI_GameOverBehaviour : MonoBehaviour
    {
        [SerializeField] TMP_Text gameOverText;
        [SerializeField] StringVariable gameOverString;

        private void Awake()
        {
            GE_RefreshScene();
        }

        public void GE_GameOver()
        {
            gameOverText.enabled = true;
            gameOverText.text = gameOverString.Value;
        }

        public void GE_RefreshScene()
        {
            gameOverText.enabled = false;
        }
    }
}