using System;
using Cinemachine;
using Sirenix.OdinInspector;

[Serializable]
public class CameraData
{
    [TableColumnWidth(100, false)] public string Key;
    public CinemachineVirtualCamera Camera;
}
