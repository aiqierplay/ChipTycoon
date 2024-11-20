using System;
using Aya.Extension;
using UnityEngine;

public abstract class SceneScrollItem : EntityBase
{
    [NonSerialized] public int Index;
    [NonSerialized] public SceneScrollList ScrollList;
    [NonSerialized] public Vector3 TargetScrollPos;

    public abstract Vector3 GetSize();

    public virtual float GetSortSize()
    {
        switch (ScrollList.SceneDirection)
        {
            case SceneScrollDirection.X: return GetSize().x;
            case SceneScrollDirection.Y: return GetSize().y;
            case SceneScrollDirection.Z: return GetSize().z;
        }

        return GetSize().x;
    }

    public virtual void SetSortPos(float pos, bool immediately = false)
    {
        switch (ScrollList.SceneDirection)
        {
            case SceneScrollDirection.X:
                TargetScrollPos.x = pos;
                if (immediately) Trans.SetLocalPositionX(pos);
                break;
            case SceneScrollDirection.Y:
                TargetScrollPos.y = pos;
                if (immediately) Trans.SetLocalPositionY(pos);
                break;
            case SceneScrollDirection.Z:
                TargetScrollPos.z = pos;
                if (immediately) Trans.SetLocalPositionZ(pos);
                break;
        }
    }

    public virtual void Init(SceneScrollList scrollList)
    {
        ScrollList = scrollList;
    }

    public virtual void OnSelect()
    {

    }

    public virtual void LateUpdate()
    {
        UpdateScrollPos();
    }

    public virtual void UpdateScrollPos()
    {
        LocalPosition = Vector3.Lerp(LocalPosition, TargetScrollPos, ScrollList.StopSpeed * DeltaTime);
    }
}