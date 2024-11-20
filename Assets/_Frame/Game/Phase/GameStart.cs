using System.Globalization;
using Aya.Analysis;

public class GameStart : GamePhaseHandler
{
    public override GamePhaseType Type => GamePhaseType.Start;

    public override void Enter(params object[] args)
    {
        Camera.Switch("Game", CameraSwitchData.GameStart);
        State.GameStart();
        UI.Show<UIGame>();
        App.Enter<GamePlay>();
        Dispatch(GameEvent.Start);

        SDKUtil.Event("Game Start " + Save.LevelIndex);
        AnalysisManager.Instance.LevelStart(Save.LevelIndex.ToString(CultureInfo.InvariantCulture));
    }

    public override void UpdateImpl()
    {

    }

    public override void Exit()
    {

    }
}
