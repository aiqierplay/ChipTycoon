using System;
using System.Collections;
using Aya.Async;
using Aya.Events;
using Aya.Extension;
using Aya.Physical;
using Aya.TweenPro;
using Aya.Util;
using Sirenix.OdinInspector;
using UnityEngine;
public enum WorkerType
{
    Player = 0,
    Computer = 1,
}

public class Worker : EntityBase
{
    [Title("Animation")] 
    public string IdleClip;
    public string WalkClip;
    public GameObject MaxTip;

    public float RotateSpeed = 50f;
    public float TransferInterval = 0.1f;

    [GetComponentInChildren, NonSerialized]
    public StackList StackList;

    public bool ClampPosWithLayer;
    [ShowIf(nameof(ClampPosWithLayer))] public LayerMask WalkLayer;

    public bool IsFull => StackList.Count >= Capacity;
    public bool IsEmpty => StackList.IsEmpty;

    [NonSerialized] public WorkerType Type;
    [NonSerialized] public float MoveSpeed = 10;
    [NonSerialized] public int Capacity = 10;
    [NonSerialized] public BuildingFactory WorkFactory;

    public void Init(WorkerType type)
    {
        EnableMove();
        Type = type;
        StackList.Init();
        Play(IdleClip);
        Refresh();
        RefreshData();
        if (Type == WorkerType.Computer)
        {
            StartCoroutine(ComputerWorkCo());
        }
    }

    [Listen(GameEvent.Upgrade)]
    public void RefreshData()
    {
        if (Type == WorkerType.Computer)
        {
            MoveSpeed = Upgrade.GetInfo<WorkerMoveSpeedData>(CurrentLevel.SaveKey + "/Worker").Current.Value;
            Capacity = Upgrade.GetInfo<WorkerCapacityData>(CurrentLevel.SaveKey + "/Worker").Current.IntValue;
        }
        else
        {
            MoveSpeed = Upgrade.GetInfo<WorkerMoveSpeedData>(CurrentLevel.SaveKey + "/Player").Current.Value;
            Capacity = Upgrade.GetInfo<WorkerCapacityData>(CurrentLevel.SaveKey + "/Player").Current.IntValue;
        }
    }

    #region Trigger

    [NonSerialized] public BuildingBase CurrentBuilding;
    [NonSerialized] public FactoryBase CurrentFactory;
    [NonSerialized] public FactoryPoint CurrentFactoryPoint;
    [NonSerialized] public bool IsWorking= false;
    [NonSerialized] public Coroutine WorkCoroutine;

    public void OnEnter(BuildingBase building)
    {
        CurrentBuilding = building;
        if (CurrentBuilding is FactoryBase factory)
        {
            WorkCoroutine = StartCoroutine(FactoryWorkCo(factory));
        }
    }

    public void OnEnter(FactoryBase factory, FactoryPoint point)
    {
        CurrentFactory = factory;
        CurrentFactoryPoint = point;
        OnEnter(factory);
    }

    public void Refresh()
    {
        if (MaxTip != null) MaxTip.SetActive(IsFull);
    }


    public void OnExit(BuildingBase building)
    {
        CurrentBuilding = null;
        IsWorking = false;
        if (WorkCoroutine != null)
        {
            StopCoroutine(WorkCoroutine);
            WorkCoroutine = null;
        }
    }

    public void OnExit(FactoryBase factory, FactoryPoint point)
    {
        CurrentFactory = null;
        CurrentFactoryPoint = null;
        OnExit(factory);
    }

    #endregion

    #region Move

    [NonSerialized] public bool ActiveMove;
    [NonSerialized] public bool IsMoving;
    [NonSerialized] public Vector3 StartPos;
    [NonSerialized] public Vector3 Direction;

    public void EnableMove()
    {
        ActiveMove = true;
    }

    public void DisableMove()
    {
        ActiveMove = false;
        IsMoving = false;
        Play(IdleClip);
    }

    public void OnTouchStart(Vector3 pos)
    {
        Play(WalkClip);
        IsMoving = true;
        StartPos = pos;
    }

    public void OnTouch(Vector3 pos)
    {
        Direction = pos - StartPos;
        Direction = Direction.normalized;
        Direction.z = Direction.y;
        Direction.y = 0;
        Move(Direction);
    }

    public void OnTouchEnd(Vector3 pos)
    {
        Direction = Vector3.zero;
        IsMoving = false;
        Play(IdleClip);
    }

    public void Move(Vector3 direction)
    {

    }

    public IEnumerator MoveTo(Vector3 position)
    {
        Play(WalkClip);
        var tweenMove = UTween.Position(Trans, position, MoveSpeed)
            .SetSpeedBased();
        var forward = (position - Position).normalized;
        if (forward != Vector3.zero)
        {
            UTween.Forward(Trans, forward, 0.2f);
        }

        yield return tweenMove.WaitForComplete();

        Play(IdleClip);
        yield return null;
    } 

    #endregion

    public void LateUpdate()
    {
        if (ActiveMove && IsMoving)
        {
            if (Direction == Vector3.zero) return;
            var targetPos = Trans.position + Direction * MoveSpeed * DeltaTime;
            Trans.forward = Vector3.MoveTowards(Forward, Direction, RotateSpeed * DeltaTime);

            if (ClampPosWithLayer)
            {
                var (target, pos) = PhysicsUtil.Raycast<Transform>(targetPos + Vector3.up, Vector3.down, 10f, WalkLayer);
                if (target == null) return;
                 Trans.position = targetPos;
            }
        }
    }

    public Product LastProduct => StackList.List.Last() as Product;

    public ProductTypeData CurrentProductType
    {
        get
        {
            if (StackList.Count == 0) return null;
            var product = StackList.List[0] as Product;
            return product.TypeData;
        }
    }

    public FactoryBase FindOutputFactory()
    {
       var factoryList = World.FactoryList.FindAll(f => f.Output.Count > 0 && !f.Output.TypeData.IsFinal);
       if (factoryList.Count == 0) return null;
       var factory = factoryList.First();
       return factory;
    }

    public FactoryBase FindInputFactory()
    {
        var factoryList = World.FactoryList.FindAll(f => f.Input.Enable &&
                                                         f.Input.CanAdd && 
                                                         f.Input.Type == CurrentProductType.Key);
        if (factoryList.Count == 0) return null;
        var factory = factoryList.Random();
        return factory;
    }

    public IEnumerator ComputerWorkCo()
    {
        while (true)
        {
            while (IsEmpty)
            {
                var targetFactory = FindOutputFactory();
                if (targetFactory != null)
                {
                    var targetPos = targetFactory.Output.Pos.position;
                    yield return MoveTo(targetPos);
                    CurrentFactory = targetFactory;
                    CurrentFactoryPoint = targetFactory.Output;
                    yield return TransferOutputCo(targetFactory);
                }

                yield return YieldBuilder.WaitForSeconds(0.5f);
            }

            while (!IsEmpty)
            {
                var targetFactory = FindInputFactory();
                if (targetFactory != null)
                {
                    var targetPos = targetFactory.Input.Pos.position;
                    yield return MoveTo(targetPos);
                    CurrentFactory = targetFactory;
                    CurrentFactoryPoint = targetFactory.Input;
                    yield return TransferInputCo(targetFactory);
                }

                yield return YieldBuilder.WaitForSeconds(0.5f);
            }

            CurrentFactory = null;
            CurrentFactoryPoint = null;

            yield return YieldBuilder.WaitForSeconds(0.5f);
        }
    }

    public IEnumerator FactoryWorkCo(FactoryBase factory)
    {
        while (IsWorking)
        {
            yield return null;
        }

        IsWorking = true;
        if (StackList.IsEmpty || CurrentProductType.Key == factory.Output.Type)
        {
            yield return TransferOutputCo(factory);
        }
        else
        {
            if (factory.Input.Type == CurrentProductType.Key)
            {
                yield return TransferInputCo(factory);
            }
        }

        yield return null;
        IsWorking = false;
        CurrentFactory = null;
        CurrentFactoryPoint = null;
    }

    public IEnumerator TransferInputCo(FactoryBase factory)
    {
        while (!IsEmpty && factory.Input.CanAdd && CurrentFactoryPoint != null && CurrentFactoryPoint.Mode == FactoryPointMode.Input)
        {
            while (LastProduct != null && LastProduct.IsWorking)
            {
                yield return null;
            }

            var product = StackList.Pop() as Product;
            Refresh();
            if (product == null)
            {
                yield return null;
                continue;
            }

            product.IsWorking = true;
            factory.Input.StackList.AddParabola(product, () =>
            {
                product.IsWorking = false;
            });
            factory.Input.Refresh();
            yield return YieldBuilder.WaitForSeconds(TransferInterval);
        }

        yield return null;
    }

    public IEnumerator TransferOutputCo(FactoryBase factory)
    {
        // Log(StackList.Count, factory.name, CurrentFactory.name, factory.Output.IsEmpty, CurrentFactoryPoint.Mode);
        Func<bool> checkFunc = () =>
        {
            var result = !IsFull && !factory.Output.IsEmpty && CurrentFactoryPoint != null && CurrentFactoryPoint.Mode == FactoryPointMode.Output;
            return result;
        };

        while (checkFunc())
        {
            while (factory.Output.LastProduct.IsWorking)
            {
                yield return null;
            }

            var product = factory.Output.StackList.Pop() as Product;
            Refresh();
            if (product == null)
            {
                yield return null;
                continue;
            }

            if (product.TypeData.IsFinal)
            {
                product.IsWorking = true;
                product.ParabolaFlyTo(StackList.Position, RandUtil.RandFloat(2f, 3f), RandUtil.RandFloat(0.25f, 0.35f),
                    () =>
                    {
                        var value = product.TypeData.CostCoin;
                        UIFlyIcon.Ins.Fly(UIFlyIcon.Coin, WorldToUiPosition(), 1, () =>
                        {
                            Save.Coin.Value += value;
                        });

                        product.IsWorking = false;
                        GamePool.DeSpawn(product);
                    });
            }
            else
            {
                product.IsWorking = true;
                StackList.AddParabola(product, () =>
                {
                    product.IsWorking = false;
                    Refresh();
                });
            }

            factory.Output.Refresh();
            yield return YieldBuilder.WaitForSeconds(TransferInterval);
        }

        yield return null;
    }
}
