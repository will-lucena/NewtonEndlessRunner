using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public TextMeshProUGUI[] descriptionLabels;
    public TextMeshProUGUI[] progressLabels;
    public TextMeshProUGUI[] rewardLabels;
    public GameObject[] rewardButtons;
    public TextMeshProUGUI totalCoinsLabel;

    private void Start()
    {
        updateQuestsInfos();
    }

    public void loadGame()
    {
        SceneManager.LoadScene(1);
    }

    private void updateCoinsLabel(int amount)
    {
        totalCoinsLabel.SetText(amount.ToString());
    }

    private void updateQuestsInfos()
    {
        for (int i = 0; i < descriptionLabels.Length; i++)
        {
            
            Quest quest = GameController.instance.getQuest(i);
            descriptionLabels[i].SetText(quest.getDescription());
            progressLabels[i].SetText(string.Format("Progress: {0}/{1}", quest.getProgress(), quest.getGoal()));
            rewardLabels[i].SetText(string.Format("Reward: {0}", quest.getReward()));

            if (quest.isMissionFinished())
            {
                rewardButtons[i].SetActive(true);
            }
        }
        GameController.instance.save();
    }

    public void getReward(int index)
    {
        GameController.instance.totalCoins += GameController.instance.getQuest(index).getReward();
        updateCoinsLabel(GameController.instance.totalCoins);
        rewardButtons[index].SetActive(false);
        GameController.instance.generateQuest(index);
        updateQuestsInfos();
    }
}
