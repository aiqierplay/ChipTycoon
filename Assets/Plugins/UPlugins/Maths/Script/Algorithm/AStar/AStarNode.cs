using System.Collections.Generic;

namespace Aya.Maths
{
    public abstract class AStarNode
    {
        // AStar 算法的节点类型
        public AStarItemType ItemType;

        // 父节点
        public AStarNode Parent;

        // start -> end 的消耗
        // F = G + H
        public float F;

        // start -> n 的消耗
        public float G;

        // n -> end 的消耗
        public float H;

        // 根据起点计算 G 值
        public abstract float GetG(AStarNode startNode);

        // 根据终点计算 H 值
        public abstract float GetH(AStarNode endNode);

        // 计算 F = G + H
        public virtual float GetF()
        {
            return G + H;
        }

        // 可到达的邻居节点，可以依据上一个经过节点做出特殊处理
        public abstract IList<AStarNode> GetNeighbors(AStarNode preview);

        /*
        
        关于 G / H / F 的一种参考实现

        public int CalcG(IAStartItem start)
        {
            var point = this;
            var startPoint = start as CellItem;
            var g = (Mathf.Abs(point.X - startPoint.X) + Mathf.Abs(point.Y - startPoint.Y));
            var parentG = point.Parent != null ? point.Parent.G : 0;
            return g + parentG;
        }

        public int CalcH(IAStartItem end)
        {
            var point = this;
            var endPoint = end as CellItem;
            var step = Mathf.Abs(point.X - endPoint.X) + Mathf.Abs(point.Y - endPoint.Y);
            return step;
        }
        */
    }
}