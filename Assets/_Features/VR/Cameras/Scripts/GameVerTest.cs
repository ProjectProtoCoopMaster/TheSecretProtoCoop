using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Gameplay;
public class GameVerTest : MonoBehaviour
{
    public Text gameOverText;
    public CallableFunction sendGameOver;
    private bool isDead;
    private void Start()
    {
        gameOverText.gameObject.SetActive(false);
    }

    public void GE_GameOver()
    {
        if (!isDead)
        {
            Debug.Log("Game Over");
            //sendGameOver.Raise();
            isDead = true;
        }

        //ActivateUI();
    }

    private void ActivateUI()
    {
        gameOverText.gameObject.SetActive(true);
        gameOverText.text = "Game Over. You Were Spotted.";
    }
}
