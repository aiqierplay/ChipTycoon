using System.Collections;
using Aya.Async;
using Aya.Extension;
using Aya.TweenPro;
using Sirenix.OdinInspector;
using UnityEngine;

public class ItemSlide : ItemCheckBase
{
    [Title("Slide Start")] 
    public string SlideStartClip = AnimatorDefine.SlideStart;
    public Transform Start;
    public float StartDuration;
    public Transform SlideStart;
    [Title("Sliding")]
    public float SlideSpeed;
    [Title("Slide End")]
    public string SlideEndClip = AnimatorDefine.SlideEnd;
    public Transform SlideEnd;
    public float EndDuration;
    public Transform End;

    [Title("Slide Fail")]
    public Transform DropPoint;
    public float WaitForDrop;
    public AnimationCurve DropCurve;
    public float DropDuration;

    public override bool Check(Player target)
    {
        return true;
    }

    public override void OnCheckSuccess(Player target)
    {
        StartCoroutine(SuccessCo(target));
    }

    public override void OnCheckFailed(Player target)
    {
        StartCoroutine(FailedCo(target));
    }

    public override IEnumerator SuccessCo(Player target)
    {
        target.Move.DisableMove(false);
        UTween.LocalPosition(target.RendererTrans, Vector3.zero, 0.25f);
        var tweenMoveCenter = UTween.Position(target.Trans, Start, 0.25f);
        yield return tweenMoveCenter.WaitForComplete();

        target.Play(SlideStartClip);
        var tweenSlideStart = UTween.Position(target.Trans, SlideStart, StartDuration);
        yield return tweenSlideStart.WaitForComplete();

        var slideDuration = Vector3.Distance(SlideStart.position, SlideEnd.position) / SlideSpeed;
        var tweenSlide = UTween.Position(target.Trans, SlideStart, SlideEnd, slideDuration);//.SetSpeedBased();
        yield return tweenSlide.WaitForComplete();

        target.Play(SlideEndClip);
        var tweenSlideEnd = UTween.Position(target.Trans, End, EndDuration);
        yield return tweenSlideEnd.WaitForComplete();

        var dis = Vector3.Distance(Start.position, End.position);
        target.Move.MovePath(dis);
        target.Render.RenderTrans.SetLocalPositionX(0f);
        target.Move.EnableMove();

        target.Play(AnimatorDefine.Run);
    }

    public override IEnumerator FailedCo(Player target)
    {
        target.Move.DisableMove(false);
        UTween.LocalPosition(target.RendererTrans, Vector3.zero, 0.25f);
        var tweenMoveCenter = UTween.Position(target.Trans, Start, 0.25f);
        yield return tweenMoveCenter.WaitForComplete();

        target.Play(SlideStartClip);
        var tweenSlide = UTween.Position(target.Trans, Start, DropPoint, WaitForDrop);
        yield return YieldBuilder.WaitForSeconds(WaitForDrop);

        target.Play(AnimatorDefine.Drop);
        Camera.SetFollow(null);
        var tweenDrop = UTween.Position(target.Trans, target.Position + Vector3.down * 20f, DropDuration)
            .SetCurve(DropCurve);

        yield return tweenDrop.WaitForComplete();
        target.Lose();
    }
}
