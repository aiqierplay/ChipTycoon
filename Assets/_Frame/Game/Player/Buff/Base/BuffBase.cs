using UnityEngine;

public abstract class BuffBase
{
    public EntityBase Target;
    public Player TargetPlayer => Target as Player;

    public float Duration;
    public object[] Args;
    public GameObject[] Assets;
    public AnimationCurve[] Curves;

    public bool Active;
    public float Timer;
    public float Progress => Timer / Duration;
    public float RemainTime => Duration - Timer;

    public virtual void Start(float duration, object[] args, GameObject[] assets = null, AnimationCurve[] curves = null)
    {
        Duration = duration;
        Timer = 0f;
        Args = args;
        Assets = assets;
        Curves = curves;
        Active = true;
        StartImpl();
    }

    public abstract void StartImpl();

    public virtual void Update(float deltaTime)
    {
        Timer += Time.deltaTime;
        if (Timer >= Duration)
        {
            Timer = Duration;
            End();
        }

        else UpdateImpl(deltaTime);
    }

    public abstract void UpdateImpl(float deltaTime);

    public virtual void End()
    {
        if (!Active) return;
        Active = false;
        EndImpl();
    }

    public abstract void EndImpl();
}