using Aya.Extension;
using Aya.Reflection;

public class DraggableCamTarget : MovableOnPlane
{
    [TypeReference(typeof(UIPage))] public TypeReference WorkableUI;

    public void Init()
    {
        TargetObject.transform.ResetLocalPosition();
    }

    public override void Update()
    {
        if (UI == null) return;
        if (UI.Current == null) return;
        if (UI.Current.GetType() != WorkableUI.Type) return;
        base.Update();
    }
}