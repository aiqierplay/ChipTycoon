
public enum PlayerControlMode
{
    Empty = -1,
    Path = 0,
    Omnidirectional = 1,
    Shoot = 2,
    Throw = 3,
    Fps = 4,
}

public abstract class PlayerControl : PlayerBase
{
    public abstract PlayerControlMode ControlMode { get; }

    public virtual void Update()
    {
        var deltaTime = DeltaTime;
        if (!IsGaming) return;
        if (!State.EnableInput) return;
        UpdateImpl(deltaTime);
    }

    public virtual void UpdateImpl(float deltaTime)
    {

    }

    public virtual void ClearInput()
    {

    }
}

public partial class Player
{
    public virtual void SwitchControl(PlayerControlMode mode)
    {
        var controlList = GetComponentsInChildren<PlayerControl>();
        foreach (var playerControl in controlList)
        {
            var isCurrent = playerControl.ControlMode == mode;
            playerControl.enabled = isCurrent;
            if (isCurrent)
            {
                Control = playerControl;
            }
        }

        foreach (var playerBase in ComponentDic.Values)
        {
            playerBase.Control = Control;
            playerBase.Control.InitComponent();
        }

        Control.InitComponent();
    }
}