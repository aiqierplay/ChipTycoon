using System;

[Serializable]
public class GuideFalse : GuideCondition
{
    public override bool Check()
    {
        return false;
    }
}
