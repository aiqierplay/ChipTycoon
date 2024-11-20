using Aya.Data.Persistent;

public class SaveState
{
    public static bool GetBool(string key, bool defaultState)
    {
        return USave.GetValue($"{nameof(SaveState)}_{key}", defaultState);
    }

    public static void SetBool(string key, bool value)
    {
        USave.SetValue($"{nameof(SaveState)}_{key}", value);
        USave.Save();
    }

    public static float GetFloat(string key, float defaultValue)
    {
        return USave.GetValue($"{nameof(SaveState)}_{key}", defaultValue);
    }

    public static void SetFloat(string key, float value)
    {
        USave.SetValue($"{nameof(SaveState)}_{key}", value);
        USave.Save();
    }

    public static float GetInt(string key, int defaultValue)
    {
        return USave.GetValue($"{nameof(SaveState)}_{key}", defaultValue);
    }

    public static void SetInt(string key, int value)
    {
        USave.SetValue($"{nameof(SaveState)}_{key}", value);
        USave.Save();
    }
}