using System;
using UnityEngine;

#if UNITY_EDITOR
#endif

namespace Aya.TweenPro
{
    [Tweener("Sub Animation", "Misc", "cs Script Icon")]
    [Serializable]
    public partial class TweenSubAnimation : TweenValueFloat<UTweenPlayer>
    {
        public override float Value
        {
            get => Target.Animation.NormalizedProgress;
            set
            {
                if (Target == Animation.TweenPlayer) return;
                Target.Animation.NormalizedProgress = value;
                Target.Animation.Sample(Target.Animation.NormalizedProgress);
            }
        }

        public override void PrepareSample()
        {
            base.PrepareSample();
            Target.Animation.IsSubAnimation = true;
            Target.Animation.PreSample();
        }

        public override void StopSample()
        {
            base.StopSample();
            Target.Animation.StopSample();
        }
    }

#if UNITY_EDITOR

    public partial class TweenSubAnimation : TweenValueFloat<UTweenPlayer>
    {
        public override void InitParam(TweenAnimation animation, MonoBehaviour target)
        {
            base.InitParam(animation, target);
            Target = null;
        }

        public override void DrawTarget()
        {
            base.DrawTarget();
            if (Target == Animation.TweenPlayer && Target != null)
            {
                GUIUtil.DrawTipArea(UTweenEditorSetting.Ins.ErrorColor, "Can't control self!");
            }
        }
    }

#endif

}