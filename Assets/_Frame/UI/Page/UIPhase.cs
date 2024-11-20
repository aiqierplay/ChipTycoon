using System;
using System.Collections.Generic;
using Aya.Reflection;
using Aya.TweenPro;
using Sirenix.OdinInspector;
using UnityEngine.UI;

public abstract class UIPhase<T> : UIPage<T> where T : UIPhase<T>
{
    [BoxGroup("Button")] public Button BtnNextPhase;
    [BoxGroup("Phase"), TypeReference(typeof(UIPage))] public TypeReference Type;
    [BoxGroup("Phase")] public string SwitchCamera;

    [BoxGroup("Enter")] public List<UTweenPlayer> EnterTweenList;
    [BoxGroup("Complete")] public List<UTweenPlayer> CompleteTweenList;
    [BoxGroup("Exit")] public List<UTweenPlayer> ExitTweenList;

    [NonSerialized] public bool IsCompleted;

    protected override void Awake()
    {
        base.Awake();
        if (BtnNextPhase != null)
        {
            BtnNextPhase.onClick.AddListener(NextPhase);
        }
    }

    public override void Show(params object[] args)
    {
        base.Show(args);

        if (BtnNextPhase != null)
        {
            BtnNextPhase.gameObject.SetActive(false);
        }

        Enter();
    }

    public virtual void Enter()
    {
        if (!string.IsNullOrEmpty(SwitchCamera))
        {
            Camera.Switch(SwitchCamera);
        }

        IsCompleted = false;
        EnterTweenList.ForEach(t => t.Play());
        OnEnter();
    }

    public virtual void Complete()
    {
        IsCompleted = true;
        CompleteTweenList.ForEach(t => t.Play());
        OnComplete();
    }

    public virtual void Exit()
    {
        ExitTweenList.ForEach(t => t.Play());
        OnExit();
    }

    public virtual void NextPhase()
    {
        Exit();
        UI.Show(Type);
    }

    public virtual void Update()
    {
        if (!IsGaming) return;

        var complete = CheckComplete();
        if (BtnNextPhase != null && CheckCanEnterNextPhase())
        {
            BtnNextPhase.gameObject.SetActive(true);
        }

        if (complete && !IsCompleted)
        {
            Complete();
        }

        var deltaTime = DeltaTime;
        OnUpdate(deltaTime);
    }

    public virtual bool CheckComplete()
    {
        return true;
    }

    public virtual bool CheckCanEnterNextPhase()
    {
        return IsCompleted;
    }

    public abstract void OnEnter();
    public virtual void OnComplete()
    {
    }

    public abstract void OnExit();
    public abstract void OnUpdate(float deltaTime);


}
