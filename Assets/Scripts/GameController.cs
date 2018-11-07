using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Linq;
using Random = UnityEngine.Random;

[Serializable]
public class PlayerData
{
    public int coinsCollected;
    public List<int> questIds;

    public PlayerData()
    {
        questIds = new List<int>();
        coinsCollected = 0;
    }

}

public enum UIComponent
{
    Score,
    Heart,
    Coin
}

public class GameController : MonoBehaviour
{
    public Action<float> increaseSpeed;
    public Action<float> updateScore;
    public Action<float> obstacleHitNotification;
    public Action<float> coinHitNotification;
    public Action showGameoverMessage;

    public static GameController instance;
    public int totalCoins;

    private Quest[] quests;
    public List<QuestSO> availableQuests;
    private string filePath;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);

        filePath = Application.persistentDataPath + "/gameData.data";

        availableQuests = loadAvailableQuests();

        generateQuests();
    }

    public void updateUI(UIComponent key, float valor)
    {
        switch (key)
        {
            case UIComponent.Score:
                updateScore?.Invoke(valor);
                break;
            case UIComponent.Coin:
                coinHitNotification?.Invoke(valor);
                break;
            case UIComponent.Heart:
                obstacleHitNotification?.Invoke(valor);
                break;
        }
    }

    private List<QuestSO> loadAvailableQuests()
    {
        List<QuestSO> list = Resources.LoadAll<QuestSO>("Quests/").ToList();
        return list.FindAll(item => item.isCompleted() == false);
    }

    private void generateQuests()
    {
        quests = new Quest[2];

        if (File.Exists(filePath))
        {
            Debug.Log(filePath);
            load();
        }
        else
        {
            for (int i = 0; i < quests.Length; i++)
            {
                generateQuest(i);
            }
        }
    }

    public void generateQuest(int i)
    {
        quests[i] = new Quest(availableQuests[Random.Range(0, availableQuests.Count)]);
    }

    public void endGame(int coins, int progress)
    {
        totalCoins += coins;
        updatedQuestsProgress(progress);
        save();
        showGameoverMessage?.Invoke();
    }

    public void updateSpeed(float valor)
    {
        increaseSpeed?.Invoke(valor);
    }

    private void updatedQuestsProgress(int progress)
    {
        foreach(Quest quest in quests)
        {
            quest.setProgress(progress);
        }
    }

    public void save()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream file = File.Create(filePath);

        PlayerData data = new PlayerData();
        data.coinsCollected = totalCoins;

        for (int i = 0; i < quests.Length; i++)
        {
            if (quests[i].getType() == QuestType.TotalDistance)
            {
                quests[i].addProgress();
            }

            data.questIds.Add(quests[i].getId());
        }

        formatter.Serialize(file, data);
        file.Close();
    }

    public void load()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream file = File.Open(filePath, FileMode.Open);

        PlayerData data = (PlayerData)formatter.Deserialize(file);
        file.Close();
        totalCoins = data.coinsCollected;

        for (int i = 0; i < data.questIds.Count; i++)
        {
            quests[i] = new Quest(availableQuests.Find(item => item.id == data.questIds[i]));
        }
    }

    public void loadMenu()
    {
        SceneManager.LoadScene(0);
    }

    public Quest getQuest(int index)
    {
        return quests[index];
    }
}
