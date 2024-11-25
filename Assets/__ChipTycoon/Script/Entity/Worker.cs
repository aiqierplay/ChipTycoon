using System;
using System.Collections;
using Aya.TweenPro;
using Sirenix.OdinInspector;
using UnityEngine;

public enum WorkerType
{
    Player = 0,
    Computer = 1,
}

public class Worker : EntityBase
{
    [Title("Animation")] 
    public string IdleClip;
    public string WalkClip;

    public float MoveSpeed = 5f;
    public float RotateSpeed = 50f;

    [NonSerialized] public WorkerType Type;

    public void Init(WorkerType type)
    {
        EnableMove();
        Type = type;
        Play(IdleClip);
    }

    #region Trigger
    
    public void OnEnter(BuildingBase building)
    {
    }

    public void OnExit(BuildingBase building)
    {

    }

    #endregion

    #region Move
  
    [NonSerialized] public bool ActiveMove;
    [NonSerialized] public bool IsMoving;
    [NonSerialized] public Vector3 StartPos;
    [NonSerialized] public Vector3 Direction;

    public void EnableMove()
    {
        ActiveMove = true;
    }

    public void DisableMove()
    {
        ActiveMove = false;
        IsMoving = false;
    }

    public void OnTouchStart(Vector3 pos)
    {
        Play(WalkClip);
        IsMoving = true;
        StartPos = pos;
    }

    public void OnTouch(Vector3 pos)
    {
        Direction = pos - StartPos;
        Direction = Direction.normalized;
        Direction.z = Direction.y;
        Direction.y = 0;
        Move(Direction);
    }

    public void OnTouchEnd(Vector3 pos)
    {
        Direction = Vector3.zero;
        IsMoving = false;
        Play(IdleClip);
    }

    public void Move(Vector3 direction)
    {

    }

    public IEnumerator MoveTo(Vector3 position, Action onDone)
    {
        Play(WalkClip);
        var tweenMove = UTween.Position(Trans, position, MoveSpeed)
            .SetSpeedBased();
        yield return tweenMove.WaitForComplete();
        var forward = position - Position;
        if (forward != Vector3.zero)
        {
            UTween.Forward(Trans, forward, 0.2f);
        }

        Play(IdleClip);
        yield return null;
    } 

    #endregion

    public void LateUpdate()
    {
        if (ActiveMove && IsMoving)
        {
            if (Direction == Vector3.zero) return;
            Trans.position += Direction * MoveSpeed * DeltaTime;
            Trans.forward = Vector3.MoveTowards(Forward, Direction, RotateSpeed * DeltaTime);
        }
    }
}
