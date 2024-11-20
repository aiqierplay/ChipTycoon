using UnityEngine;
using MoreMountains.NiceVibrations;

namespace Fishtail.PlayTheBall.Vibration
{
    public class VibrationController : MonoBehaviour
    {
        public static VibrationController Instance { get; private set; }
        public static float Interval = 0.1f;

        public static bool Active = true;

        private float _lastTime = -1f;

        private void Awake()
        {
            Instance = this;
            MMVibrationManager.iOSInitializeHaptics();
        }

        private void OnDestroy()
        {
            MMVibrationManager.iOSReleaseHaptics();
        }

        public void Impact()
        {
            Impact(HapticTypes.LightImpact);
        }

        public void Impact(HapticTypes type)
        {
            if (!Active) return;
            var currentTime = Time.realtimeSinceStartup;
            if (currentTime - _lastTime < Interval) return;
            _lastTime = currentTime;
            MMVibrationManager.Haptic(type);
        }
    }
}