/////////////////////////////////////////////////////////////////////////////
//
//  Script   : AStar.cs
//  Info     : A* 寻路算法
//  Author   : ls9512
//  E-mail   : ls9512@vip.qq.com
//
//  Copyright : Aya Game Studio 2018
//
/////////////////////////////////////////////////////////////////////////////
using System;
using System.Collections.Generic;

namespace Aya.Maths
{
    [Flags]
    public enum AStarItemType
    {
        Start = 1, // 起点
        End = 2, // 终点
        Normal = 4, // 正常
        Obstacle = 8, // 障碍物  
    }

    public static class AStar
    {
        // 地图节点数组
        public static List<AStarNode> Map;

        // 开启列表
        public static SortedSet<AStarNode> OpenList;
        // 结束列表
        public static HashSet<AStarNode> CloseList;

        // 起点
        public static AStarNode Start;

        // 终点
        public static AStarNode End;

        public static IList<AStarNode> Search(List<AStarNode> map)
        {
            var start = map.Find(item => item.ItemType == AStarItemType.Start);
            var end = map.Find(item => item.ItemType == AStarItemType.End);
            return Search(map, start, end);
        }

        public static IList<AStarNode> Search(List<AStarNode> map, AStarNode start, AStarNode end)
        {
            Map = map;
            Start = start;
            End = end;
            OpenList = new SortedSet<AStarNode>(new AStarNodeComparer());
            CloseList = new HashSet<AStarNode>();

            // 初始化寻路基础数据
            var count = map.Count;
            for (var i = 0; i < count; i++)
            {
                var item = map[i];
                item.Parent = null;
                item.G = 0;
                item.H = 0;
                item.F = 0;
            }

            var endItem = SearchPath();
            if (endItem == null)
            {
                // 没找到
                return null;
            }

            var retList = new List<AStarNode>();
            var itemTemp = End;
            while (itemTemp.Parent != null)
            {
                retList.Insert(0, itemTemp);
                itemTemp = itemTemp.Parent;
            }

            retList.Insert(0, Start);
            return retList;
        }

        private static AStarNode SearchPath()
        {
            AStarNode previewNode = null;
            // 将 起点 加入开放列表
            OpenList.Add(Start);

            while (OpenList.Count > 0)
            {
                // 取出开放列表中 F 最小的点，并加入关闭列表
                var current = OpenList.Min;
                if (current == End) return End;

                OpenList.Remove(current);
                CloseList.Add(current);

                // 找出相邻节点
                var neighbors = current.GetNeighbors(previewNode);
                var neighborCount = neighbors.Count;
                for (var i = 0; i < neighborCount; i++)
                {
                    var neighbor = neighbors[i];
                    if ((neighbor.ItemType & AStarItemType.Obstacle) != 0 || CloseList.Contains(neighbor))
                    {
                        continue;
                    }

                    var newG = current.G + neighbor.GetG(current);
                    if (OpenList.Contains(neighbor))
                    {
                        // 如果存在于开放列表，则计算 G，如果大于原值，不作处理，否则设置当前点为父节点
                        if (newG >= neighbor.G) continue;

                        // 重新添加以触发排序
                        OpenList.Remove(neighbor);
                        neighbor.Parent = current;
                        neighbor.G = newG;
                        if (neighbor.H == 0)
                        {
                            neighbor.H = neighbor.GetH(End);
                        }

                        neighbor.F = neighbor.GetF();
                        var result = OpenList.Add(neighbor);
                    }
                    else
                    {
                        // 如果不在开放列表，则加入
                        // 并设当前点为父节点，计算 G,H,F
                        neighbor.Parent = current;
                        neighbor.G = newG;
                        if (neighbor.H == 0)
                        {
                            neighbor.H = neighbor.GetH(End);
                        }

                        neighbor.F = neighbor.GetF();
                        previewNode = neighbor;
                        var result = OpenList.Add(neighbor);
                    }
                }
            }

            return null;
        }
    }
}