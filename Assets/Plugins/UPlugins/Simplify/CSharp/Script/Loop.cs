using System;
using System.Collections.Generic;

namespace Aya.Simplify
{
    public static class Loop
    {
        #region For

        public static void For(int count, Action action)
        {
            For(0, count - 1, action);
        }

        public static void For(int count, Action<int> action)
        {
            For(0, count - 1, action);
        }

        public static void For(int from, int to, Action action)
        {
            For(from, to, 1, action);
        }

        public static void For(int from, int to, int step, Action action)
        {
            for (var i = from; i <= to; i += step)
            {
                action();
            }
        }

        public static void For(int from, int to, Action<int> action)
        {
            For(from, to, 1, action);
        }

        public static void For(int from, int to, int step, Action<int> action)
        {
            for (var i = from; i <= to; i += step)
            {
                action(i);
            }
        }

        #endregion

        #region For^2

        public static void For(int count, Action<int, int> action)
        {
            For(0, count - 1, i => { For(0, count - 1, j => { action(i, j); }); });
        }

        public static void For(int from, int to, Action<int, int> action)
        {
            For(from, to, i => { For(from, to, j => { action(i, j); }); });
        }

        #endregion

        #region While

        public static void While(Func<bool> condition, Action action)
        {
            var i = 0;
            while (condition())
            {
                action();
                i++;
            }
        }

        public static void While(Func<int, bool> condition, Action action)
        {
            var i = 0;
            while (condition(i))
            {
                action();
                i++;
            }
        }

        public static void While(Func<bool> condition, Action<int> action)
        {
            var i = 0;
            while (condition())
            {
                action(i);
                i++;
            }
        }

        public static void While(Func<int, bool> condition, Action<int> action)
        {
            var i = 0;
            while (condition(i))
            {
                action(i);
                i++;
            }
        }

        #endregion

        #region Do

        public static void Do(Func<bool> condition, Action action)
        {
            var i = 0;
            do
            {
                action();
                i++;
            } while (condition());
        }

        public static void Do(Func<int, bool> condition, Action action)
        {
            var i = 0;
            do
            {
                action();
                i++;
            } while (condition(i));
        }

        public static void Do(Func<bool> condition, Action<int> action)
        {
            var i = 0;
            do
            {
                action(i);
                i++;
            } while (condition());
        }

        public static void Do(Func<int, bool> condition, Action<int> action)
        {
            var i = 0;
            do
            {
                action(i);
                i++;
            } while (condition(i));
        }

        #endregion

        #region Foreach

        public static void Foreach<T>(Action<T> action, params IEnumerable<T>[] sources)
        {
            foreach (var source in sources)
            {
                foreach (var item in source)
                {
                    action(item);
                }
            }
        }

        #endregion
    }
}
