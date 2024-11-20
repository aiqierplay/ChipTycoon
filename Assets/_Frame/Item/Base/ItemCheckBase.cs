using System.Collections;

public abstract class ItemCheckBase : ItemBase<Player>
{
    public override void OnTargetEffect(Player target)
    {
        var check = Check(target);
        if (check) OnCheckSuccess(target);
        else OnCheckFailed(target);
    }

    public abstract bool Check(Player target);

    public virtual void OnCheckSuccess(Player target)
    {
        StartCoroutine(SuccessCo(target));
    }

    public abstract IEnumerator SuccessCo(Player target);

    public virtual void OnCheckFailed(Player target)
    {
        StartCoroutine(FailedCo(target));
    }

    public abstract IEnumerator FailedCo(Player target);
}
