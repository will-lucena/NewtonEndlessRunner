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

[Serializable]
public abstract class Skill : MonoBehaviour
{
    public Action<float> notifyReload;

    [SerializeField] protected KeyCode skillKey;
    [SerializeField] protected new string name;
    [SerializeField] protected string description;
    [SerializeField] protected int cooldown;
    [SerializeField] protected float force;
    [SerializeField] protected SkillReloadUIEffect reloadUI;

    public abstract void activateSkill(bool boolean = false);
}

public class GameController : MonoBehaviour
{
    public Func<Obstacle, Vector3> consumeShield;
    public Action<float> increaseSpeed;
    public Action<float> updateScore;
    public Action<float> obstacleHitNotification;
    public Action<float> coinHitNotification;
    public Action showGameoverMessage;
    public Action unsubscribeMethods;
    public Action enableShield;

    public static GameController instance;
    public int totalCoins;
    public int lastRunDuration;

    private Quest[] quests;
    public List<QuestSO> availableQuests;
    public List<QuestSO> questsBase;
    private string filePath;

    private void Awake()
    {
        filePath = Application.persistentDataPath + "/gameData.data";

        if (instance == null)
        {
            instance = this;
            availableQuests = new List<QuestSO>();
        }

        if (instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            questsBase = loadAvailableQuests();
            generateQuests();
        }
        DontDestroyOnLoad(gameObject);
    }

    public Vector3 useShield(Obstacle obstacle)
    {
        return consumeShield.Invoke(obstacle);
    }

    public void activeShield()
    {
        enableShield?.Invoke();
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
        return Resources.LoadAll<QuestSO>("Quests/").ToList();
    }

    private void generateQuests()
    {
        quests = new Quest[2];
        availableQuests.AddRange(questsBase);
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

    public void generateQuest(int i)
    {
        QuestSO selectedQuest = availableQuests[Random.Range(0, availableQuests.Count)];
        QuestSO completedQuest = null;

        if (quests[i] != null)
        {
            completedQuest = questsBase.Find(item => quests[i].getId() == item.id);
        }

        quests[i] = new Quest(selectedQuest);
        availableQuests.Remove(selectedQuest);

        if (availableQuests.Count == 0)
        {
            List<QuestSO> newList = questsBase.Intersect(availableQuests).ToList();
            newList.Add(completedQuest);

            availableQuests.Clear();
            availableQuests.AddRange(newList);
            availableQuests.ForEach(item => item.totalProgress = 0);
        }
    }

    public void endGame(int coins, int progress)
    {
        totalCoins += coins;
        lastRunDuration = progress;
        save();
        showGameoverMessage?.Invoke();
    }

    public void updateSpeed(float valor)
    {
        increaseSpeed?.Invoke(valor);
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

    private void load()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream file = File.Open(filePath, FileMode.Open);

        PlayerData data = (PlayerData)formatter.Deserialize(file);
        file.Close();
        totalCoins = data.coinsCollected;

        for (int i = 0; i < data.questIds.Count; i++)
        {
            QuestSO quest = questsBase.Find(item => item.id == data.questIds[i]);
            quests[i] = new Quest(quest);
            availableQuests.Remove(quest);
        }
        availableQuests.RemoveAll(item => item.isCompleted());
    }

    public void loadMenu()
    {
        unsubscribeMethods?.Invoke();
        SceneManager.LoadScene(0);
    }

    public Quest getQuest(int index)
    {
        return quests[index];
    }
}
