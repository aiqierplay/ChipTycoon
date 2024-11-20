using System;
using System.Collections.Generic;
using UnityEngine;

public class GrassManager : EntityBase<GrassManager>
{
    [NonSerialized] public HashSet<GrassNode> GrassList = new HashSet<GrassNode>();
    [NonSerialized] public HashSet<GrassPlaceholder> PlaceholderList = new HashSet<GrassPlaceholder>();

    #region Grass

    public void Register(GrassNode grass)
    {
        GrassList.Add(grass);
    }

    public void DeRegister(GrassNode grass)
    {
        GrassList.Remove(grass);
    }

    #endregion

    #region Placeholder

    public void Register(GrassPlaceholder placeholder)
    {
        PlaceholderList.Add(placeholder);
        RefreshPlaceHolder();
    }

    public void DeRegister(GrassPlaceholder placeholder)
    {
        PlaceholderList.Remove(placeholder);
        RefreshPlaceHolder();
    }

    [NonSerialized] public bool NeedRefresh;
    [NonSerialized] public Vector4[] PlaceholderPositions = new Vector4[10];
    [NonSerialized] public int PlaceholderCount = 0;

    public void RequestRefreshPlaceHolder()
    {
        NeedRefresh = true;
    }

    public void RefreshPlaceHolder()
    {
        PlaceholderCount = PlaceholderList.Count;
        var index = 0;
        foreach (var placeholder in PlaceholderList)
        {
            if (index < PlaceholderCount && index < PlaceholderPositions.Length)
            {
                PlaceholderPositions[index] = (Vector4)placeholder.Position;
                index++;
            }
        }

        foreach (var grass in GrassList)
        {
            grass.Refresh(PlaceholderPositions, PlaceholderCount);
        }

        NeedRefresh = false;
    }

    #endregion

    public void LateUpdate()
    {
        if (NeedRefresh)
        {
            RefreshPlaceHolder();
        }
    }
}
