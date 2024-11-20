using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public abstract class EntityProperty
{
    public abstract object GetValue();
}