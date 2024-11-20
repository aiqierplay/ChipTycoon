
public class FieldTiltInteractionNode : EntityBase
{
    protected override void OnEnable()
    {
        base.OnEnable();
        FieldTiltInteractionManager.Ins.NodeList.Add(this);
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        FieldTiltInteractionManager.Ins.NodeList.Remove(this);
    }
}
