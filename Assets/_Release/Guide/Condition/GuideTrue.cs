using System;

[Serializable]
public class GuideTrue : GuideCondition
{
    public override bool Check()
    {
        return true;
    }
}
