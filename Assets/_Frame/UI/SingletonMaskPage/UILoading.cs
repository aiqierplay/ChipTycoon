using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UILoading : UISingletonMaskPage<UILoading>
{
    public class LoadingPhase
    {
        public float Progress;
        public int Count;
        public Action<int> Action;

        public IEnumerator Loading()
        {
            Progress = 0f;
            for (var i = 0; i < Count; i++)
            {
                Progress = i * 1f / Count;
                Action?.Invoke(i);
                yield return null;
            }

            Progress = 1f;

            yield return null;
        }
    }

    public Image ImgProgress;
    public TMP_Text TextProgress;
    public float MinLoadDuration = 1f;

    [NonSerialized] public List<LoadingPhase> LoadingPhases = new List<LoadingPhase>();
    private float _startTime;

    public void SetProgress(float progress)
    {
        if (ImgProgress != null)
        {
            ImgProgress.fillAmount = progress;
        }

        if (TextProgress != null)
        {
            TextProgress.text = Mathf.RoundToInt(progress * 100f).ToString("D2") + "%";
        }
    }

    #region Enter Exit
   
    public override void Show(Action onShow = null)
    {
        base.Show(onShow);
    }

    public override void Hide(Action onHide = null)
    {
        base.Hide(onHide);
    } 

    #endregion

    #region Register

    public void RegisterLoading(int count, Action<int> action)
    {
        LoadingPhases.Add(new LoadingPhase()
        {
            Count = count,
            Action = action
        });
    }

    public void StartLoading(Action onDone)
    {
        Show(() =>
        {
            StartCoroutine(Loading(onDone));
        });
    }

    public void ClearRegister()
    {
        LoadingPhases.Clear();
    }

    protected IEnumerator Loading(Action onDone)
    {
        _startTime = Time.realtimeSinceStartup;
        var progress = 0f;
        SetProgress(progress);
        for (var i = 0; i < LoadingPhases.Count; i++)
        {
            var load = LoadingPhases[i];
            load.Action += index =>
            {
                progress = i * 1f / LoadingPhases.Count + load.Progress * (1f / LoadingPhases.Count);
                SetProgress(progress);
            };

            yield return load.Loading();
        }

        SetProgress(1f);
        yield return null;

        var currentTime = Time.realtimeSinceStartup;
        var costTime = currentTime - _startTime;
        if (costTime < MinLoadDuration) yield return new WaitForSeconds(MinLoadDuration - costTime);

        Hide(onDone);
    }

    #endregion
}
