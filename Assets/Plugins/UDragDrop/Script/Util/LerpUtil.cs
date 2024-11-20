using UnityEngine;

namespace Aya.DragDrop
{
    public enum LerpType
    {
        None = 0,
        Linear = 1,
        MoveTowards = 2,
        Custom = 100,
    }

    public static class LerpUtil
    {
        public static Vector3 Lerp(LerpType type, Vector3 from, Vector3 to, float factor)
        {
            switch (type)
            {
                case LerpType.None:
                    return to;
                case LerpType.Linear:
                    return Linear(from, to, factor);
                case LerpType.MoveTowards:
                    return MoveTowards(from, to, factor);
                case LerpType.Custom:
                    break;
            }

            return to;
        }

        public static Vector3 Linear(Vector3 from, Vector3 to, float factor)
        {
            return Vector3.Lerp(from, to, factor);
        }

        public static Vector3 MoveTowards(Vector3 from, Vector3 to, float factor)
        {
            return Vector3.MoveTowards(from, to, factor);
        }
    }
}