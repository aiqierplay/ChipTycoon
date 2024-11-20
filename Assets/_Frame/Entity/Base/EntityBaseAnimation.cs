using System;
using Aya.Extension;
using Aya.TweenPro;
using UnityEngine;

public abstract partial class EntityBase
{
    #region Animator

    [GetComponentInChildren, NonSerialized] public Animator Animator;

    [NonSerialized] public string CurrentAnimationClip;

    private string _lastAnimationClipName;
    private float _lastPlayAnimationTime;
    private readonly float _minPlayAnimationInterval = 0f;

    public void CacheAnimator()
    {
        Animator = GetComponentInChildren<Animator>();
        if (!string.IsNullOrEmpty(CurrentAnimationClip)) Play(CurrentAnimationClip);
    }

    public void InitAnimatorParameters()
    {
        if (Animator == null) return;
        _lastAnimationClipName = "";
        _lastPlayAnimationTime = 0;
        foreach (var animatorControllerParameter in Animator.parameters)
        {
            if (animatorControllerParameter.type == AnimatorControllerParameterType.Bool)
            {
                Animator.SetBool(animatorControllerParameter.name, false);
            }
        }
    }

    public void Play(string animationClipName, bool immediately = false)
    {
        if (Animator == null) Animator = GetComponentInChildren<Animator>(true);
        if (Animator == null) return;

        var currentTime = Time.realtimeSinceStartup;
        if (currentTime - _lastPlayAnimationTime <= _minPlayAnimationInterval && _minPlayAnimationInterval > 0f) return;

        var playSuccess = false;
        if (!string.IsNullOrEmpty(_lastAnimationClipName))
        {
            if (Animator.CheckParameterExist(_lastAnimationClipName, AnimatorControllerParameterType.Bool))
            {
                Animator.SetBool(_lastAnimationClipName, false);
            }
            else if (Animator.CheckParameterExist(_lastAnimationClipName, AnimatorControllerParameterType.Trigger))
            {
                Animator.ResetTrigger(_lastAnimationClipName);
            }
        }

        if (!immediately)
        {
            if (Animator.CheckParameterExist(animationClipName, AnimatorControllerParameterType.Bool))
            {
                Animator.SetBool(animationClipName, true);
                playSuccess = true;
            }
            else if (Animator.CheckParameterExist(animationClipName, AnimatorControllerParameterType.Trigger))
            {
                Animator.ResetTrigger(animationClipName);
                Animator.SetTrigger(animationClipName);
                playSuccess = true;
            }
            else if (Animator.CheckClipExist(animationClipName))
            {
                Animator.Play(animationClipName);
                playSuccess = true;
            }
        }
        else if (Animator.CheckClipExist(animationClipName))
        {
            Animator.Play(animationClipName, 0, 0f);
            playSuccess = true;
        }

        if (playSuccess)
        {
            CurrentAnimationClip = animationClipName;
            _lastPlayAnimationTime = Time.realtimeSinceStartup;
            _lastAnimationClipName = animationClipName;
        }
    }

    public void FadeLayerWeight(string layerName, float weight, float duration)
    {
        if (Animator == null) return;
        if (!Animator.CheckLayerExist(layerName)) return;
        UTween.Value(Animator.GetLayerWeight(layerName), weight, duration, value => { Animator.SetLayerWeight(layerName, value); });
    }

    public void PlayWithLayer(string animationClipName, float fadeWeightDuration = 0.1f)
    {
        Play(animationClipName);

        if (Animator == null) return;
        if (!Animator.CheckLayerExist(animationClipName)) return;

        var length = Animator.GetCurrentAnimatorStateInfo(animationClipName).length;
        FadeLayerWeight(animationClipName, 1f, fadeWeightDuration);
        this.ExecuteDelay(() =>
        {
            FadeLayerWeight(animationClipName, 0f, fadeWeightDuration);
        }, length);
    }

    public float GetNormalizeTime()
    {
        if (Animator == null) return 0f;
        if (!Animator.isActiveAndEnabled) return 0f;
        var stateInfo = Animator.GetCurrentAnimatorStateInfo(0);
        return stateInfo.normalizedTime;
    }

    public void SetNormalizedTime(float normalizedProgress)
    {
        if (Animator == null) return;
        if (!Animator.isActiveAndEnabled) return;
        Animator.Play(CurrentAnimationClip, 0, normalizedProgress);
        Animator.Update(0);
        Animator.speed = 1f;
    }

    #endregion
}
