#if UNITY_EDITOR
using Aya.TweenPro;
using UnityEditor;

[CustomPropertyDrawer(typeof(UTweenPlayerReference))]
public class UTweenPlayerReferenceDrawer : CustomTypeReferenceDrawer<UTweenPlayer>
{
    public override string GetDisplayName(UTweenPlayer value)
    {
        var name = value.gameObject.name;
        if (!string.IsNullOrEmpty(value.Animation.Identifier))
        {
            name += " (" + value.Animation.Identifier + ")";
        }

        return name;
    }
}

#endif