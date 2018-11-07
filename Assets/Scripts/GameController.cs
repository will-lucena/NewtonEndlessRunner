using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using Random = UnityEngine.Random;

[Serializable]
public class PlayerData
{
    public int coinsCollected;
    public int[] questMax;
    public int[] questProgress;
    public int[] questReward;
    public int[] questCurrentProgress;
    public string[] questType;
}

public class GameController : MonoBehaviour {

    public static GameController instance;
    public int totalCoins;

    private QuestsBase[] quests;
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
        MenuController.generateNewQuest += generateQuest;
        generateQuests();
    }

    private void generateQuests()
    {
        quests = new QuestsBase[2];

        if (File.Exists(filePath))
        {
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

    private void generateQuest(int i)
    {
        Destroy(quests[i].gameObject);

        GameObject go = new GameObject("Quest" + i);
        go.transform.SetParent(transform);
        QuestsType[] types = { QuestsType.SingleRun, QuestsType.FishesSingleRun, QuestsType.TotalDistance };

        switch (types[Random.Range(0, types.Length)])
        {
            case QuestsType.SingleRun:
                var script = go.AddComponent<SingleRun>();
                script.created();
                quests[i] = script;
                break;
            case QuestsType.FishesSingleRun:
                var script1 = go.AddComponent<FishesSingleRun>();
                script1.created();
                quests[i] = script1;
                break;
            case QuestsType.TotalDistance:
                var script2 = go.AddComponent<TotalDistance>();
                script2.created();
                quests[i] = script2;
                break;
        }
    }

    public void save()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream file = File.Create(filePath);

        PlayerData dataToSave = new PlayerData();
        dataToSave.coinsCollected = totalCoins;

        dataToSave.questMax = new int[2];
        dataToSave.questCurrentProgress = new int[2];
        dataToSave.questProgress = new int[2];
        dataToSave.questReward = new int[2];
        dataToSave.questType = new string[2];

        for (int i = 0; i < quests.Length; i++)
        {
            dataToSave.questMax[i] = quests[i].max;
            dataToSave.questCurrentProgress[i] = quests[i].currentProgress;
            dataToSave.questProgress[i] = quests[i].progress;
            dataToSave.questReward[i] = quests[i].reward;
            dataToSave.questType[i] = quests[i].type.ToString();
        }

        formatter.Serialize(file, dataToSave);
        file.Close();
    }

    public void load()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream file = File.Open(filePath, FileMode.Open);

        PlayerData data = (PlayerData)formatter.Deserialize(file);
        file.Close();
        totalCoins = data.coinsCollected;

        for (int i = 0; i < data.questMax.Length; i++)
        {
            GameObject go = new GameObject("Quest" + i);
            go.transform.SetParent(transform);
            QuestsType[] types = { QuestsType.SingleRun, QuestsType.FishesSingleRun, QuestsType.TotalDistance };

            QuestsBase script;

            if (data.questType[i].Equals(QuestsType.SingleRun.ToString()))
            {
                script = go.AddComponent<SingleRun>();
                script.type = QuestsType.SingleRun;
                quests[i] = script;
            }
            else if (data.questType[i].Equals(QuestsType.TotalDistance.ToString()))
            {
                script = go.AddComponent<TotalDistance>();
                script.type = QuestsType.TotalDistance;
                quests[i] = script;
            }
            else if (data.questType[i].Equals(QuestsType.FishesSingleRun.ToString()))
            {
                script = go.AddComponent<FishesSingleRun>();
                script.type = QuestsType.FishesSingleRun;
                quests[i] = script;
            }

            quests[i].max = data.questMax[i];
            quests[i].currentProgress = data.questCurrentProgress[i];
            quests[i].progress = data.questProgress[i];
            quests[i].reward = data.questReward[i];
        }
    }

    public void loadMenu()
    {
        SceneManager.LoadScene(0);
    }

    public QuestsBase getQuest(int index)
    {
        return quests[index];
    }

    public void startQuestsCount()
    {
        for (int i = 0; i < quests.Length; i++)
        {
            quests[i].run();
        }
    }
}
