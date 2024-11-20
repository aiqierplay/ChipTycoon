using System.Globalization;
using Aya.Analysis;
using Aya.Extension;

public class GameWin : GamePhaseHandler
{
    public override GamePhaseType Type => GamePhaseType.Win;

    public override void Enter(params object[] args)
    {
        this.ExecuteDelay(() =>
        {
            Camera.Switch("Finish", CameraSwitchData.GameWin);
            State.GameEnd();
            UI.Show<UIWin>();
        }, GeneralSetting.Ins.WinWaitDuration);

        CurrentLevel.Info.Pass();
        CurrentLevel.NextInfo.UnLock();

        Dispatch(GameEvent.Win);
        AnalysisManager.Instance.LevelCompleted(Save.LevelIndex.ToString(CultureInfo.InvariantCulture));
        SDKUtil.Event("Game Win " + Save.LevelIndex);
        // SDKUtil.Event("Game Win Duration ", "Level", Save.LevelIndex, "Time", State.TotalTime);
    }

    public override void UpdateImpl()
    {

    }

    public override void Exit()
    {

    }
}
