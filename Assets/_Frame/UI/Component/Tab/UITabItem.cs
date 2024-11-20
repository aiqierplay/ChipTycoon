using System;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class UITabItem
{
    public int Index { get; set; }
    public GameObject TabButton;
    public GameObject TabPanel;
    public UnityEvent OnTabSelect;

    public Action OnTabSelectAction = delegate { };
    public Action OnTabDeSelectAction = delegate { };
}