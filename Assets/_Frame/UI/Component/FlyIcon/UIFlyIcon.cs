using System;
using System.Collections;
using System.Collections.Generic;
using Aya.Async;
using Aya.Extension;
using Aya.Pool;
using Aya.TweenPro;
using Aya.Util;
using Sirenix.OdinInspector;
using UnityEngine;

public class UIFlyIcon : UIBase<UIFlyIcon>
{
    public const int Coin = 0;
    public const int Diamond = 1;
    public const int Key = 2;
    public const int Star = 3;

    public float FlyDuration = 1f;
    public float Interval = 0.05f;
    public float RandomStartPos = 100f;
    public int PerFrameLimit = 5;
    // public AnimationCurve CurveX;
    // public AnimationCurve CurveY;
    public AnimationCurve CurveScaleIcon;
    public float ScaleTargetDuration;
    public AnimationCurve CurveScaleTarget;

    [TableList] public List<UIFlyIconData> TargetList;

    [NonSerialized]
    public List<int> EaseTypeList = new List<int>()
    {
        // EaseType.Linear,
        EaseType.InQuad,
        // EaseType.OutQuad,
        // EaseType.InOutQuad,
        EaseType.InCubic,
        // EaseType.OutCubic,
        // EaseType.InOutCubic,
        EaseType.InQuart,
        // EaseType.OutQuart,
        // EaseType.InOutQuart,
        EaseType.InQuint,
        // EaseType.OutQuint,
        // EaseType.InOutQuint,
        EaseType.InSine,
        // EaseType.OutSine,
        // EaseType.InOutSine,
        EaseType.InExpo,
        // EaseType.OutExpo,
        // EaseType.InOutExpo,
        EaseType.InCirc,
        // EaseType.OutCirc,
        // EaseType.InOutCirc,
        EaseType.InBack,
        // EaseType.OutBack,
        // EaseType.InOutBack
    };

    [NonSerialized] public Dictionary<string, UIFlyIconData> TargetDic;

    public EntityPool IconPool => PoolManager.Ins[nameof(UIFlyIcon)];

    protected override void Awake()
    {
        base.Awake();
        TargetDic = TargetList.ToDictionary(t => t.Name);
    }

    public void Fly(string key, Vector3 startPos, int count = 1, Action onEach = null, Action onDone = null)
    {
        var data = TargetDic[key];
        Fly(data.Prefab, startPos, data.Target.position, data.Target, count, Interval, null, onEach, onDone);
    }

    public void Fly(string key, Vector3 startPos, int count = 1, string iconValue = null, Action onEach = null, Action onDone = null)
    {
        var data = TargetDic[key];
        Fly(data.Prefab, startPos, data.Target.position, data.Target, count, Interval, iconValue, onEach, onDone);
    }

    public void Fly(int index, Vector3 startPos, int count = 1, Action onEach = null, Action onDone = null)
    {
        var data = TargetList[index];
        Fly(data.Prefab, startPos, data.Target.position, data.Target, count, Interval, null, onEach, onDone);
    }

    public void Fly(int index, Vector3 startPos, int count = 1, string iconValue = null, Action onEach = null, Action onDone = null)
    {
        var data = TargetList[index];
        Fly(data.Prefab, startPos, data.Target.position, data.Target, count, Interval, iconValue, onEach, onDone);
    }

    public void Fly(UIFlyIconItem iconPrefab, Vector3 startPos, Vector3 endPos, Transform scaleTargetTrans, int count, float interval, string iconValue = null, Action onEach = null, Action onDone = null)
    {
        StartCoroutine(FlyCo(iconPrefab, startPos, endPos, scaleTargetTrans, count, interval, iconValue, onEach, onDone));
    }

    protected IEnumerator FlyCo(UIFlyIconItem iconPrefab, Vector3 startUiPos, Vector3 endWorldPos, Transform scaleTargetTrans, int count, float interval, string iconValue = null, Action onEach = null, Action onDone = null)
    {
        if (iconPrefab == null) yield break;
        for (var i = 0; i < count;)
        {
            var frameCounter = 0;
            while (frameCounter < PerFrameLimit && i < count)
            {
                var iconStartUiPos = startUiPos + RandUtil.RandVector3(-RandomStartPos, RandomStartPos);
                var iconEndPos = endWorldPos;
                var iconIns = IconPool.Spawn(iconPrefab, Trans, iconStartUiPos);
                iconIns.Rect.anchoredPosition = iconStartUiPos;

                var iconStartWorldPos = iconIns.Trans.position;
                iconIns.Trans.localScale = Vector3.zero;
                iconIns.Init();
                iconIns.SetValue(iconValue);
                UTween.Scale(iconIns.transform, 0f, 1f, FlyDuration)
                    .SetCurve(CurveScaleIcon);

                var easeTypeX = EaseType.FunctionDic[EaseTypeList.Random()];
                var easeTypeY = EaseType.FunctionDic[EaseTypeList.Random()];

                UTween.Value(0f, 1f, FlyDuration, value =>
                    {
                        var x = easeTypeX.Ease(iconStartWorldPos.x, iconEndPos.x, value);
                        var y = easeTypeY.Ease(iconStartWorldPos.y, iconEndPos.y, value);
                        iconIns.transform.SetPositionXY(x, y);
                    })
                    .SetOnStop(() =>
                    {
                        onEach?.Invoke();
                        if (scaleTargetTrans != null)
                        {
                            UTween.Scale(scaleTargetTrans, 0f, 1f, ScaleTargetDuration)
                                .SetCurve(CurveScaleTarget);
                        }

                        IconPool.DeSpawn(iconIns);
                    });
                i++;
                frameCounter++;
            }

            yield return YieldBuilder.WaitForSeconds(interval);
        }

        yield return YieldBuilder.WaitForSeconds(FlyDuration);
        onDone?.Invoke();
        yield return null;
    }
}