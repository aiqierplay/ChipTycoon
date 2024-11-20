using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class IAPDefaultCondition : IAPCondition
{
    public override bool Check()
    {
        return true;
    }
}
