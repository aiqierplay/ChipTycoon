using System;
using System.Collections;
using System.Collections.Generic;
using Aya.Async;
using Aya.Extension;
using Aya.TweenPro;
using Sirenix.OdinInspector;
using UnityEngine;

public abstract class Mergeable<T, TMergeItem, TMergeArea, TMergeData> : EntityBase<T>
    where T : Mergeable<T, TMergeItem, TMergeArea, TMergeData>
    where TMergeItem : MergeableItem<TMergeItem, TMergeData>
    where TMergeArea : MergeableArea<TMergeItem, TMergeData>
    where TMergeData : MergeableData<TMergeItem>
{
    [Title("Auto Merge")]
    public float MergeFlyHeight;
    public float MergeCamZ;
    public float MergeDistance;
    public float MergeFlyDuration;
    public float MergeDuration;
    public AnimationCurve MergeCurve;
    public float MergeShowWait;
    public bool AutoRePlace;


    [NonSerialized] public bool IsMerging;
    [NonSerialized] public TMergeItem OperatingItem1;
    [NonSerialized] public TMergeItem OperatingItem2;

    public abstract List<TMergeArea> MergeSlotList { get; }

    public bool CanAutoMerge
    {
        get
        {
            if (IsMerging) return false;

            if (!CheckUpgradeCost())
            {
                return false;
            }

            var slots = MergeSlotList;
            for (var i = 0; i < slots.Count - 1; i++)
            {
                for (var j = i + 1; j < slots.Count; j++)
                {
                    var slot1 = slots[i];
                    var slot2 = slots[j];

                    if (slot1.CurrentItem == null || slot2.CurrentItem == null) continue;
                    var item1 = slot1.CurrentItem as TMergeItem;
                    var item2 = slot2.CurrentItem as TMergeItem;
                    if (item1 == null || item2 == null) continue;
                    if (item1.Data.IsMaxLevel) continue;
                    if (item1.Data.Index == item2.Data.Index) return true;
                }
            }

            return false;
        }
    }

    public virtual void AutoUpgradeMerge()
    {
        // var cost = Upgrade.GetInfo<AutoMergeData>().Current.CostCoin;
        // Save.Coin.Value -= cost;
        // Upgrade.GetInfo<AutoMergeData>(CurrentLevel.SaveKey).ForceUpgrade();
    }

    public virtual bool CheckUpgradeCost()
    {
        // var cost = Upgrade.GetInfo<AutoMergeData>().Current.CostCoin;
        // var enough = Save.Coin >= cost;
        // if (!enough) return false;
        return true;
    }

    public void AutoMerge()
    {
        if (!CanAutoMerge) return;

        var (item1, item2) = GetMergeableItemPair();
        AutoUpgradeMerge();
        StartCoroutine(AutoMergeCo(item2, item1, () =>
        {
            if (AutoRePlace)
            {
                StartCoroutine(AutoRePlaceCo());
            }
        }));
    }

    public (TMergeItem, TMergeItem) GetMergeableItemPair()
    {
        var list = new List<(TMergeItem, TMergeItem)>();
        var slots = MergeSlotList;
        for (var i = 0; i < slots.Count - 1; i++)
        {
            for (var j = i + 1; j < slots.Count; j++)
            {
                var slot1 = slots[i];
                var slot2 = slots[j];

                if (slot1.CurrentItem == null || slot2.CurrentItem == null) continue;
                var item1 = slot1.CurrentItem as TMergeItem;
                var item2 = slot2.CurrentItem as TMergeItem;
                if (item1 == null || item2 == null) continue;
                if (item1.Data.IsMaxLevel) continue;
                if (item1.Data.Index != item2.Data.Index) continue;
                list.Add((item1, item2));
            }
        }

        list.SortAsc(i => i.Item1.Data.Index);
        return list.First();
    }

    public IEnumerator AutoMergeCo(TMergeItem itemFrom, TMergeItem itemTo, Action onDone = null)
    {
        IsMerging = true;
        OperatingItem1 = itemFrom;
        OperatingItem2 = itemTo;

        var startSlot = itemFrom.CurrentArea as TMergeArea;
        var endSlot = itemTo.CurrentArea as TMergeArea;

        var flyStartPos1 = itemFrom.Position;
        var flyStartRot1 = itemFrom.EulerAngles;
        var flyStartPos2 = itemTo.Position;
        var flyStartRot2 = itemTo.EulerAngles;
        itemFrom.Parent = CurrentLevel.Trans;
        itemTo.Parent = CurrentLevel.Trans;
        var mergePos = UiToWorldPosition(new Vector3(Screen.width / 2f, Screen.height / 2f, 0f), MergeCamZ);
        var mergePos1 = mergePos + Vector3.left * MergeDistance;
        var mergePos2 = mergePos + Vector3.right * MergeDistance;
        var tweenFly = UTween.Value(0f, 1f, MergeFlyDuration, value =>
        {
            var pos1 = TweenParabola.GetPositionByFactor(flyStartPos1, mergePos1, MergeFlyHeight, value);
            itemFrom.Position = pos1;
            var rot1 = Vector3.Lerp(flyStartRot1, Vector3.zero, value);
            itemFrom.EulerAngles = rot1;
            var scale1 = Vector3.Lerp(itemFrom.LocalScale, Vector3.one, value);
            itemFrom.LocalScale = scale1;

            var pos2 = TweenParabola.GetPositionByFactor(flyStartPos2, mergePos2, MergeFlyHeight, value);
            itemTo.Position = pos2;
            var rot2 = Vector3.Lerp(flyStartRot2, Vector3.zero, value);
            itemTo.EulerAngles = rot2;
            var scale2 = Vector3.Lerp(itemTo.LocalScale, Vector3.one, value);
            itemTo.LocalScale = scale2;
        });

        yield return tweenFly.WaitForComplete();

        var mergeStartPos1 = itemFrom.Position;
        var mergeStartPos2 = itemTo.Position;
        var tweenMerge = UTween.Value(0f, 1f, MergeDuration, value =>
        {
            var pos1 = Vector3.LerpUnclamped(mergeStartPos1, mergePos, value);
            itemFrom.Position = pos1;
            var pos2 = Vector3.LerpUnclamped(mergeStartPos2, mergePos, value);
            itemTo.Position = pos2;
        }).SetCurve(MergeCurve);

        yield return tweenMerge.WaitForComplete();

        if (startSlot != null) startSlot.Clear();
        var nextIndex = itemFrom.Data.Index + 1;
        var mergeItem = SpawnMergeableItem(nextIndex);
        SpawnFx(mergeItem.MergeFx, CurrentLevel.Trans, mergePos);

        if (endSlot != null)
        {
            endSlot.Clear();
            mergeItem.CurrentArea = endSlot;
            mergeItem.CurrentGroup = endSlot.DroppableGroup;
            mergeItem.FromArea = endSlot;
            mergeItem.Drop(endSlot);
        }

        var putEndPos = mergeItem.Position;
        mergeItem.Position = mergePos;

        yield return YieldBuilder.WaitForSeconds(MergeShowWait);

        var tweenPut = UTween.Value(0f, 1f, MergeFlyDuration, value =>
        {
            var pos = TweenParabola.GetPositionByFactor(mergePos, putEndPos, MergeFlyHeight, value);
            mergeItem.Position = pos;
        });

        yield return tweenPut.WaitForComplete();
        mergeItem.Trans.ResetLocal();

        OperatingItem1 = null;
        OperatingItem2 = null;

        // SDKUtil.Event("mergedata.level", "level", CurrentLevel.Index, "harvester_level", mergeItem.Data.Index);

        yield return null;
        if (onDone != null) onDone.Invoke();
        IsMerging = false;
    }

    public IEnumerator AutoRePlaceCo()
    {
        IsMerging = true;
        var mergeSlotList = new List<TMergeArea>();
        mergeSlotList.AddRange(MergeSlotList);
        // mergeSlotList.SortDesc(slot =>
        // {
        //     if (slot.IsEmpty) return 0;
        //     var currentItem = slot.CurrentItem as TMergeItem;
        //     if (currentItem == null) return 0;
        //     return currentItem.Data.Index;
        // });

        for (var i = mergeSlotList.Count - 1; i >= 0; i--)
        {
            var mergeSlot = mergeSlotList[i];
            if (mergeSlot.IsEmpty) continue;
            for (var j = 0; j < mergeSlotList.Count && j < i; j++)
            {
                var emptySlot = mergeSlotList[j];
                if (!emptySlot.IsEmpty) continue;

                var item = mergeSlot.CurrentItem as TMergeItem;
                if (item == null) continue;

                OperatingItem1 = item;
                var putStartPos = item.Position;
                var tweenPut = UTween.Value(0f, 1f, MergeFlyDuration, value =>
                {
                    var pos = TweenParabola.GetPositionByFactor(putStartPos, emptySlot.PlaceTrans.position, MergeFlyHeight, value);
                    item.Position = pos;
                });

                yield return tweenPut.WaitForComplete();

                item.Pickup();
                item.Drop(emptySlot);
                OperatingItem1 = null;

                break;
            }
        }

        yield return null;
        IsMerging = false;
    }

    protected TMergeItem SpawnMergeableItem(int index = 0)
    {
        var data = Config.GetData<TMergeData>(index);
        var mergeItem = GamePool.Spawn(data.Prefab, CurrentLevel.Trans);
        mergeItem.Init(data);
        return mergeItem;
    }
}
