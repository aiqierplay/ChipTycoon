using System;

public abstract class DropBase : EntityBase
{
    public int Value;
    [NonSerialized] public DropBase Prefab;

    public void Init()
    {
        EnablePhysic();
        World.DropList.Add(this);
    }

    public void EnablePhysic()
    {
        Rigidbody.isKinematic = false;
    }

    public void DisablePhysic()
    {
        Rigidbody.isKinematic = true;
    }

    public void Get()
    {
        GetImpl();
        DeSpawn();
    }

    public abstract void GetImpl();

    public void DeSpawn()
    {
        World.DropList.Remove(this);
        GamePool.DeSpawn(this);
    }
}
