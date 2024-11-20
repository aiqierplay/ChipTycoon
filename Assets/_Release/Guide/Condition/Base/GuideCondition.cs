using System;

[Serializable]
public abstract class GuideCondition
{
    public AppManager App => AppManager.Ins;
    public LevelManager Level => LevelManager.Ins;
    public Level CurrentLevel => Level.Level;
    public Player Player => App.Player;

    public abstract bool Check();
}
