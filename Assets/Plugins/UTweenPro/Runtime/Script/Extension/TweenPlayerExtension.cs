
namespace Aya.TweenPro
{
    public static class TweenPlayerExtension
    {
        #region Play / Pasue / Resume / Stop / Sample

        public static UTweenPlayer Play(this UTweenPlayer tweenPlayer, bool forward = true)
        {
            tweenPlayer.Animation?.Play(forward);
            return tweenPlayer;
        }

        public static UTweenPlayer Play(this UTweenPlayer tweenPlayer, float initNormalizedProgress, bool forward = true)
        {
            tweenPlayer.Animation?.Play(initNormalizedProgress, forward);
            return tweenPlayer;
        }

        public static UTweenPlayer Pause(this UTweenPlayer tweenPlayer)
        {
            tweenPlayer.Animation?.Pause();
            return tweenPlayer;
        }

        public static UTweenPlayer Resume(this UTweenPlayer tweenPlayer)
        {
            tweenPlayer.Animation?.Resume();
            return tweenPlayer;
        }

        public static UTweenPlayer Stop(this UTweenPlayer tweenPlayer)
        {
            tweenPlayer.Animation?.Stop();
            return tweenPlayer;
        }

        public static UTweenPlayer PlayForward(this UTweenPlayer tweenPlayer)
        {
            tweenPlayer.Animation?.PlayForward();
            return tweenPlayer;
        }

        public static UTweenPlayer PlayBackward(this UTweenPlayer tweenPlayer)
        {
            tweenPlayer.Animation?.PlayBackward();
            return tweenPlayer;
        }

        public static UTweenPlayer Sample(this UTweenPlayer tweenPlayer, float normalizedDuration)
        {
            tweenPlayer.Animation?.Sample(normalizedDuration);
            return tweenPlayer;
        }

        #endregion
    }
}