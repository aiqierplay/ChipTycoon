#if UTWEEN_TIMELINE
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace Aya.TweenPro
{
    public class UTweenPlayable : PlayableBehaviour
    {
        public UTweenPlayer UTweenPlayer;

        public override void ProcessFrame(Playable playable, FrameData info, object playerData)
        {
            base.ProcessFrame(playable, info, playerData);
            
        }
    }
}
#endif