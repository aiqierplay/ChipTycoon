using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Aya.UI.Scroll
{
    [RequireComponent(typeof(RectTransform))]
    public abstract class ScrollBase : MonoBehaviour
    {
        public RectTransform Rect
        {
            get
            {
                if (_rect == null) _rect = GetComponent<RectTransform>();
                return _rect;
            }
        }

        private RectTransform _rect;


        public virtual void Awake()
        {
            
        }

        public virtual void Calculate()
        {

        }

        public virtual void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            var corners = new Vector3[4];
            Rect.GetWorldCorners(corners);
            Gizmos.DrawLine(corners[0], corners[1]);
            Gizmos.DrawLine(corners[1], corners[2]);
            Gizmos.DrawLine(corners[2], corners[3]);
            Gizmos.DrawLine(corners[3], corners[0]);
        }
    }
}