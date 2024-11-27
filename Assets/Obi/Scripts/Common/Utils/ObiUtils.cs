using UnityEngine;

        public static void DebugDrawBox(Vector3 pos, Quaternion rot, Vector3 scale, Color c)
        {
            // create matrix
            Matrix4x4 m = new Matrix4x4();
            m.SetTRS(pos, rot, scale);

            var point1 = m.MultiplyPoint(new Vector3(-0.5f, -0.5f, 0.5f));
            var point2 = m.MultiplyPoint(new Vector3(0.5f, -0.5f, 0.5f));
            var point3 = m.MultiplyPoint(new Vector3(0.5f, -0.5f, -0.5f));
            var point4 = m.MultiplyPoint(new Vector3(-0.5f, -0.5f, -0.5f));

            var point5 = m.MultiplyPoint(new Vector3(-0.5f, 0.5f, 0.5f));
            var point6 = m.MultiplyPoint(new Vector3(0.5f, 0.5f, 0.5f));
            var point7 = m.MultiplyPoint(new Vector3(0.5f, 0.5f, -0.5f));
            var point8 = m.MultiplyPoint(new Vector3(-0.5f, 0.5f, -0.5f));

            Debug.DrawLine(point1, point2, c);
            Debug.DrawLine(point2, point3, c);
            Debug.DrawLine(point3, point4, c);
            Debug.DrawLine(point4, point1, c);

            Debug.DrawLine(point5, point6, c);
            Debug.DrawLine(point6, point7, c);
            Debug.DrawLine(point7, point8, c);
            Debug.DrawLine(point8, point5, c);

            Debug.DrawLine(point1, point5, c);
            Debug.DrawLine(point2, point6, c);
            Debug.DrawLine(point3, point7, c);
            Debug.DrawLine(point4, point8, c);
        }

            return MergeBatches(batches, batches.Count, true);

        public unsafe static Vector3 OctDecode(float k)
        {
            uint d = *(uint*)&k;
            Vector2 f = new Vector2((d >> 16) / 65535f, (d & 0xffff) / 65535f);
            f.x = f.x * 2 - 1;
            f.y = f.y * 2 - 1;

            Vector3 n = new Vector3(f.x, f.y, 1.0f - Mathf.Abs(f.x) - Mathf.Abs(f.y));
            float t = Mathf.Max(-n.z, 0);
            n.x += n.x >= 0.0f ? -t : t;
            n.y += n.y >= 0.0f ? -t : t;
            return Vector3.Normalize(n);
        }

        public unsafe static Vector4 UnpackFloatRGBA(float v)
        {
            uint rgba = *(uint*)&v;
            float r = ((rgba & 0xff000000) >> 24) / 255f;
            float g = ((rgba & 0x00ff0000) >> 16) / 255f;
            float b = ((rgba & 0x0000ff00) >> 8) / 255f;
            float a = (rgba & 0x000000ff) / 255f;
            return new Vector4(r, g, b, a);
        }

        public unsafe static float PackFloatRGBA(Vector4 enc)
        {
            uint rgba = ((uint)(enc.x * 255f) << 24) +
                        ((uint)(enc.y * 255f) << 16) +
                        ((uint)(enc.z * 255f) << 8) +
                        (uint)(enc.w * 255f);
            return *(float*)&rgba;
        }

        public unsafe static Vector2 UnpackFloatRG(float v)
        {
            uint rgba = *(uint*)&v;
            float r = ((rgba & 0xffff0000) >> 16) / 65535f;
            float g = (rgba & 0x0000ffff) / 65535f;
            return new Vector2(r, g);
        }

        public unsafe static float PackFloatRG(Vector2 enc)
        {
            uint rgba = ((uint)(enc.x * 65535f) << 16) +
                        (uint)(enc.y * 65535f);
            return *(float*)&rgba;
        }