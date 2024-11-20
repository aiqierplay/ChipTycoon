/////////////////////////////////////////////////////////////////////////////
//
//  Script   : ParticleSpawner.cs
//  Info     : 粒子生成器
//  Author   : ls9512
//  E-mail   : ls9512@vip.qq.com
//
//  Copyright : Aya Game Studio 2018
//
/////////////////////////////////////////////////////////////////////////////
using System;
using UnityEngine;
using Aya.Extension;
using Aya.Pool;

namespace Aya.Particle
{
    public class ParticleSpawner : MonoBehaviour
    {
        public float Duration;
        public bool RealTime = false;
        public bool AutoPlay = false;
        public bool AutoClear = true;
        public bool AutoDeSpawn = true;
        public bool Rewind = false;
        public Vector3 ScaleVector;
        public float ScaleValue;
        public bool ClearTrailOnEnable = true;

        [NonSerialized] public RectTransform RectTransform;
        [NonSerialized] public ParticleSpawnerData Data;
        [NonSerialized] public bool AutoDeSpawnParticleSpawner = false;
        [NonSerialized] public bool IsPlaying;
        [NonSerialized] public float PlayTimer;

        public static EntityPool EntityPool => PoolManager.Ins["Effect"];

        private Coroutine _fadeCoroutine;

        #region Static API

        public static ParticleSpawner Spawn(GameObject particlePrefab, Transform transform, Vector3 position, bool autoPlay = true, bool autoDeSpawn = true, bool realTime = false)
        {
            var spawner = GetParticleSpawner(particlePrefab, transform);
            spawner.Duration = 0f;
            spawner.AutoPlay = autoPlay;
            spawner.RealTime = realTime;
            spawner.AutoDeSpawn = autoDeSpawn;
            spawner.AutoDeSpawnParticleSpawner = true;

            if (transform is RectTransform trans && spawner.RectTransform != null)
            {
                spawner.RectTransform.anchoredPosition = position;
            }
            else
            {
                spawner.transform.position = position;
            }

            spawner.Data.Init(spawner, particlePrefab);

            if (autoPlay)
            {
                spawner.Play();
            }

            return spawner;
        }

        public static ParticleSpawner Spawn(GameObject particlePrefab, Transform transform, Vector3 position, float duration, bool autoPlay = true, bool autoDeSpawn = true, bool realTime = false)
        {
            var spawner = GetParticleSpawner(particlePrefab, transform);
            spawner.Duration = duration;
            spawner.AutoPlay = autoPlay;
            spawner.RealTime = realTime;
            spawner.AutoDeSpawn = autoDeSpawn;
            spawner.AutoDeSpawnParticleSpawner = true;
            spawner.transform.position = position;
            spawner.Data.Init(spawner, particlePrefab);

            if (autoPlay)
            {
                spawner.Play();
            }

            return spawner;
        }
        protected static ParticleSpawner GetParticleSpawner(GameObject particlePrefab, Transform transform)
        {
            var spawnerPrefab = Resources.Load<ParticleSpawner>(nameof(ParticleSpawner));
            var spawnerInstance = EntityPool.Spawn(spawnerPrefab, transform);
            var particleInstance = EntityPool.Spawn(particlePrefab, spawnerInstance.transform);
            spawnerInstance.Data = ParticleSpawnerData.GetParticleSpawnerData(particleInstance);
            return spawnerInstance;
        }

        #endregion

        #region Monobehaviour

        public void Awake()
        {
            RectTransform = GetComponent<RectTransform>();
        }

        public void OnEnable()
        {
            if (AutoClear)
            {
                Clear();
            }

            if (ClearTrailOnEnable)
            {
                ClearTrail();
            }
        }

        public void Update()
        {
            if (IsPlaying)
            {
                PlayTimer += Time.deltaTime;
                if (PlayTimer >= Data.RuntimeDuration + 0.1f)
                {
                    DeSpawn(AutoDeSpawnParticleSpawner);
                }
            }
        }

        public void OnDisable()
        {
            // DeSpawn();
            if (ClearTrailOnEnable)
            {
                ClearTrail();
            }
        }

        public void Reset()
        {
            ScaleVector = Vector3.one;
            ScaleValue = 1f;
        }

        #endregion

        #region Scale

        public void SetScale(float scale)
        {
            ScaleValue = scale;
            ScaleVector = Vector3.one;
            AdapterScale();
        }

        public void SetScale(Vector3 scale)
        {
            ScaleValue = 1f;
            ScaleVector = scale;
            AdapterScale();
        }

        public void AdapterScale()
        {
            if (Data == null) return;
            if (Data.ScaleDataList == null)
            {
                Data.Cache();
            }

            if (Data.ScaleDataList != null)
            {
                foreach (var data in Data.ScaleDataList)
                {
                    var scale = new Vector3(data.BeginScale.x * ScaleVector.x, data.BeginScale.y * ScaleVector.y, data.BeginScale.z * ScaleVector.z);
                    scale *= ScaleValue;
                    if (data.Transform != null)
                    {
                        data.Transform.localScale = scale;
                    }
                }
            }
        }

        #endregion

        #region Trail Renderer / Line Renderer

        public void ClearTrail()
        {
            if (Data == null) return;
            if (Data.TrailRendererList != null)
            {
                for (var i = 0; i < Data.TrailRendererList.Length; i++)
                {
                    var trailRenderer = Data.TrailRendererList[i];
                    trailRenderer.Clear();
                }
            }

            if (Data.LineRendererList != null)
            {
                for (var i = 0; i < Data.LineRendererList.Length; i++)
                {
                    var lineRenderer = Data.LineRendererList[i];
                    lineRenderer.Clear();
                }
            }
        }

        #endregion

        #region Load / Unload

        public void DeSpawn(bool deSpawnSpawner = true)
        {
            IsPlaying = false;
            Data.DeSpawn(deSpawnSpawner);
            if (deSpawnSpawner)
            {
                Data.ParticlePrefab = null;
                EntityPool.DeSpawn(this);
            }

            Data = null;
        }

        #endregion

        #region Play / Stop

        public void Play()
        {
            if (Data == null) return;
            if (!gameObject.activeSelf)
            {
                gameObject.SetActive(true);
            }

            // 粒子特效位于UI上，则需要处理排序和UI缩放适配
            // var uiBehaviour = Instance.FindComponentInParents<UIBehaviour>();
            // if (uiBehaviour != null)
            // {
            //     UISortingOrder.Sort(Instance);
            //     var uiParticleScaler = Instance.GetOrAddComponent<UIParticleAdapter>();
            //     uiParticleScaler.SetDesignSize(UIController.Ins.DesignWidth, UIController.Ins.DesignHeight);
            // }

            Data.ParticleInstance.SetActive(false);
            Data.ParticleInstance.SetActive(true);

            if (Data.RuntimeDuration > 0f && AutoDeSpawn)
            {
                IsPlaying = true;
                PlayTimer = 0f;
            }
        }

        public void Stop()
        {
            DeSpawn(AutoDeSpawnParticleSpawner);
        }

        public void Clear()
        {
            if (Data == null) return;
            if (Data.Particles == null) return;
            for (var i = 0; i < Data.Particles.Length; i++)
            {
                var particle = Data.Particles[i];
                particle.Clear();
            }
        }

        #endregion

        #region Fade In / Out

        public void FadeIn(float time = 0)
        {
            AutoDeSpawn = false;
            Play();
            if (_fadeCoroutine != null) StopCoroutine(_fadeCoroutine);
            _fadeCoroutine = this.ExecuteDelay(
                () =>
                {
                    for (var i = 0; i < Data.Particles.Length; i++)
                    {
                        var particle = Data.Particles[i];
                        var emission = particle.emission;
                        emission.enabled = true;
                    }
                }, time);
        }

        public void FadeOut(float time = 0)
        {
            if (Data.Particles == null) return;
            if (_fadeCoroutine != null) StopCoroutine(_fadeCoroutine);
            _fadeCoroutine = this.ExecuteDelay(
                () =>
                {
                    for (var i = 0; i < Data.Particles.Length; i++)
                    {
                        var particle = Data.Particles[i];
                        var emission = particle.emission;
                        emission.enabled = false;
                    }
                }, time);
        }

        #endregion
    }
}