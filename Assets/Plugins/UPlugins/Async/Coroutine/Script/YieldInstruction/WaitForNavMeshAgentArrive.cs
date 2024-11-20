using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace Aya.Async
{
    public class WaitForNavMeshAgentArrive : CustomYieldInstruction
    {
        public NavMeshAgent NavMeshAgent;

        public override bool keepWaiting => !IsArrive(NavMeshAgent);

        public WaitForNavMeshAgentArrive(NavMeshAgent navMeshAgent)
        {
            NavMeshAgent = navMeshAgent;
        }

        public static bool IsArrive(NavMeshAgent navMeshAgent)
        {
            if (!navMeshAgent.enabled) return false;
            return !navMeshAgent.pathPending && navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance;
        }
    }

    public static partial class YieldBuilder
    {
        public static IEnumerator WaitForNavMeshAgentArrive(NavMeshAgent navMeshAgent)
        {
            while (!Async.WaitForNavMeshAgentArrive.IsArrive(navMeshAgent))
            {
                yield return null;
            }
        }
    }
}