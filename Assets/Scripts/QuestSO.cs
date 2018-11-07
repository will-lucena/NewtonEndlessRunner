using UnityEngine;

[CreateAssetMenu(fileName = "New quest", menuName = "Quest")]
public class QuestSO : ScriptableObject
{
    public int id;
    public int goal;
    public int reward;
    public int totalProgress;
    public QuestType type;

    public string description()
    {
        switch (type)
        {
            case QuestType.SingleRun:
                return string.Format("Corra {0}m em uma corrida", goal);
            case QuestType.TotalDistance:
                return string.Format("Corra {0}m no total", goal);
            case QuestType.FishesSingleRun:
                return string.Format("Colete {0} peixes em uma corrida", goal);
            default:
                return "No description";
        }
    }
    
    public bool isCompleted(int progress = 0)
    {
        return progress + totalProgress >= goal;
    }
}
