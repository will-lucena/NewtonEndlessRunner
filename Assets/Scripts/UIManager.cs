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
    
    private void Start()
    {
        GameController.instance.coinHitNotification += updateCoins;
        GameController.instance.obstacleHitNotification += updateLives;
        GameController.instance.updateScore += updateScore;
        GameController.instance.showGameoverMessage += endGame;
    }

    public void updateLives(float currentLifes)
    {
        for (int i = 0; i < lifeHearts.Length; i++)
        {
            if ((int)currentLifes > i)
            {
                lifeHearts[i].color = Color.white;
            }
            else
            {
                lifeHearts[i].color = Color.clear;
            }
        }
    }

    public void updateCoins(float amount)
    {
        coinsLabel.SetText(string.Format("x{0}", (int)amount));
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

    private void updateScore(float score)
    {
        scoreLabel.SetText(string.Format("Score: {0}m", (int)score));
    }

}
