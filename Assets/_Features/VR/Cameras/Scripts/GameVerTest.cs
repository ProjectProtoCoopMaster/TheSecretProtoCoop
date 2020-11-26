using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameVerTest : MonoBehaviour
{
    public Text gameOverText;

    private void Start()
    {
        gameOverText.gameObject.SetActive(false);
    }

    public void GE_GameOver()
    {
        Debug.Log("Game Over");
        ActivateUI();
    }

    private void ActivateUI()
    {
        gameOverText.gameObject.SetActive(true);
        gameOverText.text = "Game Over. You Were Spotted.";
    }
}
