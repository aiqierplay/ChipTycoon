using System;
using Aya.Extension;
using Aya.TweenPro;
using Aya.Util;
using Sirenix.OdinInspector;
using UnityEngine;

[Serializable]
public class BreakableData
{
    public ItemBase Item;
    public Rigidbody Rigidbody;
    public Collider Collider;
    public Vector3 LocalPosition;
    public Vector3 LocalEulerAngles;
    public Transform Transform;
    public float Distance;

    public BreakableConfigData Config;

    public BreakableData(ItemBase item, Transform transform, BreakableConfigData config)
    {
        Item = item;
        Config = config;
        Cache(transform);
    }

    public void Cache(Transform transform)
    {
        if (Config.UseRigidbody) Rigidbody = transform.GetOrAddComponent<Rigidbody>();
        Collider = transform.GetOrAddComponent<Collider>();
        Transform = transform;
        LocalPosition = transform.localPosition;
        LocalEulerAngles = transform.localEulerAngles;
        Distance = LocalPosition.magnitude;
    }

    public void Reset()
    {
        if (Config.UseRigidbody)
        {
            Rigidbody.ClearMomentum();
            Rigidbody.isKinematic = true;
        }

        Collider.enabled = true;
        Transform.gameObject.SetActive(false);
        Transform.localPosition = LocalPosition;
        Transform.localEulerAngles = LocalEulerAngles;
    }

    public void Explode(Vector3 center)
    {
        Transform.gameObject.SetActive(true);
        if (Config.UseRigidbody)
        {
            Collider.enabled = true;
            Rigidbody.isKinematic = false;
            Rigidbody.AddExplosionForce(Config.ExplodeForce, Config.ExplodeCenter + center, Config.ExplodeRange);
            Rigidbody.angularVelocity = RandUtil.RandVector3(0, 360);
            if (Config.AutoDeActive)
            {
                Item.ExecuteDelay(() =>
                {
                    Transform.gameObject.SetActive(false);
                }, RandUtil.RandFloat(Config.DeActiveDelay.x, Config.DeActiveDelay.y));
            }
        }
        else
        {
            Collider.enabled = false;
            var start = Transform.position;
            var end = start + (Transform.position - center).normalized * Config.ExplodeRange;
            UTween.Value(0f, 1f, RandUtil.RandFloat(0.5F, 1.5f), value =>
            {
                var pos = TweenParabola.GetPositionByFactor(start, end, Config.ExplodeForce, value);
                Transform.position = pos;
            })
                .SetUpdateMode(UpdateMode.LateUpdate)
                .SetOnStop(() =>
                {
                    if (Config.AutoDeActive)
                    {
                        Transform.gameObject.SetActive(false);
                    }
                });
        }
    }
}