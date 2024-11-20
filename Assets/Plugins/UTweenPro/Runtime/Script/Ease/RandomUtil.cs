using UnityEngine;

namespace Aya.TweenPro
{
    public static class RandomUtil
    {
        public static System.Random Random = new System.Random();

        public static bool RandomBoolean()
        {
            return Random.Next(0, 100) >= 50;
        }

        public static int RandomInt(int from, int to)
        {
            return Random.Next(from, to);
        }

        public static long RandomLong(long from, long to)
        {
            return Random.Next((int)from, (int)to);
        }

        public static float RandomFloat(float from, float to)
        {
            return (float)(Random.NextDouble() * (to - from) + from);
        }

        public static double RandomDouble(double from, double to)
        {
            return Random.NextDouble() * (to - from) + from;
        }

        public static BoundsInt RandomBoundsInt(BoundsInt from, BoundsInt to)
        {
            var x = RandomInt(from.position.x, to.position.x);
            var y = RandomInt(from.position.y, to.position.y);
            var z = RandomInt(from.position.z, to.position.z);
            var sizeX = RandomInt(from.size.x, to.size.x);
            var sizeY = RandomInt(from.size.y, to.size.y);
            var sizeZ = RandomInt(from.size.z, to.size.z);
            var result = new BoundsInt(new Vector3Int(x, y, z), new Vector3Int(sizeX, sizeY, sizeZ));
            return result;
        }

        public static Bounds RandomBounds(Bounds from, Bounds to)
        {
            var x = RandomFloat(from.center.x, to.center.x);
            var y = RandomFloat(from.center.y, to.center.y);
            var z = RandomFloat(from.center.z, to.center.z);
            var sizeX = RandomFloat(from.size.x, to.size.x);
            var sizeY = RandomFloat(from.size.y, to.size.y);
            var sizeZ = RandomFloat(from.size.z, to.size.z);
            var result = new Bounds(new Vector3(x, y, z), new Vector3(sizeX, sizeY, sizeZ));
            return result;
        }

        public static Color RandomColor(Color from, Color to)
        {
            var r = RandomFloat(from.r, to.r);
            var g = RandomFloat(from.g, to.g);
            var b = RandomFloat(from.b, to.b);
            var a = RandomFloat(from.a, to.a);
            var result = new Color(r, g, b, a);
            return result;
        }

        public static Vector2 RandomVector2(Vector2 from, Vector2 to)
        {
            var x = RandomFloat(from.x, to.x);
            var y = RandomFloat(from.y, to.y);
            var result = new Vector2(x, y);
            return result;
        }

        public static Vector2Int RandomVector2Int(Vector2Int from, Vector2Int to)
        {
            var x = RandomInt(from.x, to.x);
            var y = RandomInt(from.y, to.y);
            var result = new Vector2Int(x, y);
            return result;
        }

        public static Vector3 RandomVector3(Vector3 from, Vector3 to)
        {
            var x = RandomFloat(from.x, to.x);
            var y = RandomFloat(from.y, to.y);
            var z = RandomFloat(from.z, to.z);
            var result = new Vector3(x, y, z);
            return result;
        }

        public static Vector3Int RandomVector3Int(Vector3Int from, Vector3Int to)
        {
            var x = RandomInt(from.x, to.x);
            var y = RandomInt(from.y, to.y);
            var z = RandomInt(from.z, to.z);
            var result = new Vector3Int(x, y, z);
            return result;
        }

        public static Vector4 RandomVector4(Vector4 from, Vector4 to)
        {
            var x = RandomFloat(from.x, to.x);
            var y = RandomFloat(from.y, to.y);
            var z = RandomFloat(from.z, to.z);
            var w = RandomFloat(from.w, to.w);
            var result = new Vector4(x, y, z, w);
            return result;
        }

        public static Quaternion RandomQuaternion(Quaternion from, Quaternion to)
        {
            var fromEuler = from.eulerAngles;
            var toEuler = to.eulerAngles;
            var result = Quaternion.Euler(RandomVector3(fromEuler, toEuler));
            return result;
        }

        public static Rect RandomRect(Rect from, Rect to)
        {
            var x = RandomFloat(from.x, to.x);
            var y = RandomFloat(from.y, to.y);
            var width = RandomFloat(from.width, to.width);
            var height = RandomFloat(from.height, to.height);
            var result = new Rect(x, y, width, height);
            return result;
        }

        public static RectInt RandomRectInt(RectInt from, RectInt to)
        {
            var x = RandomInt(from.x, to.x);
            var y = RandomInt(from.y, to.y);
            var width = RandomInt(from.width, to.width);
            var height = RandomInt(from.height, to.height);
            var result = new RectInt(x, y, width, height);
            return result;
        }

        public static RectOffset RandomRectOffset(RectOffset from, RectOffset to)
        {
            var left = RandomInt(from.left, to.left);
            var right = RandomInt(from.right, to.right);
            var top = RandomInt(from.top, to.top);
            var bottom = RandomInt(from.bottom, to.bottom);
            var result = new RectOffset(left, right, top, bottom);
            return result;
        }
    }
}
