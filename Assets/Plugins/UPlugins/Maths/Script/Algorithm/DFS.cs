/////////////////////////////////////////////////////////////////////////////
//
//  Script   : DFS.cs
//  Info     : 深度优先搜索 寻路算法
//  Author   : ls9512
//  E-mail   : ls9512@vip.qq.com
//
//  Copyright : Aya Game Studio 2021
//
/////////////////////////////////////////////////////////////////////////////
using System;
using System.Collections.Generic;

namespace Aya.Maths
{
    public static class DFS
    {
        internal static bool Find = false;

        public static (bool, List<T>) Search<T>(List<T> startList, Func<T, IEnumerable<T>> nextNodesGetter, Predicate<T> finishPredicate, int maxDepth = 10)
        {
            Find = false;
            var last = startList[startList.Count - 1];
            SearchRecursion(startList, last, nextNodesGetter, finishPredicate, 1, maxDepth);
            return (Find, startList);
        }

        public static (bool, List<T>) Search<T>(T startNode, Func<T, IEnumerable<T>> nextNodesGetter, Predicate<T> finishPredicate, int maxDepth = 10)
        {
            Find = false;
            var result = new List<T>();
            SearchRecursion(result, startNode, nextNodesGetter, finishPredicate, 1, maxDepth);
            return (Find, result);
        }

        internal static void SearchRecursion<T>(List<T> result, T currentNode, Func<T, IEnumerable<T>> nextNodesGetter, Predicate<T> finishPredicate, int depth, int maxDepth)
        {
            if (!result.Contains(currentNode))
            {
                result.Add(currentNode);
            }

            if (Find) return;
            if (depth >= maxDepth) return;
            if (finishPredicate(currentNode))
            {
                Find = true;
                return;
            }

            var nextItems = nextNodesGetter(currentNode);
            foreach (var nextItem in nextItems)
            {
                if (result.Contains(nextItem)) continue;
                SearchRecursion(result, nextItem, nextNodesGetter, finishPredicate, depth + 1, maxDepth);
                if (Find) return;
            }

            result.RemoveAt(result.Count - 1);
        }
    }
}