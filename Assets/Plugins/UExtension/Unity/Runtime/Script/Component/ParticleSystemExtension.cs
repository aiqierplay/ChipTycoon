using UnityEngine;

namespace Aya.Extension
{
    public static class ParticleSystemExtension
    {
        public static float GetDuration(this ParticleSystem particle, bool allowLoop = false)
        {
            if (!particle.emission.enabled) return 0f;
            if (particle.main.loop && !allowLoop)
            {
                return -1f;
            }
            
            return particle.main.duration + particle.main.startDelay.GetMaxValue() + particle.main.startLifetime.GetMaxValue();
        }
    }
}