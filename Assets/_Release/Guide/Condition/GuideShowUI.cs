using System;

[Serializable]
public class GuideShowUI : GuideCondition
{
    public string Name;

    public override bool Check()
    {
        if (UIController.Ins == null) return false;
        return UIController.Ins.Current.GetType().Name == Name;
    }
}