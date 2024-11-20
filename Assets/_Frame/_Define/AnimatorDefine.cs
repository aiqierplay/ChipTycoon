using System.Collections;
using Aya.Extension;
using Sirenix.OdinInspector;

public static partial class AnimatorDefine
{
    public const string Idle = "Idle";

    public const string Walk = "Walt";
    public const string Run = "Run";
    public const string Jump = "Jump";
    public const string Drop = "Drop";

    public const string SlideStart = "SlideStart";
    public const string SlideEnd = "SlideEnd";

    public const string Attack = "Attack";
    public const string Hit = "Hit";

    public const string Win = "Win";
    public const string Lose = "Lose";
    public const string Die = "Die";

    #region Editor

    public static IEnumerable GetDefineKeys()
    {
        var dropdown = new ValueDropdownList<string>();
        foreach (var fieldInfo in typeof(AnimatorDefine).GetFields())
        {
            var key = fieldInfo.Name;
            var value = fieldInfo.GetValue(null).CastType<string>();
            dropdown.Add(key, value);
        }

        return dropdown;
    }

    #endregion
}