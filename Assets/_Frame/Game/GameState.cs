using System;
using UnityEngine;

public class GameState : EntityBase
{
    [NonSerialized] public float StartTime;
    [NonSerialized] public float EndTime;

    public float TotalTime
    {
        get
        {
            if (EndTime < StartTime)
            {
                return Time.realtimeSinceStartup - StartTime;
            }

            return EndTime - StartTime;
        }
    }

    public void Init()
    {
        StartTime = -1;
        EndTime = -1;
    }

    public void GameStart()
    {
        State.StartTime = Time.realtimeSinceStartup;
    }

    public void GameEnd()
    {
        State.EndTime = Time.realtimeSinceStartup;
    }
}
