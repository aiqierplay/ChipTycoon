using System.Collections;
using System.Collections.Generic;
using Aya.Physical;
using UnityEngine;

public class DiggableBomb : DiggableBase
{
    public float Radius = 3f;

    public override void OnEnterImpl(DiggerTool digger)
    {
        var oreList = PhysicsUtil.OverlapSphere<DiggableOre>(Position, Radius, LayerManager.Ins.Diggable);
        foreach (var ore in oreList)
        {
            ore.OnEnterForce(DiggerArea.DiggerTool, true);
        }
    }
}
