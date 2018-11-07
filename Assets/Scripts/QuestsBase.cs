using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class QuestsBase : MonoBehaviour
{
    public int max;
    public int progress;
    public int reward;
    public PlayerMovement player;
    public int currentProgress;

    public abstract void created();
    public abstract string getDescription();
    public abstract void run();
    public abstract void Update();

    public bool isMissionFinished()
    {
        return progress + currentProgress >= max;
    }
}

public enum QuestsType
{
    SingleRun,
    TotalDistance,
    FishesSingleRun
}

public class SingleRun : QuestsBase
{
    public override void created()
    {
        int[] values = { 1000, 2000, 3000, 4000 };
        int[] rewards = { 10, 20, 30, 40 };

        int index = Random.Range(0, values.Length);
        reward = rewards[index];
        max = values[index];
        progress = 0;
    }

    public override string getDescription()
    {
        return string.Format("Corra {0}m em uma corrida", max);
    }

    public override void run()
    {
        progress = 0;
        player = FindObjectOfType<PlayerMovement>();
    }

    public override void Update()
    {
        if (player != null)
        {
            progress = (int)player.score;
        }
    }
}

public class TotalDistance : QuestsBase
{
    public override void created()
    {
        int[] highValues = { 10000, 20000, 30000, 40000 };
        int[] hightRewards = { 100, 200, 300, 400 };

        int index = Random.Range(0, highValues.Length);
        reward = hightRewards[index];
        max = highValues[index];
        progress = 0;
    }

    public override string getDescription()
    {
        return string.Format("Corra {0}m no total", max);
    }

    public override void run()
    {
        progress += currentProgress;
        player = FindObjectOfType<PlayerMovement>();
    }

    public override void Update()
    {
        if (player != null)
        {
            currentProgress = (int)player.score;
        }
    }
}

public class FishesSingleRun : QuestsBase
{
    public override void created()
    {
        int[] values = { 100, 200, 300, 400, 500 };
        int[] rewards = { 10, 20, 30, 40, 50 };

        int index = Random.Range(0, values.Length);
        reward = rewards[index];
        max = values[index];
        progress = 0;
    }

    public override string getDescription()
    {
        return string.Format("Colete {0} peixes em uma corrida", max);
    }

    public override void run()
    {
        progress += currentProgress;
        player = FindObjectOfType<PlayerMovement>();
    }

    public override void Update()
    {
        if (player != null)
        {
            currentProgress = (int)player.coinAmount;
        }
    }
}