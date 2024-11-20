using System;
using Aya.Async;
using System.Collections;
using UnityEngine;

public class ShootBullet : EntityBase
{
    public GameObject ShootFx;

    [NonSerialized] public EntityBase Shooter;
    [NonSerialized] public Vector3 Direction;

    protected override void OnEnable()
    {
        base.OnEnable();
        if (ShootFx != null) ShootFx.SetActive(false);
    }

    public IEnumerator ShootCo(float speed, float lifeTime)
    {
        var timer = 0f;
        if (ShootFx != null) ShootFx.SetActive(true);
        while (timer < lifeTime)
        {
            timer += DeltaTime;
            if (!gameObject.activeSelf) yield break;
            Position += Direction * speed * DeltaTime;
            yield return YieldBuilder.WaitForFixedUpdate();
        }

        if (gameObject.activeSelf)
        {
            GamePool.DeSpawn(gameObject);
        }
    }
}
