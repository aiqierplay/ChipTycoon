using System;
using System.Collections.Generic;
using UnityEngine;

namespace Aya.Maths.Sample
{
    public class MapNode : AStarNode
    {
        [NonSerialized] public Map Map;
        [NonSerialized] public int X;
        [NonSerialized] public int Y;
        [NonSerialized] public IList<AStarNode> NeighborList;

        public void Init(Map map, int x, int y)
        {
            Map = map;
            X = x;
            Y = y;
            ItemType = AStarItemType.Normal;
        }

        public void CacheNeighbors()
        {
            NeighborList = new List<AStarNode>();
            if (Y >= 1) NeighborList.Add(Map.MapArray[X, Y - 1]);
            if (Y < Map.Height - 1) NeighborList.Add(Map.MapArray[X, Y + 1]);
            if (X >= 1) NeighborList.Add(Map.MapArray[X - 1, Y]);
            if (X < Map.Width - 1) NeighborList.Add(Map.MapArray[X + 1, Y]);

            if (X >= 1 && Y >= 1) NeighborList.Add(Map.MapArray[X - 1, Y - 1]);
            if (X >= 1 && Y < Map.Height - 1) NeighborList.Add(Map.MapArray[X - 1, Y + 1]);
            if (X < Map.Width - 1 && Y >= 1) NeighborList.Add(Map.MapArray[X + 1, Y - 1]);
            if (X < Map.Width - 1 && Y < Map.Height - 1) NeighborList.Add(Map.MapArray[X + 1, Y + 1]);
        }

        #region AStar

        public override float GetG(AStarNode startNode)
        {
            var node = startNode as MapNode;
            if (node.X == X || node.Y == Y)
            {
                return Math.Abs(node.X - X) + Math.Abs(node.Y - Y);
            }

            return (float)Math.Sqrt(Math.Pow(node.X - X, 2) + Math.Pow(node.Y - Y, 2));
        }

        public override float GetH(AStarNode endNode)
        {
            var node = endNode as MapNode;
            return Math.Abs(node.X - X) + Mathf.Abs(node.Y - Y);
        }

        public override IList<AStarNode> GetNeighbors(AStarNode preview)
        {
            return NeighborList;
        }

        #endregion
    }
}