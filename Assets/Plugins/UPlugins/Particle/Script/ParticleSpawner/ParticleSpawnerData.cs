/////////////////////////////////////////////////////////////////////////////
//
//  Script   : ParticleSpawnerData.cs
//  Info     : 粒子生成器数据
//  Author   : ls9512
//  E-mail   : ls9512@vip.qq.com
//
//  Copyright : Aya Game Studio 2023
//
/////////////////////////////////////////////////////////////////////////////
using System.Collections.Generic;
using Aya.Extension;
using UnityEngine;

namespace Aya.Particle
{
    public class ParticleSpawnerData
    {
        public ParticleSpawner ParticleSpawner;
        public GameObject ParticlePrefab;
        public GameObject ParticleInstance;

        public bool IsCached;

        public ParticleSystem[] Particles;
        public Renderer[] RendererList;
        public TrailRenderer[] TrailRendererList;
        public LineRenderer[] LineRendererList;

        public RealTimeParticle RealTime;
        public RewindParticle Rewind;

        public float ParticleDuration;
        public float RuntimeDuration;

        public List<ParticleScaleData> ScaleDataList;

        public static Dictionary<GameObject, ParticleSpawnerData> ParticleSpawnerDataDic = new Dictionary<GameObject, ParticleSpawnerData>();

        public static ParticleSpawnerData GetParticleSpawnerData(GameObject particleInstance)
        {
            if (!ParticleSpawnerDataDic.TryGetValue(particleInstance, out var data))
            {
                data = new ParticleSpawnerData(particleInstance);
                ParticleSpawnerDataDic.Add(particleInstance, data);
            }

            return data;
        }

        public ParticleSpawnerData(GameObject particleInstance)
        {
            ParticleInstance = particleInstance;
            Cache();
        }

        public void Cache()
        {
            if (IsCached) return;
            IsCached = true;
            Particles = ParticleInstance.GetComponentsInChildren<ParticleSystem>(false);
            RendererList = ParticleInstance.GetComponentsInChildren<Renderer>(false);
            TrailRendererList = ParticleInstance.GetComponentsInChildren<TrailRenderer>(false);
            LineRendererList = ParticleInstance.GetComponentsInChildren<LineRenderer>(false);

            ParticleDuration = ParticleInstance.GetParticleDuration();

            RealTime = ParticleInstance.GetComponent<RealTimeParticle>();
            Rewind = ParticleInstance.GetComponent<RewindParticle>();

            ScaleDataList = new List<ParticleScaleData>();
            for (var i = 0; i < Particles.Length; i++)
            {
                var particle = Particles[i];
                ScaleDataList.Add(new ParticleScaleData()
                {
                    Transform = particle.transform,
                    BeginScale = particle.transform.localScale
                });
            }
        }

        public void Init(ParticleSpawner spawner, GameObject prefab)
        {
            ParticleSpawner = spawner;
            ParticlePrefab = prefab;
            ParticleInstance.SetActive(true);

            ParticleInstance.transform.ResetLocal();
            if (Mathf.Abs(ParticleSpawner.Duration) < 1e-6)
            {
                RuntimeDuration = ParticleDuration;
            }
            else
            {
                RuntimeDuration = ParticleSpawner.Duration;
            }

            if (ParticleSpawner.RealTime && RealTime != null)
            {
                RealTime.Load(true);
            }

            if (ParticleSpawner.Rewind && Rewind != null)
            {
                Rewind.Load(true);
            }

            ParticleInstance.SetActive(false);
        }


        public void DeSpawn(bool deSpawnSpawner = true)
        {
            if (ParticleInstance != null)
            {
                ParticleSpawner.EntityPool.DeSpawn(ParticleInstance);
            }

            if (Particles != null && Particles.Length > 0)
            {
                Particles = null;
            }
        }
    }
}