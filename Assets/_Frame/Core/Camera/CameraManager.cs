using System;
using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using Sirenix.OdinInspector;
using UnityEngine;

[ExecuteInEditMode]
public class CameraManager : EntityBase<CameraManager>
{
    [BoxGroup("Component")] public new Camera Camera;
    [BoxGroup("Component")] public CinemachineBrain Brain;

    [BoxGroup("Param")] public CameraReference Default;
    [BoxGroup("Param")] public CinemachineBlendDefinition.Style DefaultBlend = CinemachineBlendDefinition.Style.EaseInOut;
    [BoxGroup("Param")]
    [SuffixLabel("sec")]
    public float BlendDuration = 1f;

    [TableList] public List<CameraData> CameraList;

    [NonSerialized] public string CurrentKey;
    [NonSerialized] public CameraData Current;

    protected override void Awake()
    {
        base.Awake();
        if (Camera == null) Camera = Camera.main;
        Switch(Default);
    }

    [Button(SdfIconType.ArrowRepeat, " Auto Cache")]
    public void AutoCache()
    {
        var cameras = GetComponentsInChildren<CinemachineVirtualCamera>(true);
        CameraList = cameras.Select(c => new CameraData()
        {
            Key = c.name,
            Camera = c
        }).ToList();
    }

    public CameraData GetCamera(string key)
    {
        foreach (var cameraData in CameraList)
        {
            if (cameraData.Key == key)
            {
                return cameraData;
            }
        }

        return null;
    }

    public CameraData GetCamera(CinemachineVirtualCamera virtualCamera)
    {
        foreach (var cameraData in CameraList)
        {
            if (cameraData.Camera == virtualCamera)
            {
                return cameraData;
            }
        }

        return null;
    }

    public void SetFollow(Transform target)
    {
        Current.Camera.Follow = target;
    }

    public void SetLookAt(Transform target)
    {
        Current.Camera.LookAt = target;
    }

    public void SwitchImmediately(string key, Transform follow = null, Transform lookAt = null)
    {
        Switch(key, follow, lookAt, CinemachineBlendDefinition.Style.Cut, 0f);
    }

    public void Switch(string key, Transform follow = null, Transform lookAt = null)
    {
        Switch(key, follow, lookAt, DefaultBlend, BlendDuration);
    }

    public void Switch(string key, CinemachineBlendDefinition.Style blendType, float blendDuration)
    {
        Switch(key, null, null, blendType, blendDuration);
    }

    public void Switch(string key, CameraSwitchData switchData)
    {
        Switch(key, switchData.Follow, switchData.LookAt, switchData.BlendType, switchData.BlendDuration);
    }

    public void Switch(string key, Transform follow, Transform lookAt, CinemachineBlendDefinition.Style blendType, float blendDuration)
    {
        if (CurrentKey == key && follow == Current.Camera.Follow && lookAt == Current.Camera.LookAt) return;
        var cam = GetCamera(key);
        if (cam == null) return;

        Brain.m_DefaultBlend = new CinemachineBlendDefinition(blendType, blendDuration);
        foreach (var cameraData in CameraList)
        {
            if (cameraData.Key == key)
            {
                CurrentKey = key;
                Current = cameraData;
                cameraData.Camera.gameObject.SetActive(true);
                Dispatch(GameEvent.SwitchCamera, CurrentKey);
            }
            else
            {
                cameraData.Camera.gameObject.SetActive(false);
            }
        }

        SetFollow(follow);
        SetLookAt(lookAt);

        Dispatch(GameEvent.SwitchCamera, key, follow, lookAt);
    }
}
