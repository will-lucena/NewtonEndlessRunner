public class Quest
{
    private QuestSO data;

    public Quest(QuestSO data)
    {
        this.data = data;
    }

    public int getProgress()
    {
        return GameController.instance.lastRunDuration > data.totalProgress ? GameController.instance.lastRunDuration : data.totalProgress;
    }

    public int getGoal()
    {
        return data.goal;
    }

    public int getReward()
    {
        return data.reward;
    }

    public string getDescription()
    {
        return data.description();
    }

    public bool isCompleted()
    {
        return data.isCompleted();
    }

    public QuestType getType()
    {
        return data.type;
    }

    public int getId()
    {
        return data.id;
    }

    public void addProgress()
    {
        data.totalProgress += GameController.instance.lastRunDuration;
    }
}

public enum QuestType
{
    SingleRun,
    TotalDistance,
    FishesSingleRun
}