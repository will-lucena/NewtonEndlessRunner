using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour {

    public Image[] lifeHearts;
    public TextMeshProUGUI coinsLabel;
    public GameObject gameoverPanel;
    public TextMeshProUGUI scoreLabel;

    private PlayerMovement playerScript;

    private void Start()
    {
        playerScript = FindObjectOfType<PlayerMovement>();
        playerScript.coinHitNotification += updateCoins;
        playerScript.obstacleHitNotification += updateLives;
        playerScript.updateScore += updateScore;
        playerScript.endGame += endGame;
    }

    public void updateLives(int currentLifes)
    {
        for (int i = 0; i < lifeHearts.Length; i++)
        {
            if (currentLifes > i)
            {
                lifeHearts[i].color = Color.white;
            }
            else
            {
                lifeHearts[i].color = Color.clear;
            }
        }
    }

    public void updateCoins(int amount)
    {
        coinsLabel.SetText(string.Format("x{0}", amount));
    }

    private void endGame()
    {
        gameoverPanel.SetActive(true);
        Invoke("loadMenuScene", 2f);
        
    }

    private void loadMenuScene()
    {
        GameController.instance.loadMenu();
    }

    private void updateScore(int score)
    {
        scoreLabel.SetText(string.Format("Score: {0}m", score));
    }

}
