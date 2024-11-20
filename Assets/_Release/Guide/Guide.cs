using System;
using System.Collections;
using Aya.Data.Persistent;
using Aya.Events;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Events;

public class Guide : EntityBase
{
    [BoxGroup("Guide")] [ValueDropdown(nameof(GetDefineKeys))]
    public string Key;

    public static IEnumerable GetDefineKeys() => GuideKeyDefine.GetDefineKeys();

    [BoxGroup("Guide")] public List<GameObject> GuideList;

    [BoxGroup("Condition")] [SerializeReference]
    public List<GuideCondition> ShowConditionList = new List<GuideCondition>();

    [BoxGroup("Condition")] [SerializeReference]
    public List<GuideCondition> CompleteConditionList = new List<GuideCondition>();

    [BoxGroup("Condition")] public List<GameEvent> CompleteEventList;

    [FoldoutGroup("Event")] public UnityEvent ShowCallback;
    [FoldoutGroup("Event")] public UnityEvent CompleteCallback;

    [FoldoutGroup("SDK")] public string EventKey;
    [FoldoutGroup("SDK")] public string EventParam;
    [FoldoutGroup("SDK")] public string EventValue;

    public string SaveKey
    {
        get
        {
            if (string.IsNullOrEmpty(_saveKey))
            {
                _saveKey = GetGuideKey(Key);
            }

            return _saveKey;
        }
    }

    private string _saveKey;

    public bool SaveGuideState
    {
        get
        {
            if (!GuideStateDic.TryGetValue(SaveKey, out var state))
            {
                state = USave.GetValue(SaveKey, false);
                GuideStateDic.Add(SaveKey, state);
            }

            return state;
        }
        set
        {
            if (!GuideStateDic.TryGetValue(SaveKey, out var state))
            {
                state = !value;
                GuideStateDic.Add(SaveKey, state);
            }

            if (state != value)
            {
                USave.SetValue(SaveKey, value);
                GuideStateDic[SaveKey] = value;
            }
        }
    }

    public bool IsComplete => SaveGuideState;

    public MethodInfo RefreshMethod => GetType().GetMethod(nameof(Refresh));
    public MethodInfo CompleteMethod => GetType().GetMethod(nameof(Complete));

    #region Static

    [NonSerialized] public static Dictionary<string, Guide> GuideDic = new Dictionary<string, Guide>();
    [NonSerialized] public static Dictionary<string, bool> GuideStateDic = new Dictionary<string, bool>();
    [NonSerialized] public static Dictionary<int, string> GuideKeyDefineDic = new Dictionary<int, string>();

    static Guide()
    {
        var fieldInfos = typeof(GuideKeyDefine).GetFields();
        foreach (var fieldInfo in fieldInfos)
        {
            var index = (int) fieldInfo.GetValue(null);
            var key = GetGuideKey(fieldInfo.Name);
            GuideKeyDefineDic.TryAdd(index, key);
        }
    }

    public static bool IsGuideShow(int keyDefine)
    {
        var key = GetGuideKey(keyDefine);
        return IsGuideShow(key);
    }

    public static bool IsGuideShow(string key)
    {
        key = GetGuideKey(key);
        if (GuideDic.TryGetValue(key, out var guide))
        {
            return guide.CheckShow();
        }

        return false;
    }

    public static bool IsGuideComplete(int keyDefine)
    {
        var key = GetGuideKey(keyDefine);
        return IsGuideComplete(key);
    }

    public static bool IsGuideComplete(string key)
    {
        key = GetGuideKey(key);
        if (!GuideStateDic.TryGetValue(key, out var state))
        {
            state = USave.GetValue(key, false);
            GuideStateDic.Add(key, state);
        }

        return state;
    }

    // public static void CompleteGuide(int keyDefine)
    // {
    //     var key = GetGuideKey(keyDefine);
    //     CompleteGuide(key);
    // }
    //
    // public static void CompleteGuide(string key)
    // {
    //     key = GetGuideKey(key);
    //     if (!GuideStateDic.TryGetValue(key, out var state))
    //     {
    //         state = SaveData.GetValue(key, false);
    //         GuideStateDic.Add(key, state);
    //     }
    //
    //     if (!state)
    //     {
    //         SaveData.SetValue(key, true);
    //         GuideStateDic[key] = true;
    //     }
    // }

    #endregion

    #region Guide Key

    public static string GetGuideKey(string key)
    {
        if (!key.StartsWith(nameof(Guide)))
        {
            key = nameof(Guide) + "_" + key;
        }

        return key;
    }

    public static string GetGuideKey(int keyDefine)
    {
        if (GuideKeyDefineDic.TryGetValue(keyDefine, out var key))
        {
            return key;
        }

        return default;
    }

    #endregion

    protected override void OnEnable()
    {
        base.OnEnable();
        GuideDic.TryAdd(Key, this);
        Refresh();

        if (SaveGuideState) return;
        if (CompleteEventList == null || CompleteEventList.Count == 0) return;
        foreach (var gameEvent in CompleteEventList)
        {
            UEvent.Listen(gameEvent, this, CompleteMethod);
            UEvent.Listen(gameEvent, this, RefreshMethod);
        }
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        if (GuideDic.ContainsKey(Key)) GuideDic.Remove(Key);

        if (CompleteEventList == null || CompleteEventList.Count == 0) return;
        foreach (var gameEvent in CompleteEventList)
        {
            UEvent.Remove(gameEvent, this, CompleteMethod);
            UEvent.Remove(gameEvent, this, RefreshMethod);
        }
    }

    public virtual void Update()
    {
        if (SaveGuideState) return;
        var complete = CheckComplete();
        if (complete) Complete();

        Refresh();
    }

    [Listen(GameEvent.CompleteGuide)]
    public virtual void Refresh()
    {
        var show = CheckShow();
        if (show) ShowCallback.Invoke();
        GuideList.ForEach(g => g.SetActive(show));
    }

    #region Check
  
    public virtual bool CheckShow()
    {
        if (SaveGuideState) return false;
        if (ShowConditionList == null || ShowConditionList.Count == 0) return true;
        foreach (var condition in ShowConditionList)
        {
            if (!condition.Check()) return false;
        }

        return true;
    }

    public virtual bool CheckComplete()
    {
        if (SaveGuideState) return true;
        if (CompleteConditionList == null || CompleteConditionList.Count == 0) return false;
        foreach (var condition in CompleteConditionList)
        {
            if (!condition.Check()) return false;
        }

        return true;
    } 

    #endregion

    public virtual void Complete()
    {
        if (SaveGuideState) return;
        SaveGuideState = true;
        GuideStateDic[SaveKey] = true;
        CompleteCallback.Invoke();
        Refresh();
        Dispatch(GameEvent.CompleteGuide, Key);

        if (string.IsNullOrEmpty(EventParam))
        {
            SDKUtil.Event(EventKey);
        }
        else
        {
            SDKUtil.Event(EventKey, EventParam, EventValue);
        }
    }
}