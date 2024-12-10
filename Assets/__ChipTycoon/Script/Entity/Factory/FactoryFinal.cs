using System.Collections;
using System.Collections.Generic;
using Aya.Async;
using Aya.TweenPro;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

public class FactoryFinal : FactoryBase
{
    [Title("Carr")]
    public Transform CarTrans;
    public int CarCapacity = 10;
    public StackList CarStackList;
    public UTweenPlayerReference TweenCarEnter;
    public string MoveClip;
    public UTweenPlayerReference TweenCarExit;
    public string IdleClip;
    public float CarWaitInterval = 1f;
    public TMP_Text TextCarValue;
    public TMP_Text TextCarCount;

    public override void Refresh()
    {
        base.Refresh();
        if (TextCarValue != null) TextCarValue.text = CarStackList.Count.ToString();
        if (TextCarCount != null) TextCarCount.text = CarCapacity.ToString();
    }

    public override IEnumerator WorkCo()
    {
        while (!Info.Unlock)
        {
            yield return YieldBuilder.WaitForSeconds(0.1f);
        }

        while (true)
        {
            yield return YieldBuilder.WaitForSeconds(CarWaitInterval);

            TweenCarEnter.Play();
            Play(MoveClip);
            var tweenEnter = TweenCarEnter.Value.Animation;
            yield return tweenEnter.WaitForComplete();
            Play(IdleClip);

            while (CarStackList.Count < CarCapacity)
            {
                if (Input.Count > 0)
                {
                    var product = Input.StackList.Pop();
                    CarStackList.AddParabola(product);
                    Refresh();
                }

                yield return null;
            }

            yield return YieldBuilder.WaitForSeconds(1f);

            for (var i = 0; i < Output.Number; i++)
            {
                Output.Add(1);
                yield return null;
            }

            TweenCarExit.Play();
            Play(MoveClip);
            var tweenExit = TweenCarExit.Value.Animation;
            yield return tweenExit.WaitForComplete();
            Play(IdleClip);

            CarStackList.Clear();
        }
    }
}
