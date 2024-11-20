using Aya.Events;
using UnityEngine.UI;

public enum SwitchLevelType
{
    Previous = 0,
    Next = 1,
    Current = 2,
}

public class UISwitchLevelBtn : UIButton
{
    public SwitchLevelType Mode;
    public Text TextLevel;

    [Listen(GameEvent.LoadLevel)]
    public override void Refresh(bool immediately = false)
    {
        base.Refresh(immediately);
        if (Level == null || CurrentLevel == null) return;
        if (TextLevel != null)
        {
            if (!CanInteractable())
            {
                TextLevel.text = "";
            }
            else
            {
                switch (Mode)
                {
                    case SwitchLevelType.Previous:
                        TextLevel.text = (CurrentLevel.Index - 1).ToString();
                        break;
                    case SwitchLevelType.Next:
                        TextLevel.text = (CurrentLevel.Index + 1).ToString();
                        break;
                    case SwitchLevelType.Current:
                        TextLevel.text = CurrentLevel.Index.ToString();
                        break;
                }
            }
        }
    }

    public override bool CanInteractable()
    {
        var result = base.CanInteractable();
        if (!result) return false;
        if (Level == null || CurrentLevel == null) return false;
        switch (Mode)
        {
            case SwitchLevelType.Previous:
                return CurrentLevel.Index - 1 >= 1;
            case SwitchLevelType.Next:
                if (Level.MaxLevel <= 0) return true;
                return CurrentLevel.Index + 1 <= Level.MaxLevel;
            case SwitchLevelType.Current:
                return true;
        }

        return true;
    }

    public override void OnClickImpl()
    {
        var index = 0;
        switch (Mode)
        {
            case SwitchLevelType.Previous:
                index = CurrentLevel.Index - 1;
                break;
            case SwitchLevelType.Next:
                index = CurrentLevel.Index + 1;
                break;
            case SwitchLevelType.Current:
                index = CurrentLevel.Index;
                break;
        }

        Level.LevelStart(index);
    }
}
