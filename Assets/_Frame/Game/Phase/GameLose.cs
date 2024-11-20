using System.Globalization;
using Aya.Analysis;
using Aya.Extension;

public class GameLose : GamePhaseHandler
{
    public override GamePhaseType Type => GamePhaseType.Lose;

    public override void Enter(params object[] args)
    {
        this.ExecuteDelay(() =>
        {
            Camera.Switch("Finish", CameraSwitchData.GameLose);
            State.GameEnd();
            UI.Show<UILose>();
        }, GeneralSetting.Ins.LoseWaitDuration);

        Dispatch(GameEvent.Lose);
        AnalysisManager.Instance.LevelFailed(Save.LevelIndex.ToString(CultureInfo.InvariantCulture));
        SDKUtil.Event("Game Lose " + Save.LevelIndex);
        // SDKUtil.Event("Game Lose Duration ", "Level", Save.LevelIndex, "Time", State.TotalTime);
    }

    public override void UpdateImpl()
    {

    }

    public override void Exit()
    {

    }
}
