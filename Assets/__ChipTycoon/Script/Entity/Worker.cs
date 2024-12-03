using System;
using System.Collections;
using Aya.Async;
using Aya.Events;
using Aya.Extension;
using Aya.TweenPro;
using Sirenix.OdinInspector;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

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

    public float RotateSpeed = 50f;

    [GetComponentInChildren, NonSerialized]
    public StackList StackList;

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
        if (CurrentBuilding is BuildingFactory factory)
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
            Trans.position += Direction * MoveSpeed * DeltaTime;
            Trans.forward = Vector3.MoveTowards(Forward, Direction, RotateSpeed * DeltaTime);
        }
    }

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
       var factory = factoryList.Random();
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
                    yield return TransferInputCo(targetFactory);
                }

                yield return YieldBuilder.WaitForSeconds(0.5f);
            }

            yield return YieldBuilder.WaitForSeconds(0.5f);
        }
    }

    public IEnumerator FactoryWorkCo(BuildingFactory factory)
    {
        while (IsWorking)
        {
            yield return null;
        }

        IsWorking = true;
        if (StackList.IsEmpty)
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
    }

    public IEnumerator TransferInputCo(FactoryBase factory)
    {
        while (!IsEmpty && factory.Input.CanAdd && CurrentFactoryPoint != null && CurrentFactoryPoint.Mode == FactoryPointMode.Input)
        {
            var product = StackList.Pop() as Product;
            factory.Input.StackList.AddParabola(product);
            factory.Input.Refresh();
            yield return null;
        }


        yield return null;
    }

    public IEnumerator TransferOutputCo(FactoryBase factory)
    {
        while (!IsFull && !factory.Output.IsEmpty && CurrentFactoryPoint != null && CurrentFactoryPoint.Mode == FactoryPointMode.Output)
        {
            var product = factory.Output.StackList.Pop() as Product;
            if (product == null) yield break;
            if (product.TypeData.IsFinal)
            {
                StackList.AddParabola(product, () =>
                {
                    var value = product.TypeData.CostCoin;
                    UIFlyIcon.Ins.Fly(UIFlyIcon.Coin, WorldToUiPosition(), 1, () =>
                    {
                        Save.Coin.Value += value;
                    });

                    StackList.Remove(product);
                });
            }
            else
            {
                StackList.AddParabola(product);
                
            }

            factory.Output.Refresh();
            yield return null;
        }

        yield return null;
    }
}
