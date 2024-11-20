using Aya.Extension;
using Sirenix.OdinInspector;
#if UNITY_EDITOR
using UnityEditorInternal;
#endif

public class TagManager : EntityBase<TagManager>
{
    public string Road;

#if UNITY_EDITOR
    [Button(SdfIconType.ArrowRepeat, " Auto Cache")]
    public void AutoCache()
    {
        var tags = InternalEditorUtility.tags;
        GetType()
            .GetFields()
            .FindAll(f => f.FieldType == typeof(string))
            .ForEach(f =>
            {
                f.SetValue(this, tags.Contains(s => s == f.Name) ? f.Name : "");
            });
    }
#endif
}
