using Aya.TweenPro;
using Sirenix.OdinInspector;
using UnityEngine;

public enum FollowMode
{
    Distance = 0,
    DistanceZ = 1,
    Speed = 2,
}

public class Follower : EntityBase
{
    public Transform Target;
    public FollowMode Mode;

    [Title("Dis Mode")]
    public float KeepDistance;
    public float LerpSpeed;

    [Title("Speed Mode")]
    public float Speed;
    public float MaxDistance;

    public bool KeepDirection;
    public float CaughtDistance;

    public Follower Prefab { get; set; }
    public bool Active { get; set; }

    public virtual void Init()
    {
        Target = null;
        Active = false;
    }

    public virtual void StartFollow()
    {
        if (Active) return;
        Active = true;
    }

    public virtual void StopFollow()
    {
        if (!Active) return;
        Active = false;
    }

    public virtual bool CheckCaught()
    {
        if (Target == null) return false;
        var result = (Target.position - Position).magnitude < CaughtDistance;
        return result;
    }

    public void Follow(Transform target)
    {
        Follow(target, KeepDistance);
    }

    public void Follow(Transform target, float keepDistance)
    {
        Target = target;
        KeepDistance = keepDistance;
        StartFollow();
    }

    public virtual void LateUpdate()
    {
        if (!Active) return;
        if (Target == null) return;

        var targetPos = Target.position;
        var currentPos = Position;
        var targetForward = Target.forward;
        if (!KeepDirection) targetForward = targetPos - currentPos;
        if (targetForward != Vector3.zero)
        {
            Forward = targetForward;
        }

        if (Mode == FollowMode.Distance)
        {
            var pos = -targetForward.normalized * KeepDistance + targetPos;
            pos = Vector3.Lerp(currentPos, pos, DeltaTime * LerpSpeed);
            Position = pos;
            // Log(name, Target.name, pos, Target.position, LerpSpeed, KeepDistance, targetForward);
        }

        if (Mode == FollowMode.DistanceZ)
        {
            var pos = Vector3.back * KeepDistance + targetPos;
            pos = Vector3.Lerp(currentPos, pos, DeltaTime * LerpSpeed);
            var z = pos.z;
            PositionZ = z;
            PositionX = pos.x;
        }

        if (Mode == FollowMode.Speed)
        {
            var direction = (targetPos - currentPos);
            var distance = direction.magnitude;
            var speedMultiplier = 1f;
            if (distance >= MaxDistance) speedMultiplier = 1.5f;
            var move = direction.normalized * DeltaTime * Speed * speedMultiplier;
            if (move.sqrMagnitude > (targetPos - currentPos).sqrMagnitude)
            {
                move = targetPos - currentPos;
            }

            var pos = currentPos + move;
            Position = pos;
        }
    }

    public virtual void OnAdded()
    {

    }

    public virtual void OnRemoved()
    {

    }

    public virtual void Die()
    {

    }

    public void OnDrawGizmos()
    {
        if (Target == null) return;
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, Target.position);
    }
}