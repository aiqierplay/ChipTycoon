using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class ThemeAdapterManager : MonoBehaviour
{
    #region Instance

    public static ThemeAdapterManager Ins
    {
        get
        {
            if (Application.isPlaying && ApplicationIsQuitting)
            {
                return null;
            }

            if (Instance != null) return Instance;
            Instance = FindObjectOfType<ThemeAdapterManager>();
            if (Instance != null)
            {
                return Instance;
            }

            var hideFlag = HideFlags.None;
            var insName = nameof(ThemeAdapterManager);
            if (!Application.isPlaying)
            {
                insName += " (Editor)";
            }

            var obj = new GameObject
            {
                name = insName,
                hideFlags = hideFlag,
            };

            Instance = obj.AddComponent<ThemeAdapterManager>();
            if (Application.isPlaying) DontDestroyOnLoad(Instance);
            return Instance;
        }
    }

    internal static bool ApplicationIsQuitting = false;
    internal static ThemeAdapterManager Instance;

    #endregion

    [NonSerialized] public HashSet<ThemeAdapter> AdapterList = new HashSet<ThemeAdapter>();

    protected virtual void Awake()
    {
        if (!Equals(Instance, this))
        {
            Instance = this;
        }
    }

    public void Register(ThemeAdapter adapter)
    {
        AdapterList.Add(adapter);
    }

    public void DeRegister(ThemeAdapter adapter)
    {
        if (AdapterList.Contains(adapter)) AdapterList.Remove(adapter);
    }

    [Button]
    public void Apply()
    {
        foreach (var adapter in AdapterList)
        {
            adapter.Apply();
        }
    }
}