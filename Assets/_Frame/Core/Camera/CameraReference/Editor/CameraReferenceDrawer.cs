#if UNITY_EDITOR
using System.Collections.Generic;
using Aya.Reflection;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(CameraReference))]
public class CameraReferenceDrawer : SearchableDropdownDrawer<CameraData>
{
    public CameraManager CameraManager
    {
        get
        {
            if (_cameraManager == null)
            {
                _cameraManager = Object.FindObjectOfType<CameraManager>(true);
            }

            return _cameraManager;
        }
    }

    private CameraManager _cameraManager;

    public SerializedProperty CameraProperty;

    public override void CacheProperty(SerializedProperty property)
    {
        CameraProperty = Property.FindPropertyRelative(nameof(Camera));
    }

    public override CameraData GetValue()
    {
        return CameraManager.GetCamera(CameraProperty.stringValue);
    }

    public override void SetValue(CameraData value)
    {
        if (value != null)
        {
            CameraProperty.stringValue = value.Key;
        }
        else
        {
            CameraProperty.stringValue = "";
        }
    }

    public override string GetDisplayName(CameraData value)
    {
        return value.Key;
    }

    public override string GetRootName()
    {
        return nameof(Camera);
    }

    public override IEnumerable<CameraData> GetValueCollection()
    {
        return CameraManager.CameraList;
    }
}
#endif