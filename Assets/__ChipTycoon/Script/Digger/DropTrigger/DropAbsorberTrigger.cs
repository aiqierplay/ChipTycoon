using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropAbsorberTrigger : DropTriggerBase
{
    public float Speed = 5f;

    public override void OnEnterImpl(DropBase dropItem)
    {
        StartCoroutine(AbsorberCo(dropItem));
    }

    public IEnumerator AbsorberCo(DropBase dropItem)
    {
        dropItem.DisablePhysic();
        while (true)
        {
            if (CurrentLevel == null || World == null || World.DiggerArea.DiggerTool.CurrentTool == null)
            {
                yield return null;
                continue;
            }

            var targetPos = World.DiggerArea.DiggerTool.CurrentTool.Target.transform.position;
            var fromPos = dropItem.Position;
            var pos = Vector3.Lerp(fromPos, targetPos, Speed * DeltaTime);
            dropItem.Position = pos;
            var dis = (targetPos - pos).magnitude;
            if (dis < 0.1f) break;
            yield return null;
        }

        dropItem.Get();
    }
}
