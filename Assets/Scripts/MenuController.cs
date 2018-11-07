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

    public void goToGame()
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

            if (quest.isCompleted())
            {
                rewardButtons[i].SetActive(true);
            }
        }
        updateCoinsLabel(GameController.instance.totalCoins);
    }

    public void getReward(int index)
    {
        GameController.instance.totalCoins += GameController.instance.getQuest(index).getReward();
        rewardButtons[index].SetActive(false);
        GameController.instance.generateQuest(index);
        GameController.instance.save();
        updateQuestsInfos();
    }
}
