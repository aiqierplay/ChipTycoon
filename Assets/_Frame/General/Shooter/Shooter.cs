using Aya.Events;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum ShootDirectionMode
{
    Direction = 0,
    ShootPointForward = 1,
    TransForward = 2,
}

public class Shooter : EntityBase
{
    #region Shoot

    [Title("Shoot")]
    public List<GameObject> ShootFxList;
    public ShootBullet BulletPrefab;
    public Transform ShootPoint;

    public ShootDirectionMode DirectionMode;
    [ShowIf(nameof(DirectionMode), ShootDirectionMode.Direction)]
    public Vector3 Direction = Vector3.forward;
    [ShowIf(nameof(DirectionMode), ShootDirectionMode.TransForward)]
    public Transform ForwardTrans;

    [Title("Animation")]
    public UTweenPlayerReference TweenShoot;

    public string ShootClipName = "Shoot";

    [Title("Param")]
    public float BulletSpeed = 10f;
    public float ShootStartWait = 0.5f;
    public float BulletLifeTime = 2f;
    public float ShootCountPerSecond = 1f;
    public int BulletNum = 1;
    public int MagCount = 10;
    public float MagInterval = 1f;

    public UnityEvent OnShoot;

    public Action OnBeforeShootAction = delegate { };
    public Action OnShootAction = delegate { };
    public Action<ShootBullet> OnShootBulletAction = delegate { };
    public Action OnChangeMagStart = delegate { };
    public Action OnChangeMagComplete = delegate { };

    [GetComponentInChildren, NonSerialized] public List<TransPointGroup> ShootFormationList;

    private float _shootWaitTimer;
    private float _shootTimer;
    private int _magCounter;
    private float _changeMagTimer;

    public int MagRemain => MagCount - _magCounter;

    [NonSerialized] public bool ActiveShooting;
    [NonSerialized] public float ShootInterval = 1f;
    [NonSerialized] public Coroutine ShootCoroutine;
    [NonSerialized] public int BulletFormationIndex = -1;

    public void Init()
    {
        ActiveShooting = false;

        _shootWaitTimer = 0f;
        _shootTimer = ShootInterval;
        _magCounter = 0;
        _changeMagTimer = 0f;
        BulletFormationIndex = -1;

        RefreshShootParam(ShootCountPerSecond, BulletLifeTime);
    }

    public void StartShoot()
    {
        ActiveShooting = true;

        TweenShoot.Stop();
        if (ShootCoroutine != null)
        {
            StopCoroutine(ShootCoroutine);
        }

        ShootCoroutine = StartCoroutine(ShootCo());
    }

    public void StopShoot()
    {
        if (!ActiveShooting) return;
        ActiveShooting = false;

        if (Animator != null) Animator.speed = 1f;
        TweenShoot.Stop();

        StopCoroutine(ShootCoroutine);
        ShootCoroutine = null;
    }

    public IEnumerator ShootCo()
    {
        while (true)
        {
            if (!ActiveShooting) yield return null;

            // Start Wait
            if (_shootWaitTimer <= ShootStartWait)
            {
                _shootWaitTimer += DeltaTime;
                yield return null;
            }

            while (ActiveShooting)
            {
                // Shooting
                _shootTimer += DeltaTime;
                while (_shootTimer >= ShootInterval)
                {
                    // Change Mag
                    if (_magCounter >= MagCount && MagCount > 0)
                    {
                        OnChangeMagStart?.Invoke();
                        while (_changeMagTimer < MagInterval)
                        {
                            _changeMagTimer += DeltaTime;
                            yield return null;
                        }

                        _changeMagTimer = 0f;
                        _magCounter = 0;
                        _shootTimer = 0f;
                        OnChangeMagComplete?.Invoke();

                        break;
                    }

                    OnBeforeShootAction?.Invoke();
                    _shootTimer -= ShootInterval;
                    if (BulletNum > 1 && BulletFormationIndex >= 0)
                    {
                        var formation = ShootFormationList[BulletFormationIndex];
                        for (var i = 0; i < formation.Count; i++)
                        {
                            var bullet = Shoot();
                            var point = formation.PointList[i];
                            bullet.Position = ShootPoint.position + RendererTrans.InverseTransformPoint(point.position);
                            bullet.Forward = bullet.Direction = point.forward;
                        }
                    }
                    else
                    {
                        var bullet = Shoot();
                    }

                    OnShootAction?.Invoke();
                    _magCounter++;
                }

                yield return null;
            }
        }
    }

    public void RefreshShootParam()
    {
        RefreshShootParam(ShootCountPerSecond, BulletLifeTime);
    }

    public void RefreshShootParam(float shootCountPerSecond, float bulletLifeTime)
    {
        ShootCountPerSecond = shootCountPerSecond;
        ShootInterval = 1f / ShootCountPerSecond;
        BulletLifeTime = bulletLifeTime;
        _shootTimer = ShootInterval;
        _shootWaitTimer = 0f;

        if (Animator != null)
        {
            Animator.speed = 1f / ShootInterval;
        }

        if (TweenShoot.Value != null)
        {
            TweenShoot.Value.Animation.SelfScale = 1f / ShootInterval;
        }
    }

    public Vector3 GetShootDirection()
    {
        Vector3 direction;
        switch (DirectionMode)
        {
            case ShootDirectionMode.Direction:
                direction = Direction;
                break;
            case ShootDirectionMode.ShootPointForward:
                direction = ShootPoint.forward;
                break;
            case ShootDirectionMode.TransForward:
                direction = ForwardTrans.forward;
                break;
            default:
                direction = Vector3.forward;
                break;
        }

        return direction;
    }

    public ShootBullet Shoot()
    {
        if (!string.IsNullOrEmpty(ShootClipName))
        {
            Play(ShootClipName, true);
        }
       
        TweenShoot.Stop();
        TweenShoot.Play();

        foreach (var fx in ShootFxList)
        {
            SpawnFx(fx, ShootPoint);
        }
       
        var bullet = GamePool.Spawn(BulletPrefab, CurrentLevel.Trans);
        bullet.Shooter = this;
        bullet.Position = ShootPoint.position;
        if (BulletFormationIndex < 0)
        {
            bullet.Forward = bullet.Direction = GetShootDirection();
        }
        
        bullet.StartCoroutine(bullet.ShootCo(BulletSpeed, BulletLifeTime));

        OnShoot.Invoke();
        OnShootBulletAction?.Invoke(bullet);

        return bullet;
    }

    #endregion
}
