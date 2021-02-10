#if UNITY_STANDALONE
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

        bool displayLose = false;

        private void Awake()
        {
            GE_RefreshScene();
        }

        public void GE_GameOver()
        {
            gameOverText.enabled = true;
            displayLose = true;
            gameOverText.text = gameOverString.Value;
        }

        private void Update()
        {
            if(displayLose) gameOverText.text = gameOverString.Value;
        }

        public void GE_RefreshScene()
        {
            displayLose = false;
            gameOverText.enabled = false;
        }
    }
}
#endif