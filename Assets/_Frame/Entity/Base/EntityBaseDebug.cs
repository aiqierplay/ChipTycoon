using System;
using System.Collections.Generic;
using Aya.Test;
using Aya.UI.Markup;
using UnityEngine;

public abstract partial class EntityBase
{
    #region Try

    public void TryCatch(Action action, string message)
    {
        try
        {
            action();
        }
        catch (Exception exception)
        {
            Error(message + "\n" + exception);
        }
    }

    #endregion

    #region Log

    public virtual string LogPrefix => "[" + name + "]\t";
    public virtual Color LogColor => Color.white;
    public virtual Color WarningColor => Color.yellow;
    public virtual Color ErrorColor => Color.red;

    public static readonly List<Color> LogTextColor = new List<Color>()
    {
        new Color(1, 0.25f, 0.25f),
        new Color(1, 0.6f, 0.3f),
        new Color(1, 0.9f, 0.3f),
        new Color(0.3f, 1, 0.4f),
        new Color(0.3f, 1, 1),
        new Color(0.1f, 0.6f, 1),
        new Color(0.3f, 0.4f, 1),
        new Color(1, 0.3f, 0.8f),
        new Color(1, 0.6f, 1)
    };

    public void Log(params object[] logs)
    {
        var result = "";
        var colorIndex = 0;
        for (var i = 0; i < logs.Length; i++)
        {
            var log = logs[i];
            if (log == null)
            {
                result += "NULL".ToMarkup(LogTextColor[colorIndex]);
            }
            else
            {
                result += log.ToString().ToMarkup(LogTextColor[colorIndex]);
            }

            if (i < logs.Length - 1) result += " ";
            colorIndex++;
            if (colorIndex >= LogTextColor.Count) colorIndex = 0;
        }

        Log(result);
    }

    public void Log(string log)
    {
        Log(true, log);
    }

    public void Log(bool condition, string log)
    {
        if (!condition) return;
        Debug.Log(LogPrefix.ToMarkup(LogColor) + log);
    }

    public void Warning(string log)
    {
        Warning(true, log);
    }

    public void Warning(bool condition, string log)
    {
        if (!condition) return;
        Debug.LogWarning(LogPrefix.ToMarkup(WarningColor) + log);
    }

    public void Error(string log)
    {
        Error(true, log);
    }

    public void Error(bool condition, string log)
    {
        if (!condition) return;
        Debug.LogError(LogPrefix.ToMarkup(ErrorColor) + log);
    }

    #endregion

    #region Test

    public void Test(Action action)
    {
        Test("Test", action);
    }

    public void Test(string key, Action action)
    {
        var timeStart = Time.realtimeSinceStartup;
        action?.Invoke();
        var timeEnd = Time.realtimeSinceStartup;
        var duration = (timeEnd - timeStart) * 1000;
        Log(key, duration);
    }

    #endregion
}
