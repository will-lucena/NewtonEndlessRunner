using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {

    public static GameController instance;

    private QuestsBase[] quests;

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
        generateQuests();
    }

    private void generateQuests()
    {
        quests = new QuestsBase[2];
        for (int i = 0; i < quests.Length; i++)
        {
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
