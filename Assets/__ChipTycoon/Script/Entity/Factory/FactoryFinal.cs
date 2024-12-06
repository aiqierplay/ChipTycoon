using System.Collections;
using System.Collections.Generic;
using Aya.Async;
using Aya.TweenPro;
using Sirenix.OdinInspector;
using UnityEngine;

public class FactoryFinal : FactoryBase
{
    [Title("Carr")]
    public Transform CarTrans;
    public int CarCapacity = 10;
    public StackList CarStackList;
    public UTweenPlayerReference TweenCarEnter;
    public UTweenPlayerReference TweenCarExit;
    public float CarWaitInterval = 1f;

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
            var tweenEnter = TweenCarEnter.Value.Animation;
            yield return tweenEnter.WaitForComplete();

            while (CarStackList.Count < CarCapacity)
            {
                if (Input.Count > 0)
                {
                    var product = Input.StackList.Pop();
                    CarStackList.AddParabola(product);
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
            var tweenExit = TweenCarExit.Value.Animation;
            yield return tweenExit.WaitForComplete();

            CarStackList.Clear();
        }
    }
}
