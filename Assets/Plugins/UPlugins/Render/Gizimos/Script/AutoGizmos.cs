/////////////////////////////////////////////////////////////////////////////
//
//  Script   : AutoGizmos.cs
//  Info     : Gizmos 自动显示
//  Author   : ls9512
//  E-mail   : ls9512@vip.qq.com
//
//  Copyright : Aya Game Studio 2020
//
/////////////////////////////////////////////////////////////////////////////

using System;
using UnityEditor;
using UnityEngine;

namespace Aya.Render
{
    [ExecuteInEditMode]
    public class AutoGizmos : MonoBehaviour
    {
        public Color Color = Color.green;
        public bool IsWire = true;
        public bool IsWireFill = true;

        // 2D
        [NonSerialized] public BoxCollider2D BoxCollider2D;
        [NonSerialized] public CircleCollider2D CircleCollider2D;

        // 3D
        [NonSerialized] public BoxCollider BoxCollider;
        [NonSerialized] public SphereCollider SphereCollider;
        [NonSerialized] public MeshCollider MeshCollider;
        [NonSerialized] public CapsuleCollider CapsuleCollider;

        [NonSerialized] public Camera Camera;
        [NonSerialized] public float WireFillAlpha = 0.15f;

#if UNITY_EDITOR
        public void Awake()
        {
            BoxCollider2D = gameObject.GetComponent<BoxCollider2D>();
            CircleCollider2D = gameObject.GetComponent<CircleCollider2D>();

            BoxCollider = gameObject.GetComponent<BoxCollider>();
            SphereCollider = gameObject.GetComponent<SphereCollider>();
            MeshCollider = gameObject.GetComponent<MeshCollider>();
            CapsuleCollider = gameObject.GetComponent<CapsuleCollider>();
            Camera = gameObject.GetComponent<Camera>();
        }

        public void OnDrawGizmos()
        {
            // BoxCollider2D
            if (BoxCollider2D != null)
            {
                Gizmos.color = Color;
                Gizmos.matrix = transform.localToWorldMatrix;
                if (IsWire)
                {
                    Gizmos.DrawWireCube(BoxCollider2D.offset, BoxCollider2D.size);
                }

                if (IsWireFill)
                {
                    Gizmos.color = Color * WireFillAlpha;
                }

                if ((IsWire && IsWireFill) || !IsWire)
                {
                    Gizmos.DrawCube(BoxCollider2D.offset, BoxCollider2D.size);
                }
            }

            // CircleCollider2D
            if (CircleCollider2D != null)
            {
                Gizmos.color = Color;
                Gizmos.matrix = transform.localToWorldMatrix;
                if (IsWire)
                {
                    Gizmos.DrawWireSphere(CircleCollider2D.offset, CircleCollider2D.radius);
                }

                if (IsWireFill)
                {
                    Gizmos.color = Color * WireFillAlpha;
                }

                if ((IsWire && IsWireFill) || !IsWire)
                {
                    Gizmos.DrawSphere(CircleCollider2D.offset, CircleCollider2D.radius);
                }
            }

            // BoxCollider
            if (BoxCollider != null)
            {
                Gizmos.color = Color;
                Gizmos.matrix = transform.localToWorldMatrix;
                if (IsWire)
                {
                    Gizmos.DrawWireCube(BoxCollider.center, BoxCollider.size);
                }

                if (IsWireFill)
                {
                    Gizmos.color = Color * WireFillAlpha;
                }

                if ((IsWire && IsWireFill) || !IsWire)
                {
                    Gizmos.DrawCube(BoxCollider.center, BoxCollider.size);
                }
            }

            // SphereCollider
            if (SphereCollider != null)
            {
                Gizmos.color = Color;
                Gizmos.matrix = transform.localToWorldMatrix;
                if (IsWire)
                {
                    Gizmos.DrawWireSphere(SphereCollider.center, SphereCollider.radius);
                }

                if (IsWireFill)
                {
                    Gizmos.color = Color * WireFillAlpha;
                }

                if ((IsWire && IsWireFill) || !IsWire)
                {
                    Gizmos.DrawSphere(SphereCollider.center, SphereCollider.radius);
                }
            }

            // MeshCollider
            if (MeshCollider != null)
            {
                Gizmos.color = Color;
                Gizmos.matrix = transform.localToWorldMatrix;
                if (IsWire)
                {
                    Gizmos.DrawWireMesh(MeshCollider.sharedMesh, 0);
                }

                if (IsWireFill)
                {
                    Gizmos.color = Color * WireFillAlpha;
                }

                if ((IsWire && IsWireFill) || !IsWire)
                {
                    Gizmos.DrawMesh(MeshCollider.sharedMesh, 0);
                }
            }

            // CapsuleCollider
            if (CapsuleCollider != null)
            {
                Gizmos.color = Color;
                Gizmos.matrix = transform.localToWorldMatrix;
                DrawWireCapsule(CapsuleCollider.center, CapsuleCollider.radius, CapsuleCollider.height);
            }

            // Camera
            if (Camera != null && !Camera.orthographic)
            {
                Gizmos.color = Color;
                var posRotation = Matrix4x4.Translate(transform.position);
                var rotMatrix = Matrix4x4.Rotate(transform.rotation);
                Gizmos.matrix = posRotation * rotMatrix;
                Gizmos.DrawFrustum(Vector3.zero, Camera.fieldOfView, Camera.farClipPlane, Camera.nearClipPlane, Camera.aspect);
            }
        }

        public static void DrawWireCapsule(Vector3 center, float radius, float height)
        {
            Handles.color = Gizmos.color;
            var angleMatrix = Gizmos.matrix;

            using (new Handles.DrawingScope(angleMatrix))
            {
                var pointOffset = (height - (radius * 2F)) / 2F;

                //draw sideways
                Handles.DrawWireArc(center + Vector3.up * pointOffset, Vector3.left, Vector3.back, -180, radius);
                Handles.DrawLine(center + new Vector3(0, pointOffset, -radius), center + new Vector3(0, -pointOffset, -radius));
                Handles.DrawLine(center + new Vector3(0, pointOffset, radius), center + new Vector3(0, -pointOffset, radius));
                Handles.DrawWireArc(center + Vector3.down * pointOffset, Vector3.left, Vector3.back, 180, radius);
                //draw frontways
                Handles.DrawWireArc(center + Vector3.up * pointOffset, Vector3.back, Vector3.left, 180, radius);
                Handles.DrawLine(center + new Vector3(-radius, pointOffset, 0), center + new Vector3(-radius, -pointOffset, 0));
                Handles.DrawLine(center + new Vector3(radius, pointOffset, 0), center + new Vector3(radius, -pointOffset, 0));
                Handles.DrawWireArc(center + Vector3.down * pointOffset, Vector3.back, Vector3.left, -180, radius);
                //draw center
                Handles.DrawWireDisc(center + Vector3.up * pointOffset, Vector3.up, radius);
                Handles.DrawWireDisc(center + Vector3.down * pointOffset, Vector3.up, radius);
            }
        }
#endif
    }
}

