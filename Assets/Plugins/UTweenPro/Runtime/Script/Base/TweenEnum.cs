﻿using System;

namespace Aya.TweenPro
{
    [Flags]
    public enum AxisConstraint
    {
        None = 0,
        X = 2,
        Y = 4,
        Z = 8,
        W = 16,
        All = X | Y| Z| W,
    }

    public enum TweenControlMode
    {
        TweenManager = 0,
        Component = 1,
    }

    public enum PlayState
    {
        None = 0,
        Playing = 1,
        Paused = 2,
        Stopped = 3,
        Completed = 4,
    }

    public enum PlayMode
    {
        Once = 0,
        Loop = 1,
        PingPong = 2,
    }

    public enum PrepareSampleMode
    {
        None = 0,
        Awake = 1,
        Enable = 2,
        Start = 3
    }

    public enum TimeMode
    {
        Normal = 0,
        UnScaled = 1,
        Smooth = 2,
    }

    public enum AutoPlayMode
    {
        None = 0,
        Awake = 1,
        Enable = 2,
        Start = 3,
        [Obsolete] Trigger = 4,
    }

    public enum UpdateMode
    {
        Update = 0,
        LateUpdate = 1,
        FixedUpdate = 2,
        WaitForFixedUpdate = 3,
        WaitForEndOfFrame = 4,
        SelfCoroutine = 6,
    }

    public enum SpaceMode
    {
        World = 0,
        Local = 1,
    }

    public enum AngleMode
    {
        Clamp360 = 0,
        Over360 = 1,
    }

    public enum ColorMode
    {
        FromTo = 0,
        Gradient = 1,
    }

    public enum ColorLerpMode
    {
        RGB = 0,
        HSV = 1,
    }

    public enum ColorOverlayMode
    {
        Multiply = 0,
        Add = 1,
        Minus = 2,
    }

    public enum TextRangeMode
    {
        Length = 0,
        Percent = 1,
    }

    public enum TextOrderMode
    {
        Normal = 0,
        UniformRandom = 1,
        SelfRandom = 2,
    }

    public enum CharacterSpaceMode
    {
        Character = 0,
        Text = 1,
    }

    public enum CallbackType
    {
        OnPlay = 0,
        OnStart = 1,
        OnUpdate = 2,
        OnLoopStart = 3,
        OnLoopEnd = 4,
        OnPause = 5,
        OnResume = 6,
        OnStop = 7,
        OnComplete = 8
    }

    public enum DurationMode
    {
        DurationDelay = 0,
        FromTo = 1,
    }

    public enum TweenMaterialMode
    {
        Property = 0,
        Instance = 1,
        Shared = 2,
    }

#if UNITY_EDITOR

    public enum TweenEditorMode
    {
        Component = 1,
        ScriptableObject = 2,
    }

    public enum PathOperationMode
    {
        Position = 0,
        Rotation = 1,
    }

#endif
}