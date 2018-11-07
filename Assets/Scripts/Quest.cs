﻿using UnityEngine;

public class Quest
{
    private int currentProgress;
    private QuestSO data;

    public Quest(QuestSO data)
    {
        currentProgress = 0;
        this.data = data;
    }

    public int getProgress()
    {
        return currentProgress > data.totalProgress ? currentProgress : data.totalProgress;
    }

    public void setProgress(int progress)
    {
        currentProgress = progress;
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
        return data.isCompleted(currentProgress);
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
        Debug.Log(currentProgress);
        data.totalProgress += currentProgress;
    }
}

public enum QuestType
{
    SingleRun,
    TotalDistance,
    FishesSingleRun
}