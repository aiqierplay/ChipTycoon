using System;
using UnityEngine;

namespace Aya.DragDrop
{
    [Serializable]
    public class DragLerpData
    {
        public LerpType LerpType = LerpType.None;
        public float LerpDuration = 0.2f;
        public bool TimeScale = true;

        public virtual float DeltaTime => TimeScale ? Time.deltaTime : Time.unscaledDeltaTime;
    }
}