using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MenuController : MonoBehaviour {

    public TextMeshProUGUI[] descriptionLabels;
    public TextMeshProUGUI[] progressLabels;
    public TextMeshProUGUI[] rewardLabels;
    public GameObject[] rewardButtons;

    private void Start()
    {
        for (int i = 0; i < descriptionLabels.Length; i++)
        {
            QuestsBase quest = GameController.instance.getQuest(i);
            descriptionLabels[i].SetText(quest.getDescription());
            progressLabels[i].SetText(string.Format("Progress: {0}/{1}", quest.progress, quest.max));
            rewardLabels[i].SetText(string.Format("Reward: {0}", quest.reward));

            if (quest.isMissionFinished())
            {
                rewardButtons[i].SetActive(true);
            }
        }
        GameController.instance.save();
    }

    public void loadGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }
}
