using System;
using Aya.Extension;
using UnityEngine;
using UnityEngine.AI;

public abstract partial class EntityBase
{
    #region Nav Mesh Agent
  
    [GetComponentInChildren, NonSerialized] public NavMeshAgent NavMeshAgent;
    public bool IsNavMeshMoving => NavMeshAgent != null && !NavMeshAgent.isStopped;

    public void NavMoveTo(Vector3 position, float speed, float stopDistance, Action onDone = null)
    {
        if (NavMeshAgent == null) return;

        NavMeshAgent.speed = speed;
        NavMeshAgent.stoppingDistance = stopDistance;
        NavMeshAgent.destination = position;

        if (onDone != null)
        {
            this.ExecuteWhen(() =>
            {
                onDone?.Invoke();
            }, () => NavMeshAgent.IsArrive());
        }
    }

    #endregion

    #region Behaviour Tree



    #endregion

    #region Steering Behaviour



    #endregion
}
