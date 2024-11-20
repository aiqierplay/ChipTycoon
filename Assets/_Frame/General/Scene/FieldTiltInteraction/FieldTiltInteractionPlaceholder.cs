
public class FieldTiltInteractionPlaceholder : EntityBase
{
    protected override void OnEnable()
    {
        base.OnEnable();
        FieldTiltInteractionManager.Ins.SetPlaceHolder(this);
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        FieldTiltInteractionManager.Ins.Placeholder = null;
    }
}
