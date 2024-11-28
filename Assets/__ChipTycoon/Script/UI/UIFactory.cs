using System;
using System.Collections.Generic;

public class UIFactory : UIPage<UIFactory>
{
    [GetComponentInChildren, NonSerialized]
    public List<UIUpgradeButton> UpgradeList;

    [NonSerialized] public BuildingFactory Factory;

    public override void Show(params object[] args)
    {
        base.Show(args);
        Factory = args[0] as BuildingFactory;
        foreach (var upgradeButton in UpgradeList)
        {
            upgradeButton.UpgradeInfo.AssetPath = Factory.DataKey;
            upgradeButton.UpgradeInfo.Init();
            upgradeButton.Refresh();
        }
    }

    public override void Back()
    {
        base.Back();
        World.Character.EnableMove();
    }
}
