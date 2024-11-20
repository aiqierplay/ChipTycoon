#if CrazyLab
using System;
using Aya.Analysis;
using UnityEngine;
using Tabtale.TTPlugins;
using System.Collections.Generic;

public class AnalysicCrazyLab : AnalysisBase
{
    public override void LevelStart(string level)
    {
        try
        {
            var levelId = int.Parse(level);
            if (levelId == 1)
            {
                var parameters = new Dictionary<string, object> { { "missionName", "First Level" } };
                TTPGameProgression.FirebaseEvents.MissionStarted(1, parameters);
            }
            else
            {
                TTPGameProgression.FirebaseEvents.MissionStarted(levelId, null);
            }
        }
        catch (Exception e)
        {
            Debug.LogError(e);
        }
    }

    public override void LevelCompleted(string level)
    {
        try
        {
            var levelId = int.Parse(level);
            TTPGameProgression.FirebaseEvents.MissionComplete(null);
        }
        catch (Exception e)
        {
            Debug.LogError(e);
        }
    }

    public override void LevelFailed(string level)
    {
        try
        {
            var levelId = int.Parse(level);
            TTPGameProgression.FirebaseEvents.MissionFailed(null);
        }
        catch (Exception e)
        {
            Debug.LogError(e);
        }
    }
}
#endif