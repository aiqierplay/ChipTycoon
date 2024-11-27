using System;
using System.Collections;
using Aya.Extension;
using Aya.TweenPro;
using Sirenix.OdinInspector;
using UnityEngine;

public enum WorkerType
{
    Player = 0,
    Computer = 1,
}

public class Worker : EntityBase
{
    [Title("Param")] 
    public int Capacity = 10;

    [Title("Animation")] 
    public string IdleClip;
    public string WalkClip;

    public float MoveSpeed = 5f;
    public float RotateSpeed = 50f;

    [GetComponentInChildren, NonSerialized]
    public StackList StackList;

    public bool IsFull => StackList.Count >= Capacity;
    public bool IsEmpty => StackList.IsEmpty;

    [NonSerialized] public WorkerType Type;

    public void Init(WorkerType type)
    {
        EnableMove();
        Type = type;
        StackList.Init();
        Play(IdleClip);
    }

    #region Trigger

    [NonSerialized] public BuildingBase CurrentBuilding;
    [NonSerialized] public bool IsWorking= false;
    [NonSerialized] public Coroutine WorkCoroutine;

    public void OnEnter(BuildingBase building)
    {
        CurrentBuilding = building;
        if (CurrentBuilding is BuildingFactory factory)
        {
            WorkCoroutine = StartCoroutine(WorkCo(factory));
        }
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

    public IEnumerator MoveTo(Vector3 position, Action onDone)
    {
        Play(WalkClip);
        var tweenMove = UTween.Position(Trans, position, MoveSpeed)
            .SetSpeedBased();
        yield return tweenMove.WaitForComplete();
        var forward = position - Position;
        if (forward != Vector3.zero)
        {
            UTween.Forward(Trans, forward, 0.2f);
        }

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


    public IEnumerator WorkCo(BuildingFactory factory)
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

    public IEnumerator TransferInputCo(BuildingFactory factory)
    {
        while (!IsEmpty && factory.Input.CanAdd)
        {
            var product = StackList.Pop() as Product;
            factory.Input.StackList.AddParabola(product);
            factory.Input.Refresh();
            yield return null;
        }


        yield return null;
    }

    public IEnumerator TransferOutputCo(BuildingFactory factory)
    {
        while (!IsFull && !factory.Output.IsEmpty)
        {
            var product = factory.Output.StackList.Pop() as Product;
            StackList.AddParabola(product);
            factory.Output.Refresh();
            yield return null;
        }

        yield return null;
    }
}
