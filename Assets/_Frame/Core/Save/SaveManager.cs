using System;
using Aya.Data.Persistent;

public class SaveManager : EntityBase<SaveManager>
{
    public sDateTime RegisterTime = new sDateTime(nameof(RegisterTime), DateTime.UtcNow);
    public sInt LaunchCount = new sInt(nameof(LaunchCount), 0);
    public sDateTime LastExitTime = new sDateTime(nameof(LastExitTime), DateTime.UtcNow);

    public sInt LevelIndex = new sInt(nameof(LevelIndex), 1);
    public sInt RandLevelIndex = new sInt(nameof(RandLevelIndex), 0);

    public sDecimal Coin;
    public sInt Diamond;
    public sInt Key;

    #region Business

    public sInt BusinessBlockIndex = new sInt(nameof(BusinessBlockIndex), 0);

    #endregion

    protected override void Awake()
    {
        base.Awake();

        LaunchCount++;
        Coin = new sDecimal(nameof(Coin), GetSetting<GeneralSetting>().DefaultCoin);
        Diamond = new sInt(nameof(Diamond), GetSetting<GeneralSetting>().DefaultDiamond);
        Key = new sInt(nameof(Key), GetSetting<GeneralSetting>().DefaultKey);
    }
}