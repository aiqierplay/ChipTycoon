
public abstract class UISingletonMaskPage<T> : UISingletonMaskPage where T : UISingletonMaskPage<T>
{
    public static T Ins { get; protected set; }

    protected override void Awake()
    {
        base.Awake();
        Ins = this as T;
    }
}