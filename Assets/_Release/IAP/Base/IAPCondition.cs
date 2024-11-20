using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public abstract class IAPCondition
{
    public abstract bool Check();
}
