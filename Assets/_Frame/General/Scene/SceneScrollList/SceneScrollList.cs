using System;
using System.Collections.Generic;
using Aya.Maths;
using Aya.Pool;
using UnityEngine;

public enum SceneScrollDirection
{
    X = 0,
    Y = 1,
    Z = 2,

}

public enum SceneScrollMouseDirection
{
    X = 0,
    Y = 1,
    NegativeX = 2,
    NegativeY = 3,
}

public enum SceneScrollListUpdateMode
{
    Update = 0,
    LateUpdate = 1,
    FixedUpdate = 2,
}

public abstract class SceneScrollList : EntityBase
{
    public SceneScrollItem ItemPrefab;
    public Transform MoveTrans;
    public Transform ItemTrans;
    public float ViewportSize;
    public SceneScrollDirection SceneDirection;
    public SceneScrollMouseDirection MouseDirection;
    public SceneScrollListUpdateMode UpdateMode = SceneScrollListUpdateMode.LateUpdate;
    public float ScrollSpeed = 5;
    public float StopSpeed = 5;
    public float ReBoundSpeed = 10;
    public bool OverScroll = true;
    public Vector3 Offset;
    public bool ReverseScrollRange = false;

    [NonSerialized] public List<SceneScrollItem> ItemList = new List<SceneScrollItem>();
    public int Count => ItemList.Count;

    public virtual EntityPool Pool => PoolManager.Ins[nameof(SceneScrollList)];

    public SceneScrollItem this[int index] => ItemList[index];

    public virtual Vector3 MinPosition { get; set; }
    public virtual Vector3 MaxPosition { get; set; }

    public virtual Vector3 CurrentPosition
    {
        get => MoveTrans.localPosition - Offset;
        set => MoveTrans.localPosition = value + Offset;
    }

    public virtual void Init()
    {
        Clear();
        OnMouseUp();
    }

    public virtual void AddItem(Action<SceneScrollItem> onSpawned = null)
    {
        AddItem(ItemPrefab, onSpawned);
    }

    public virtual void AddItem(SceneScrollItem prefab, Action<SceneScrollItem> onSpawned = null)
    {
        var item = Pool.Spawn(prefab, ItemTrans);
        item.Init(this);
        ItemList.Add(item);
        ReSort(true);
        onSpawned?.Invoke(item);
    }

    public virtual void RemoveItem(SceneScrollItem item)
    {
        if (!ItemList.Contains(item)) return;
        ItemList.Remove(item);
        ReSort(false);
    }

    public virtual void Clear()
    {
        Pool.DeSpawnAll();
        ItemList.Clear();
        CurrentPosition = Vector3.zero;
    }

    public virtual void ReSort(bool immediately = false)
    {
        var count = Count;
        var currentPos = 0f;
        for (var index = 0; index < count; index++)
        {
            var item = ItemList[index];
            item.Index = index;
            item.SetSortPos(currentPos, immediately);
            var size = item.GetSortSize();
            currentPos += size;
        }

        if (currentPos <= ViewportSize)
        {
            MinPosition = Vector3.zero;
            MaxPosition = Vector3.zero;
        }
        else if (ReverseScrollRange)
        {
            MinPosition = Vector3.zero;
            switch (SceneDirection)
            {
                case SceneScrollDirection.X:
                    MaxPosition = new Vector3(currentPos - ViewportSize, 0, 0);
                    break;
                case SceneScrollDirection.Y:
                    MaxPosition = new Vector3(0, currentPos - ViewportSize, 0);
                    break;
                case SceneScrollDirection.Z:
                    MaxPosition = new Vector3(0, 0, currentPos - ViewportSize);
                    break;
            }
        }
        else
        {
            MaxPosition = Vector3.zero;
            switch (SceneDirection)
            {
                case SceneScrollDirection.X:
                    MinPosition = new Vector3(ViewportSize - currentPos, 0, 0);
                    break;
                case SceneScrollDirection.Y:
                    MinPosition = new Vector3(0, ViewportSize - currentPos, 0);
                    break;
                case SceneScrollDirection.Z:
                    MinPosition = new Vector3(0, 0, ViewportSize - currentPos);
                    break;
            }
        }

        if (!OverScroll)
        {
            CurrentPosition = ClampPosition(CurrentPosition);
        }
    }

    public virtual Vector3 ClampPosition(Vector3 pos)
    {
        var result = MathUtil.Clamp(CurrentPosition, MinPosition, MaxPosition);
        return result;
    }

    public virtual bool IsOverViewport(Vector3 pos)
    {
        return pos.x < MinPosition.x ||
               pos.x > MaxPosition.x ||
               pos.y < MinPosition.y ||
               pos.y > MaxPosition.y ||
               pos.z < MinPosition.z ||
               pos.z > MaxPosition.z;
    }

    private Vector3 _lastPos;
    private Vector3 _currentPos;
    private bool _isMouseDown;
    private float _endSpeed;
    private Vector3 _endDirection;

    public virtual void OnMouseDown()
    {
        _isMouseDown = true;
        _lastPos = Input.mousePosition;
        _currentPos = Input.mousePosition;
    }

    public virtual void OnMouseUp()
    {
        _isMouseDown = false;
        _endDirection = _currentPos - _lastPos;
        _endSpeed = ScrollSpeed;
    }

    public virtual void MoveUp(float distance)
    {
        Move(distance);
    }

    public virtual void MoveDown(float distance)
    {
        Move(-distance);
    }

    public virtual void Move(float distance)
    {
        switch (SceneDirection)
        {
            case SceneScrollDirection.X:
                CurrentPosition += new Vector3(distance, 0, 0);
                break;
            case SceneScrollDirection.Y:
                CurrentPosition += new Vector3(0, distance, 0);
                break;
            case SceneScrollDirection.Z:
                CurrentPosition += new Vector3(0, 0, distance);
                break;
        }

        if (!OverScroll)
        {
            CurrentPosition = ClampPosition(CurrentPosition);
        }
    }

    public virtual void Update()
    {
        if (UpdateMode != SceneScrollListUpdateMode.Update) return;
        var deltaTime = DeltaTime;
        UpdateImpl(deltaTime);
    }

    public virtual void LateUpdate()
    {
        if (UpdateMode != SceneScrollListUpdateMode.LateUpdate) return;
        var deltaTime = DeltaTime;
        UpdateImpl(deltaTime);
    }

    public virtual void FixedUpdate()
    {
        if (UpdateMode != SceneScrollListUpdateMode.FixedUpdate) return;
        var deltaTime = FixedDeltaTime;
        UpdateImpl(deltaTime);
    }

    public virtual void UpdateImpl(float deltaTime)
    {
        var direction = Vector3.zero;
        var moveDistance = 0f;
        var moveSpeed = 0f;
        var needScroll = false;

        var isOverViewport = IsOverViewport(CurrentPosition);

        if (_isMouseDown)
        {
            _lastPos = _currentPos;
            _currentPos = Input.mousePosition;
            direction = _currentPos - _lastPos;
            moveSpeed = ScrollSpeed / 60f;
            needScroll = true;
        }
        else
        {
            if (_endSpeed >= 0.01f && !isOverViewport)
            {
                direction = _endDirection;
                _endSpeed = Mathf.Lerp(_endSpeed, 0f, deltaTime * StopSpeed);
                moveSpeed = _endSpeed * deltaTime;
                needScroll = true;
            }
        }

        if (needScroll)
        {
            switch (MouseDirection)
            {
                case SceneScrollMouseDirection.X:
                    moveDistance = direction.x;
                    break;
                case SceneScrollMouseDirection.Y:
                    moveDistance = direction.y;
                    break;
                case SceneScrollMouseDirection.NegativeX:
                    moveDistance = -direction.x;
                    break;
                case SceneScrollMouseDirection.NegativeY:
                    moveDistance = -direction.y;
                    break;
            }

            var offset = moveDistance * moveSpeed;
            Move(offset);
        }

        if (!_isMouseDown && OverScroll && isOverViewport)
        {
            var targetPosition = ClampPosition(CurrentPosition);
            CurrentPosition = Vector3.Lerp(CurrentPosition, targetPosition, deltaTime * ReBoundSpeed);
        }
    }
}
