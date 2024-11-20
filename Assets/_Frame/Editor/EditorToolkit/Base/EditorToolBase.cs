#if UNITY_EDITOR
using Sirenix.OdinInspector;
using Sirenix.Utilities.Editor;

[LabelWidth(120)]
public abstract class EditorToolBase : SerializedScriptableObject
{
    public abstract string GetTitle();
    public abstract SdfIconType GetIcon();

}
#endif