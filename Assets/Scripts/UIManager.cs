using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

    public Image[] lifeHearts;
    private PlayerMovement playerScript;

    private void Start()
    {
        playerScript = FindObjectOfType<PlayerMovement>();
        playerScript.hitNotification += updateLives;
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
}
