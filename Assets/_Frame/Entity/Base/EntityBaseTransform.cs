using System;
using System.Collections.Generic;
using UnityEngine;

public abstract partial class EntityBase
{
    #region Transform
    
    [NonSerialized] public Transform Trans;

    public Vector3 Forward
    {
        get => Trans.forward;
        set => Trans.forward = value;
    }

    public Vector3 Backward
    {
        get => -Trans.forward;
        set => Trans.forward = -value;
    }

    public Vector3 Right
    {
        get => Trans.right;
        set => Trans.right = value;
    }

    public Vector3 Left
    {
        get => -Trans.right;
        set => Trans.right = -value;
    }

    public Vector3 Up
    {
        get => Trans.up;
        set => Trans.up = value;
    }

    public Vector3 Down
    {
        get => -Trans.up;
        set => Trans.up = -value;
    }

    #region Position
   
    public Vector3 Position
    {
        get => Trans.position;
        set => Trans.position = value;
    }

    public float PositionX
    {
        get => Trans.position.x;
        set
        {
            var pos = Trans.position;
            pos.x = value;
            Trans.position = pos;
        }
    }

    public float PositionY
    {
        get => Trans.position.y;
        set
        {
            var pos = Trans.position;
            pos.y = value;
            Trans.position = pos;
        }
    }

    public float PositionZ
    {
        get => Trans.position.z;
        set
        {
            var pos = Trans.position;
            pos.z = value;
            Trans.position = pos;
        }
    }

    #endregion

    #region Local Position
    
    public Vector3 LocalPosition
    {
        get => Trans.localPosition;
        set => Trans.localPosition = value;
    }

    public float LocalPositionX
    {
        get => Trans.localPosition.x;
        set
        {
            var pos = Trans.localPosition;
            pos.x = value;
            Trans.localPosition = pos;
        }
    }

    public float LocalPositionY
    {
        get => Trans.localPosition.y;
        set
        {
            var pos = Trans.localPosition;
            pos.y = value;
            Trans.localPosition = pos;
        }
    }

    public float LocalPositionZ
    {
        get => Trans.localPosition.z;
        set
        {
            var pos = Trans.localPosition;
            pos.z = value;
            Trans.localPosition = pos;
        }
    }

    #endregion

    public Quaternion Rotation
    {
        get => Trans.rotation;
        set => Trans.rotation = value;
    }

    public Quaternion LocalRotation
    {
        get => Trans.localRotation;
        set => Trans.localRotation = value;
    }

    #region EularAngles
    
    public Vector3 EulerAngles
    {
        get => Trans.eulerAngles;
        set => Trans.eulerAngles = value;
    }

    public float EulerAnglesX
    {
        get => Trans.eulerAngles.x;
        set
        {
            var pos = Trans.eulerAngles;
            pos.x = value;
            Trans.eulerAngles = pos;
        }
    }

    public float EulerAnglesY
    {
        get => Trans.eulerAngles.y;
        set
        {
            var pos = Trans.eulerAngles;
            pos.y = value;
            Trans.eulerAngles = pos;
        }
    }

    public float EulerAnglesZ
    {
        get => Trans.eulerAngles.z;
        set
        {
            var pos = Trans.eulerAngles;
            pos.z = value;
            Trans.eulerAngles = pos;
        }
    }

    #endregion

    #region Local EulerAngles

    public Vector3 LocalEulerAngles
    {
        get => Trans.localEulerAngles;
        set => Trans.localEulerAngles = value;
    }

    public float LocalEulerAnglesX
    {
        get => Trans.localEulerAngles.x;
        set
        {
            var pos = Trans.localEulerAngles;
            pos.x = value;
            Trans.localEulerAngles = pos;
        }
    }

    public float LocalEulerAnglesY
    {
        get => Trans.localEulerAngles.y;
        set
        {
            var pos = Trans.localEulerAngles;
            pos.y = value;
            Trans.localEulerAngles = pos;
        }
    }

    public float LocalEulerAnglesZ
    {
        get => Trans.localEulerAngles.z;
        set
        {
            var pos = Trans.localEulerAngles;
            pos.z = value;
            Trans.localEulerAngles = pos;
        }
    }

    #endregion

    #region Local Scale
    
    public Vector3 LocalScale
    {
        get => Trans.localScale;
        set => Trans.localScale = value;
    }

    public float LocalScaleX
    {
        get => Trans.localScale.x;
        set
        {
            var pos = Trans.localScale;
            pos.x = value;
            Trans.localScale = pos;
        }
    }

    public float LocalScaleY
    {
        get => Trans.localScale.y;
        set
        {
            var pos = Trans.localScale;
            pos.y = value;
            Trans.localScale = pos;
        }
    }

    public float LocalScaleZ
    {
        get => Trans.localScale.z;
        set
        {
            var pos = Trans.localScale;
            pos.z = value;
            Trans.localScale = pos;
        }
    }

    public float LocalScaleValue
    {
        set => Trans.localScale = Vector3.one * value;
    }

    #endregion

    #endregion

    #region Cordinate

    public Vector3 WorldToUiPosition()
    {
        return WorldToUiPosition(Position);
    }

    public Vector3 WorldToUiPosition(Vector3 worldPosition)
    {
        var sceneCamera = Camera.MainCamera;
        var uiCanvas = UI.Canvas;
        switch (uiCanvas.renderMode)
        {
            case RenderMode.ScreenSpaceCamera:
                // 返回结果为 anchoredPosition 
                var uiCamera = UI.UICamera;
                return WorldToUiPosition(sceneCamera, uiCamera, uiCanvas, worldPosition);
            case RenderMode.ScreenSpaceOverlay:
                // 返回结果为 anchoredPosition 
                return WorldToUiPosition(sceneCamera, null, uiCanvas, worldPosition);
            case RenderMode.WorldSpace:
                // 返回结果为 position 
                var result = uiCanvas.transform.InverseTransformPoint(worldPosition);
                return result;
            default:
                return worldPosition;
        }
    }

    public Vector3 WorldToUiPosition(Camera sceneCamera, Camera uiCamera, Canvas uiCanvas, Vector3 worldPosition)
    {
        var screenPos = sceneCamera.WorldToScreenPoint(worldPosition);
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            uiCanvas.transform as RectTransform,
            screenPos,
            uiCamera,
            out var result);

        return result;
    }

    public Vector3 MouseToWorldPosition(float cameraZ = 0f)
    {
        return UiToWorldPosition(Input.mousePosition, cameraZ);
    }

    public Vector3 UiToWorldPosition(Vector3 mousePosition, float cameraZ = 0f)
    {
        var sceneCamera = MainCamera;
        mousePosition.z = cameraZ;
        var worldPosition = sceneCamera.ScreenToWorldPoint(mousePosition);
        return worldPosition;
    }

    public Vector3 LocalToWorldPosition(Vector3 localPosition)
    {
        return Trans.TransformPoint(localPosition);
    }

    public Vector3 WorldToLocalPosition(Vector3 worldPosition)
    {
        return Trans.InverseTransformPoint(worldPosition);
    }

    #endregion

    #region Parent

    public Transform Parent
    {
        get => Trans.parent;
        set
        {
            if (!gameObject.activeInHierarchy) return;
            Trans.parent = value;
        }
    }

    public void SetParentToLevel()
    {
        Parent = CurrentLevel.Trans;
    }

    public void ClearParent()
    {
        Parent = null;
    }

    #endregion

    #region Child


    [NonSerialized] public Dictionary<string, Transform> ChildTransDic = new Dictionary<string, Transform>();

    public Transform GetChildTrans(string transName)
    {
        if (ChildTransDic.TryGetValue(transName, out var trans)) return trans;
        trans = Trans.Find(transName);
        if (trans != null) return trans;
        var go = new GameObject(transName);
        trans = go.transform;
        trans.parent = Trans;
        ChildTransDic.Add(transName, trans);
        return trans;
    }

    #endregion
}