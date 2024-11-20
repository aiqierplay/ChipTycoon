using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
public class DataFieldOrderAttribute : Attribute
{
    public int Order;

    public DataFieldOrderAttribute(int order)
    {
        Order = order;
    }
}
