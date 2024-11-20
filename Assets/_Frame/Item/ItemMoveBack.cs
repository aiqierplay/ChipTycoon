using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemMoveBack : ItemBase<Player>
{
    public override void OnTargetEffect(Player target)
    {
        target.Move.NotAllowMoveBounce();
    }
}
