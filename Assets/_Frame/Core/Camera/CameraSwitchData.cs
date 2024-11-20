using Cinemachine;
using System;
using UnityEngine;

[Serializable]
public class CameraSwitchData
{
    public Transform Follow;
    public Transform LookAt;
    public CinemachineBlendDefinition.Style BlendType;
    public float BlendDuration;

    internal static AppManager App => AppManager.Ins;
    internal static CameraManager Camera => CameraManager.Ins;
    internal static Player Player => App.Player;
    internal static Transform PlayerTarget => Player != null ? Player.RendererTrans : null;
    internal static Transform DraggableCamTarget => App.CurrentLevel.DraggableCamTarget.TargetObject.transform;

    public static CameraSwitchData GameReady =>
        new()
        {
            Follow = PlayerTarget,
            LookAt = null,
            BlendType = CinemachineBlendDefinition.Style.Cut,
            BlendDuration = 0
        };

    public static CameraSwitchData GameStart =>
        new()
        {
            Follow = PlayerTarget,
            LookAt = null,
            BlendType = Camera.DefaultBlend,
            BlendDuration = Camera.BlendDuration
        };

    public static CameraSwitchData GameWin =>
        new()
        {
            Follow = PlayerTarget,
            LookAt = null,
            BlendType = Camera.DefaultBlend,
            BlendDuration = Camera.BlendDuration
        };

    public static CameraSwitchData GameLose =>
        new()
        {
            Follow = PlayerTarget,
            LookAt = null,
            BlendType = Camera.DefaultBlend,
            BlendDuration = Camera.BlendDuration
        };
}