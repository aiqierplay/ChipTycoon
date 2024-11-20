using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class ItemChangeGamePhase : ItemBase<Player>
{
    [Title("Game Phase")]
    public GamePhaseType GamePhase;
    public string[] Args;

    public override void OnTargetEffect(Player target)
    {
        if (!target.IsPlayer) return;
        if (GamePhase == GamePhaseType.Win)
        {
            target.Win();
        }
        else if (GamePhase == GamePhaseType.Lose)
        {
            target.Lose();
        }
        else
        {
            App.Enter(GamePhase, Args as object[]);
        }
    }
}
