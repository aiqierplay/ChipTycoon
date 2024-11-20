using System;

[Serializable]
public class BattleProperty
{
    public int Hp = 1;
    public int Power = 1;
    public float RunSpeed = 5f;
    public float RotateSpeed = 10f;
    public float SearchRange = 5f;
    public float AttackRange = 1f;
    public float AttackInterval = 1f;
    public float Size = 1f;

    public BattleProperty(bool reset = true)
    {
        if (reset) Reset();
    }

    public BattleProperty(BattleProperty property)
    {
        Copy(property);
    }

    public void Reset()
    {
        Hp = default;
        Power = default;
        RunSpeed = default;
        RotateSpeed = default;
        SearchRange = default;
        AttackRange = default;
        AttackInterval = default;
    }

    public void Copy(BattleProperty property)
    {
        Hp = property.Hp;
        Power = property.Power;
        RunSpeed = property.RunSpeed;
        RotateSpeed = property.RotateSpeed;
        SearchRange = property.SearchRange;
        AttackRange = property.AttackRange;
        AttackInterval = property.AttackInterval;
    }

    public void AddProperty(BattleProperty property)
    {
        Hp += property.Hp;
        Power += property.Power;
        RunSpeed += property.RunSpeed;
        RotateSpeed += property.RotateSpeed;
        SearchRange += property.SearchRange;
        AttackRange += property.AttackRange;
        AttackInterval += property.AttackInterval;
    }
}