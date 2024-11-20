using System.Collections.Generic;
using UnityEngine;

namespace Aya.Maths
{
    public class AStarNodeComparer : IComparer<AStarNode>
    {
        public int Compare(AStarNode node1, AStarNode node2)
        {
            // if (node1 == null || node2 == null) return 0;
            if (node1.F != node2.F)
            {
                return node1.F.CompareTo(node2.F);
            }

            if (node1.H != node2.H)
            {
                return node1.H.CompareTo(node2.H);
            }

            return 0;
        }
    }
}