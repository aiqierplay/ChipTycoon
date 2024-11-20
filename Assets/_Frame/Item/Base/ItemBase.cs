using System;
using System.Collections.Generic;
using UnityEngine;
using Aya.Physical;
using Aya.Extension;
using Aya.Util;
using Sirenix.OdinInspector;

public enum ItemDeSpawnMode
{
    None = 0,
    Effect = 1,
    Exit = 2,
}

public enum ItemEffectMode
{
    Once = 0,
    UnLimit = 1,
    Count = 2,
    Stay = 3,
}

[Searchable]
[HideMonoScript]
[LabelWidth(100)]
public abstract class ItemBase : EntityBase
{
    #region Const
    
    public const bool ShowIndexLabels = false;
    public const string ItemTabGroupMain = "Main";
    public const string ItemTabGroupAdvance = "Advance";
    public const string ItemTabGroupMisc = "Misc";

    public const string ItemTabGeneral = " General";
    public const string ItemTabCondition = " Condition";

    public const string ItemTabRender = " Render";
    public const string ItemTabActive = " Active";
    public const string ItemTabEffect = " Effect";
    public const string ItemTabAnimation = " Animation";
    public const string ItemTabAudio = " Audio";
    public const string ItemTabVibrate = " Vibrate";

    public const string ItemTabRefresh = " Refresh";
    public const string ItemTabPhysics = " Physics";
    public const string ItemTabFace = " Face";
    public const string ItemTabCamera = " Camera";
    public const string ItemTabDock = " Dock";
    public const string ItemTabGuide = " Guide";
    public const string ItemTabEvent = " Event";

    #endregion

    #region Property

    // General
    [PropertyTooltip("碰撞层级" +
                     "\n\n指定碰撞触发的层级，可以被多种物体触发的道具，需要指定多个层级。")]
    [TabGroup(ItemTabGroupMain, ItemTabGeneral, SdfIconType.Gear, TextColor = "white", TabLayouting = TabLayouting.Shrink)]
    public LayerMask LayerMask;

    [PropertyTooltip("消失模式" +
                     "\n\n指定在什么情况下会消失回收。" +
                     "\n\n◆None:不消失" +
                     "\n\n◆Effect被触发后消失，一般用于瞬间触发且只能触发一次的道具" +
                     "\n\n◆Exit:当离开道具范围后消失，一般用于持续触发的道具")]
    [TabGroup(ItemTabGroupMain, ItemTabGeneral)]
    [EnumToggleButtons]
    public ItemDeSpawnMode DeSpawnMode = ItemDeSpawnMode.Effect;

    [PropertyTooltip("触发模式" +
                     "\n\n指定触发的限制规则。" +
                     "\n\n◆Once:只能被触发一次，适用于大多数情况" +
                     "\n\n◆UnLimit:不限制触发次数，适用于常驻道具" +
                     "\n\n◆Count:可以被触发指定的次数，达到次数后失效" +
                     "\n\n◆Stay:当触发器位于道具范围内，间隔一定时间触发一次，没有次数限制，适用于持续性效果的道具")]
    [TabGroup(ItemTabGroupMain, ItemTabGeneral)]
    [EnumToggleButtons]
    public ItemEffectMode EffectMode = ItemEffectMode.Once;

    [PropertyTooltip("触发次数" +
                     "\n\n指定道具可以被触发的最大次数，达到次数后即使不消失，道具功能也会失效")]
    [TabGroup(ItemTabGroupMain, ItemTabGeneral)]
    [ShowIf(nameof(EffectMode), ItemEffectMode.Count)]
    public int EffectCount = 1;

    [PropertyTooltip("触发间隔时间" +
                     "\n\n指定持续触发模式下道具效果每次触发的间隔时间，单位s")]
    [TabGroup(ItemTabGroupMain, ItemTabGeneral)]
    [ShowIf(nameof(EffectMode), ItemEffectMode.Stay)]
    [SuffixLabel("sec", Overlay = true)]
    public float EffectInterval = 1f;

    // Renderer
    [PropertyTooltip("生成显示预制" +
                     "\n\n在道具初始化时，将列表中的预制全部生成显示在 Renderer 下。")]
    [TabGroup(ItemTabGroupAdvance, ItemTabRender, SdfIconType.Display, TextColor = "#7FD6FD", TabLayouting = TabLayouting.Shrink)]
    public List<GameObject> RenderPrefabs;

    [PropertyTooltip("随机生成显示预制" +
                     "\n\n在道具初始化时，随机选取列表中的若干个预制全部生成显示在 Renderer 下。")]
    [TabGroup(ItemTabGroupAdvance, ItemTabRender)]
    public List<GameObject> RenderRandomPrefabs;

    [PropertyTooltip("随机生成预制数量" +
                     "\n\n指定 RenderRandomPrefabs 中随机显示的预制数量。")]
    [TabGroup(ItemTabGroupAdvance, ItemTabRender)]
    [LabelText("Rand Count")] 
    public int RandRenderCount = 1;

    [PropertyTooltip("关闭显示" +
                     "\n\n被触发后，关闭 Renderer 层级的显示，隐藏道具。需要注意此时道具脚本并未关闭，如果是持续性功能的道具，会继续运行。")]
    [TabGroup(ItemTabGroupAdvance, ItemTabRender)] 
    public bool DeActiveRender;

    // [TabGroup("Renderer")] public int ItemGroupIndex = -1;

    // Active
    [PropertyTooltip("开关/关闭物体" +
                     "\n\n将列表中的物体在指定的时机打开或者关闭。")]
    [TabGroup(ItemTabGroupAdvance, ItemTabActive, SdfIconType.Check2Square, TextColor = "white")]
    [TableList(ShowIndexLabels = ShowIndexLabels)]
    public List<GameEffectActive> ActiveObjectList;

    [PropertyTooltip("激活/关闭物体" +
                     "\n\n将列表中的道具在指定的时机打开或者关闭。需要注意的是，开关的是道具的功能效果，而非显示状态，可以用于实现一组道具的功能互斥。")]
    [TabGroup(ItemTabGroupAdvance, ItemTabActive)]
    [TableList(ShowIndexLabels = ShowIndexLabels)] 
    public List<GameEffectItem> ActiveItemList;

    // Effect
    [PropertyTooltip("显示特效" +
                     "\n\n在指定的时机和位置，播放列表中的特效。" +
                     "显示模式" +
                     "\n\n◆Self:在被触发对象自身节点下播放" +
                     "\n\n◆Other:在触发功能的外部对象节点下播放")]
    [TabGroup(ItemTabGroupAdvance, ItemTabEffect, SdfIconType.Snow, TextColor = "#7DECFF")]
    [TableList(ShowIndexLabels = ShowIndexLabels)] 
    public List<GameEffectFx> FxList;

    [PropertyTooltip("播放 GameEffect" +
                     "\n\n在指定的时机触发 GameEffect 配置实现的复合效果。")]
    [TabGroup(ItemTabGroupAdvance, ItemTabEffect)]
    [TableList(ShowIndexLabels = ShowIndexLabels)] 
    public List<GameEffectSubEffect> GameEffectList;

    // Animation
    [PropertyTooltip("播放 UTween 动画" +
                     "\n\n在指定的时机播放列表中由 UTweenPlayer 配置实现的动画。")]
    [TabGroup(ItemTabGroupAdvance, ItemTabAnimation, SdfIconType.PlayCircle, TextColor = "#7FFDE4")]
    [TableList(ShowIndexLabels = ShowIndexLabels)] 
    public List<GameEffectTweenPlayer> TweenPlayerList;

    [PropertyTooltip("播放 Unity Animation 动画" +
                     "\n\n在指定的时机播放列表中由 Unity Animator 组件控制的 Animation 动画。")]
    [TabGroup(ItemTabGroupAdvance, ItemTabAnimation)]
    [TableList(ShowIndexLabels = ShowIndexLabels)]
    public List<GameEffectAnimator> AnimatorList;

    // Audio
    [PropertyTooltip("播放音频" +
                     "\n\n在指定的时机播放列表中的音频。")]
    [TabGroup(ItemTabGroupAdvance, ItemTabAudio, SdfIconType.VolumeUp, TextColor = "#FFE27C")]
    [TableList(ShowIndexLabels = ShowIndexLabels)] 
    public List<GameEffectAudio> AudioList;

    // Vibration
    [PropertyTooltip("震动" +
                     "\n\n在指定的时机触发指定模式的设备震动。")]
    [TabGroup(ItemTabGroupAdvance, ItemTabVibrate, SdfIconType.PhoneVibrate)]
    [TableList(ShowIndexLabels = ShowIndexLabels)] 
    public List<GameEffectVibration> VibrationList;

    // Refresh
    [PropertyTooltip("自动刷新" +
                     "\n\n当道具功能结束后，指定时间后重新初始化启用该道具。")]
    [TabGroup(ItemTabGroupMisc, ItemTabRefresh, SdfIconType.ArrowRepeat, TextColor = "white", TabLayouting = TabLayouting.Shrink)] 
    public bool AutoRefresh = false;

    [PropertyTooltip("自动刷新时间" +
                     "\n\n道具自动刷新间隔时间范围，单位s。")]
    [TabGroup(ItemTabGroupMisc, ItemTabRefresh)]
    [ShowIf(nameof(AutoRefresh))]
    [SuffixLabel("sec", Overlay = true)]
    public Vector2 RefreshTime = new Vector2(1f, 2f);

    // Physics
    [PropertyTooltip("清除触发器" +
                     "\n\n道具在初始化时，是否清除碰撞监听器上的所有事件以确保不会重复触发，当同一个物体存在多个道具时，排在前面的道具需要取消此项，以确保排在后面的道具可以依次正确绑定事件。")]
    [TabGroup(ItemTabGroupMisc, ItemTabPhysics, SdfIconType.Box, TextColor = "#B1FD59")] 
    public bool ClearTrigger = true;

    // Face
    [PropertyTooltip("朝向目标" +
                     "\n\n被触发后，道具会旋转朝向触发目标方向。")]
    [TabGroup(ItemTabGroupMisc, ItemTabFace, SdfIconType.Forward)]
    public bool FaceToTarget;

    [PropertyTooltip("朝向目标时长" +
                     "\n\n道具旋转朝向目标过度的时长，单位s。")]
    [TabGroup(ItemTabGroupMisc, ItemTabFace)]
    [ShowIf(nameof(FaceToTarget))]
    [SuffixLabel("sec", Overlay = true)]
    public float FaceToDuration = 0.5f;

    // Camera
    [PropertyTooltip("切换相机" +
                     "\n\n道具被触发后，切换到指定相机显示，相机跟随目标为触发目标。")]
    [TabGroup(ItemTabGroupMisc, ItemTabCamera, SdfIconType.Camera, TextColor = "#7FD6FD")] 
    public bool SwitchCamera;

    [PropertyTooltip("进入切换相机" +
                     "\n\n进入道具范围后，切换到指定相机。")]
    [TabGroup(ItemTabGroupMisc, ItemTabCamera)]
    [ShowIf(nameof(SwitchCamera))]
    public CameraReference EnterCamera;

    [PropertyTooltip("离开切换相机" +
                     "\n\n离开道具范围后，切换到指定相机。")]
    [TabGroup(ItemTabGroupMisc, ItemTabCamera)]
    [ShowIf(nameof(SwitchCamera))]
    public CameraReference ExitCamera;

    // Dock
    [PropertyTooltip("停靠" +
                     "\n\n被触发后，将触发目标停靠到指定位置。")]
    [TabGroup(ItemTabGroupMisc, ItemTabDock, SdfIconType.ArrowDownRightSquare)] 
    public bool EnableDock;

    [PropertyTooltip("停靠位置" +
                     "\n\n被触发后，将触发目标停靠到指定位置。")]
    [TabGroup(ItemTabGroupMisc, ItemTabDock)]
    [ShowIf(nameof(EnableDock))] 
    public Transform DockTrans;

    [PropertyTooltip("停靠时间" +
                     "\n\n被触发后，将触发目标停靠到指定位置所用的过度时长，单位s。")]
    [TabGroup(ItemTabGroupMisc, ItemTabDock)]
    [ShowIf(nameof(EnableDock))]
    [SuffixLabel("sec", Overlay = true)]
    public float DockDuration = 0.5f;

    // Guide
    [PropertyTooltip("引导列表" +
                     "\n\n被触发后，列表中处于激活状态的引导，会改为完成状态。")]
    [TabGroup(ItemTabGroupMisc, ItemTabGuide, SdfIconType.HandIndex, TextColor = "white")] 
    public List<Guide> GuideList;

    [PropertyTooltip("回调列表" +
                     "\n\n在指定时机执行列表中的回调方法。")]
    [TabGroup(ItemTabGroupMisc, ItemTabEvent, SdfIconType.Lightning, TextColor = "yellow")]
    [TableList(ShowIndexLabels = ShowIndexLabels)]
    public List<GameEffectCallback> CallbackList; 
    
    #endregion

    [NonSerialized] public List<Collider> ColliderList;
    [NonSerialized] public List<ColliderListener> ColliderListeners;
    public virtual Type TargetType { get; set; }

    [NonSerialized] public bool Active;
    [NonSerialized] public List<GameObject> RenderInstanceList;
    [NonSerialized] public List<ItemBase> SubItems;

    [NonSerialized] public int EffectCounter;
    [NonSerialized] public float EffectTimer;
    [NonSerialized] public float EffectIntervalTimer;

    protected override void Awake()
    {
        base.Awake();

        AutoCacheSubClassList(TriggerList);
    }

    public virtual void Init()
    {
        InitRenderer();
        CacheComponents();

        gameObject.SetActive(true);
        RendererTrans?.gameObject.SetActive(true);

        ProcessTrigger(InitTriggerList);

        EffectCounter = 0;
        EffectTimer = 0f;
        EffectIntervalTimer = 0f;
        Active = true;
    }

    public virtual void InitRenderer()
    {
        if (RenderInstanceList != null && RenderInstanceList.Count > 0)
        {
            foreach (var ins in RenderInstanceList)
            {
                // 嵌套道具循环回收导致生成出错，需要过滤
                if (!ins.activeSelf) continue;
                if (ins.activeSelf && ins.transform.parent != RendererTrans) continue;
                GamePool.DeSpawn(ins);
            }
        }

        RenderInstanceList = new List<GameObject>();
        if (RenderPrefabs != null && RenderPrefabs.Count > 0)
        {
            foreach (var prefab in RenderPrefabs)
            {
                if (prefab == null) continue;
                var ins = GamePool.Spawn(prefab, RendererTrans);
                RenderInstanceList.Add(ins);
            }
        }

        if (RenderRandomPrefabs != null && RenderRandomPrefabs.Count >= RandRenderCount)
        {
            var prefabList = RenderRandomPrefabs.Random(RandRenderCount);
            foreach (var prefab in prefabList)
            {
                var ins = GamePool.Spawn(prefab, RendererTrans);
                RenderInstanceList.Add(ins);
            }
        }

        // if (ItemGroupIndex >= 0)
        // {
        //     var itemGroupData = ItemGroupSetting.Ins.CurrentSelectData;
        //     var prefab = itemGroupData[ItemGroupIndex];
        //     if (prefab != null)
        //     {
        //         var ins = GamePool.Spawn(prefab, RendererTrans);
        //         RenderInstanceList.Add(ins);
        //     }
        // }
    }

    public virtual void CacheComponents()
    {
        ColliderList = GetComponentsInChildren<Collider>().FindAll(c => c.isTrigger);
        ColliderListeners = new List<ColliderListener>();
    }

    public virtual void Complete()
    {
        Active = false;
        if (AutoRefresh)
        {
            var refreshTime = RandUtil.RandFloat(RefreshTime);
            App.ExecuteDelay(Init, refreshTime);
        }
    }

    public virtual void DeSpawn()
    {
        Active = false;
        CurrentLevel.RemoveItem(this);

        foreach (var ins in RenderInstanceList)
        {
            GamePool.DeSpawn(ins);
        }

        gameObject.SetActive(false);
    }

    public virtual void Refresh()
    {

    }

    public virtual void Reset()
    {
        LayerMask = LayerUtil.NameToMask(nameof(Player));
    }

    #region Item Trigger

    [NonSerialized] public List<GameEffectBase> TriggerList = new List<GameEffectBase>();

    public virtual void ProcessTrigger(Action<List<GameEffectBase>> action)
    {
        action(TriggerList);
    }

    public virtual void InitTriggerList(List<GameEffectBase> triggerList)
    {
        if (triggerList == null) return;
        foreach (var trigger in triggerList)
        {
            if (trigger == null) continue;
            trigger.Init(this);
        }
    }

    public virtual void ExecuteTriggerList<TTarget>(GameEffectTriggerMode mode, TTarget target)
        where TTarget : EntityBase
    {
        ProcessTrigger(triggerList => ExecuteTriggerList(triggerList, mode, target));
    }

    public virtual void ExecuteTriggerList<TTarget>(List<GameEffectBase> triggerList, GameEffectTriggerMode mode, TTarget target)
        where TTarget : EntityBase
    {
        if (triggerList == null) return;
        foreach (var trigger in triggerList)
        {
            if (trigger == null) continue;
            if (trigger.Mode != mode) continue;
            trigger.Other = target;
            trigger.Play(this, target);
        }
    }

    #endregion
}