using System;
using UnityEngine;

public abstract partial class EntityBase
{
    [FindInAllChildFuzzy(nameof(Renderer)), NonSerialized] public Transform RendererTrans;

    [GetComponentInChildren, NonSerialized] public Rigidbody Rigidbody;
    [GetComponentInChildren, NonSerialized] public Collider Collider;

    protected override void Awake()
    {
        base.Awake();
        SelfScale = 1f;

        Trans = transform;

        CacheProperty();
        CacheFiled();
        if (EnableAwakeProcessEntityProperty) ProcessEntityPropertyAttribute(EntityPropertyMode.Awake);

        CacheComponent();

        if (RendererTrans == null)
        {
            RendererTrans = transform;
        }
    }

    public virtual void CacheComponent()
    {
        CacheRendererComponent();
        CacheSubPoolInstance();
    }


    #region Setting

    public TSetting GetSetting<TSetting>() where TSetting : SettingBase<TSetting>
    {
        return SettingBase<TSetting>.Load();
    }

    #endregion

    #region MonoBehaviour

    protected override void OnEnable()
    {
        base.OnEnable();
        if (EnableEnableProcessEntityProperty) ProcessEntityPropertyAttribute(EntityPropertyMode.Enable);
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        if (EnableDisableProcessEntityProperty) ProcessEntityPropertyAttribute(EntityPropertyMode.Disable);
        DeSpawnSubPoolInstance();
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        if (EnableDestroyProcessEntityProperty) ProcessEntityPropertyAttribute(EntityPropertyMode.Destroy);
    }

    #endregion
}