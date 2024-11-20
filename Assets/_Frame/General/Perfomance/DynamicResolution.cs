using System.Collections;
using System.Collections.Generic;
using Aya.UI.Markup;
using UnityEngine;

public class DynamicResolution : EntityBase<DynamicResolution>
{
    public int TargetFps = 60;
    public float CheckInterval = 1f;
    public int TriggerDiff = 5;
    public bool AllowUpgradeResolution = true;
    public bool AutoRun = false;

    public bool Active { get; set; } = false;

    public List<Vector2Int> DesignResolution = new List<Vector2Int>()
    {
        new Vector2Int(1080, 1920),
        new Vector2Int(900, 1600),
        new Vector2Int(810, 1440),
        new Vector2Int(720, 1280),
        new Vector2Int(540, 960),
        new Vector2Int(480, 800),
    };

    public Vector2Int ScreenResolution { get; set; }

    public int CurrentRenderIndex { get; set; } = -1;

    private float _checkTimer;

    protected override void Awake()
    {
        base.Awake();

        if (AutoRun) Active = true;
        Application.targetFrameRate = TargetFps;
        ScreenResolution = new Vector2Int(Screen.width, Screen.height);

        for (var i = 0; i < DesignResolution.Count; i++)
        {
            var size = DesignResolution[i];
            var width = size.x;
            var height = Mathf.RoundToInt(width * 1f / Screen.width * Screen.height);
            size.y = height;
            DesignResolution[i] = size;
        }

        SetResolution(0);
    }

    public virtual void Update()
    {
        if (!Active) return;
        var deltaTime = Time.unscaledDeltaTime;
        _checkTimer += deltaTime;
        if (_checkTimer >= CheckInterval)
        {
            _checkTimer = 0f;

            var fps = 1f / deltaTime;
            if (fps <=  TargetFps - TriggerDiff)
            {
                SetResolution(CurrentRenderIndex + 1);
            }

            if (AllowUpgradeResolution && fps >= TargetFps)
            {
                SetResolution(CurrentRenderIndex - 1);
            }
        }
    }

    public void SetResolution(int index)
    {
        index = Mathf.Clamp(index, 0, DesignResolution.Count - 1);
        if (CurrentRenderIndex == index) return;
        CurrentRenderIndex = index;
        var size = DesignResolution[CurrentRenderIndex];
        ScalableBufferManager.ResizeBuffers(size.x, size.y);
        // Screen.SetResolution(size.x, size.y, Screen.fullScreenMode, TargetFps);
        var fps = 1f / Time.unscaledDeltaTime;
        Debug.Log("[Dynamic Resolution] ".ToMarkup(Color.cyan) + size.x + "x" + size.y + "/t Fps : " + fps);
    }
}
