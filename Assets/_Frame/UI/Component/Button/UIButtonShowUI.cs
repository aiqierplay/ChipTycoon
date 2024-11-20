using Aya.Reflection;

public class UIButtonShowUI : UIButton
{
    [TypeReference(typeof(UIPage))] public TypeReference Type;
    public string[] Args;

    public override void OnClickImpl()
    {
        if (Args != null) UI.Show(Type, Args as object[]);
    }
}
