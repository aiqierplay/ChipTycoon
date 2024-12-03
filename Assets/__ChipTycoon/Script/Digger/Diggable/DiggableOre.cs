using Aya.Util;
using UnityEngine;

public class DiggableOre : DiggableBase
{
    public override void OnEnterImpl(DiggerTool digger)
    {  
        CreateDropProduct(Position);
    }

    public static void CreateDropProduct(Vector3 position)
    {
        var prefab = GeneralSetting.Ins.DropPrefab;
        var dropItem = AppManager.Ins.GamePool.Spawn(prefab, AppManager.Ins.CurrentLevel.Trans, position + RandUtil.RandVector3(-0.05f, 0.05f));
        AppManager.Ins.World.DiggerArea.DropProductList.Add(dropItem);
        dropItem.Init();
        dropItem.Prefab = prefab;
    }
}
