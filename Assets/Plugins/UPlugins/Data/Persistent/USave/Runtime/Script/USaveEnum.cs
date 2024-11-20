
namespace Aya.Data.Persistent
{
    public enum USaveMode
    {
        Single = 0,
        Multi = 1,
    }

    public enum USaveFormat
    {
        Json = 0,
        PlayerPrefs = 1,
    }

    public enum USaveAutoMode
    {
        Manual = 0,
        PauseQuit = 1,
        Interval = 2,
    }
}