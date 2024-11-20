using System;
using System.Collections;
using System.Runtime.CompilerServices;
using Aya.Async;
using Aya.Extension;
using Aya.TweenPro;
using Sirenix.OdinInspector;
using UnityEngine;

public class PlayerMove : PlayerBase
{
    [Title("Move")]
    public bool KeepUp;
    public float MoveSpeed;
    public float RotateSpeed;
    [Title("Turn")]
    public float TurnSpeed;
    public float TurnLerpSpeed;
    [Title("Move Back")]
    public float MoveBackDistance = 1f;
    public float MoveBackDuration = 0.5f;

    public float MoveSpeedActually => MoveSpeed * State.SpeedMultiply;

    public override void InitComponent()
    {
        if (LevelPathFollower != null) LevelPathFollower.Init(Self);
        IsBounce = false;
    }

    public override void CacheComponent()
    {
        base.CacheComponent();
        LevelPathFollower = gameObject.GetOrAddComponent<LevelPathFollower>();
    }

    #region Path

    public LevelPathFollower LevelPathFollower { get; set; }
    public LevelBlock CurrentBlock => LevelPathFollower.CurrentBlock;
    public LevelPath CurrentPath => LevelPathFollower.CurrentPath;

    public Vector3 MovePath(float distance)
    {
        var position = LevelPathFollower.Move(distance);
        Position = position;
        return position;
    }

    public void AutoMovePath(float distance, float duration)
    {
        StartCoroutine(AutoMovePathCo(distance, duration));
    }

    protected IEnumerator AutoMovePathCo(float distance, float duration)
    {
        var speed = distance / duration;
        var disCount = 0f;
        while (disCount < distance)
        {
            var moveDis = speed * DeltaTime;
            if (moveDis + disCount > distance) moveDis = distance - disCount;
            disCount += moveDis;
            var position = LevelPathFollower.Move(moveDis);
            Position = position;
            yield return null;
        }
    }

    public void EnterBlock(int index)
    {
        LevelPathFollower.EnterBlock(index);
    }

    public void EnterPath(int index)
    {
        LevelPathFollower.EnterPath(index);
    }

    public void SwitchPath(int blockIndex, int pathIndex)
    {
        LevelPathFollower.SwitchPath(blockIndex, pathIndex);
    }

    public void MovePathCenter(float duration = 0.5f)
    {
        UTween.LocalPositionX(RendererTrans, 0f, duration);
    }

    public IEnumerator MovePathCenterCo(float duration = 0.5f)
    {
        UTween.LocalPositionX(RendererTrans, 0f, duration);
        yield return YieldBuilder.WaitForSeconds(duration);
    }

    [NonSerialized] public bool IsBounce;

    public void NotAllowMoveBounce()
    {
        NotAllowMoveBounce(MoveBackDuration, MoveBackDistance);
    }

    public void NotAllowMoveBounce(float duration, float distance)
    {
        if (IsBounce) return;
        IsBounce = true;
        UTween.Value(0f, -1f * distance, duration, value =>
            {
                State.SpeedMultiply = 0f;
                RendererTrans.SetLocalPositionZ(value);
            })
            .SetEase(EaseType.OutBack)
            .SetOnStop(() =>
            {
                RendererTrans.SetLocalPositionZ(0f);
                IsBounce = false;
                this.ExecuteNextFrame(() =>
                {
                    State.SpeedMultiply = 1f;
                });

                MovePath(-distance);
            });
    }

    #endregion

    #region Auto Move

    public void MoveTo(Vector3 position)
    {
        var duration = Vector3.Distance(Position, position) / MoveSpeedActually;
        MoveTo(position, duration);
    }

    public void MoveTo(Vector3 position, float duration)
    {
        var direction = position - Position;
        Forward = direction;
        UTween.Position(Trans, Position, position, duration);
    }

    #endregion

    #region lOmnidirectional

    public Vector3 MoveDirection(Vector3 direction, float distance)
    {
        var currentPosition = Position;
        currentPosition += direction * distance;
        Position = currentPosition;
        Forward = Vector3.Lerp(Forward, direction, RotateSpeed * DeltaTime);
        return currentPosition;
    }

    #endregion

    public void EnableMove()
    {
        State.EnableMove = true;
        State.EnableInput = true;
        Self.Play(AnimatorDefine.Run);
    }

    public void DisableMove(bool stopAnimation = true)
    {
        State.EnableMove = false;
        State.EnableInput = false;
        Control.ClearInput();
        if (stopAnimation) Self.Play(AnimatorDefine.Idle);
    }
}