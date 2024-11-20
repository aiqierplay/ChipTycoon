using Aya.Extension;
using Aya.Util;
using Sirenix.OdinInspector;
using UnityEngine;

public class LayerManager : EntityBase<LayerManager>
{
    public new LayerMask Player;
    public LayerMask Item;
    public LayerMask Road;
    public LayerMask Wall;

    public LayerMask Diggable;
    public LayerMask DropItem;

    [Button(SdfIconType.ArrowRepeat, " Auto Cache")]
    public void AutoCache()
    {
        GetType()
            .GetFields()
            .FindAll(f => f.FieldType == typeof(LayerMask))
            .ForEach(f =>
            {
                f.SetValue(this, LayerUtil.NameToMask(f.Name));
            });
    }
}
