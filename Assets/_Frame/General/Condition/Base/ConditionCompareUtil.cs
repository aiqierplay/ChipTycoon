using Sirenix.OdinInspector;
using UnityEngine;

public enum ConditionCompareType
{
    [LabelText("�� Greater")] Greater = 0,
    [LabelText("�� Greater Equal")] GreaterEqual = 1,
    [LabelText("�� Equal")] Equal = 2,
    [LabelText("�� Less Equal")] LessEqual = 3,
    [LabelText("�� Less")] Less = 4,
}

public static class ConditionCompareUtil
{
    public static bool Compare(float srcValue, float compareValue, ConditionCompareType type)
    {
        switch (type)
        {
            case ConditionCompareType.Greater:
                return srcValue > compareValue;
            case ConditionCompareType.GreaterEqual:
                return srcValue >= compareValue;
            case ConditionCompareType.Equal:
                return Mathf.Abs(srcValue - compareValue) < 1e-6f;
            case ConditionCompareType.LessEqual:
                return srcValue <= compareValue;
            case ConditionCompareType.Less:
                return srcValue < compareValue;
        }

        return false;
    }
}
