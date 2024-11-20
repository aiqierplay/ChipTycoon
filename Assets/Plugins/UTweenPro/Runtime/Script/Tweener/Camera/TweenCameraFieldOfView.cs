using System;
using UnityEngine;

namespace Aya.TweenPro
{
    [Tweener("Field Of View", nameof(Camera))]
    [Serializable]
    public class TweenCameraFieldOfView : TweenValueFloat<Camera>
    {
        public override bool RequireClampMin => true;
        public override float MinValue => 1e-5f;
        public override bool RequireClampMax => true;
        public override float MaxValue => 179;

        public override float Value
        {
            get => Target.fieldOfView;
            set => Target.fieldOfView = value;
        }
    }

    #region Extension

    public static partial class UTween
    {
        public static TweenCameraFieldOfView FieldOfView(Camera camera, float to, float duration)
        {
            var tweener = Play<TweenCameraFieldOfView, Camera, float>(camera, to, duration);
            return tweener;
        }

        public static TweenCameraFieldOfView FieldOfView(Camera camera, float from, float to, float duration)
        {
            var tweener = Play<TweenCameraFieldOfView, Camera, float>(camera, from, to, duration);
            return tweener;
        }
    }

    public static partial class CameraExtension
    {
        public static TweenCameraFieldOfView TweenFieldOfView(this Camera camera, float to, float duration)
        {
            var tweener = UTween.FieldOfView(camera, to, duration);
            return tweener;
        }

        public static TweenCameraFieldOfView TweenFieldOfView(this Camera camera, float from, float to, float duration)
        {
            var tweener = UTween.FieldOfView(camera, from, to, duration);
            return tweener;
        }
    }

    #endregion
}