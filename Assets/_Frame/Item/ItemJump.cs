using System.Collections;
using Aya.Async;
using Aya.Extension;
using Aya.TweenPro;
using Sirenix.OdinInspector;
using UnityEngine;

public enum ItemJumpMode
{
    Player = 0,
    Renderer = 1,
}

public class ItemJump : ItemCheckBase
{
    [Title("Jump")]
    public ItemJumpMode JumpMode;
    public string JumpClip = AnimatorDefine.Jump;
    public Transform JumpStart;
    public Transform JumpEnd;
    public float JumpDuration;
    public float JumpAnimationDuration;
    public float JumpHeight;

    [Title("Jump Fail")]
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
        StartCoroutine(SuccessCo2(target));
    }

    public override void OnCheckFailed(Player target)
    {
        StartCoroutine(FailedCo(target));
    }

    public override IEnumerator SuccessCo(Player target)
    {
        target.Move.DisableMove(false);
        var dis = Vector3.Distance(JumpStart.position, JumpEnd.position);
        target.Play(JumpClip);

        if (JumpMode == ItemJumpMode.Renderer)
        {
            target.Move.AutoMovePath(dis, JumpDuration);
            var start = target.WorldToLocalPosition(JumpStart.position);
            var end = target.WorldToLocalPosition(JumpEnd.position);
            var tweenJump = UTween.Value(0f, 1f, JumpDuration, value =>
            {
                var jumpPos = TweenParabola.GetPositionByFactor(start, end, JumpHeight, value);
                target.RendererTrans.SetLocalPositionY(jumpPos.y);
            });

            yield return tweenJump.WaitForComplete();
        }
        else
        {
            var tweenJump = UTween.Value(0f, 1f, JumpDuration, value =>
            {
                var jumpPos = TweenParabola.GetPositionByFactor(JumpStart.position, JumpEnd.position, JumpHeight, value);
                target.RendererTrans.SetPositionY(jumpPos.y);
                target.Trans.SetPositionXZ(jumpPos.x, jumpPos.z);
            });

            yield return tweenJump.WaitForComplete();
            target.Move.MovePath(dis);
        }

        target.Move.EnableMove();
    }

    public IEnumerator SuccessCo2(Player target)
    {
        yield return YieldBuilder.WaitForSeconds(JumpAnimationDuration);
        target.Play(AnimatorDefine.Run);
    }

    public override IEnumerator FailedCo(Player target)
    {
        target.Move.DisableMove(false);
        UTween.LocalPosition(target.RendererTrans, Vector3.zero, 0.25f);
        var tweenMoveCenter = UTween.Position(target.Trans, JumpStart, 0.25f);
        yield return tweenMoveCenter.WaitForComplete();

        target.Play(JumpClip);
        var tweenSlide = UTween.Position(target.Trans, JumpStart, DropPoint, WaitForDrop);
        yield return YieldBuilder.WaitForSeconds(WaitForDrop);

        var tweenDrop = UTween.Position(target.Trans, target.Position + Vector3.down * 20f, DropDuration)
            .SetCurve(DropCurve);

        yield return tweenDrop.WaitForComplete();
        target.Lose();
    }

    public void OnDrawGizmos()
    {
        if (JumpStart == null) return;
        if (JumpEnd == null) return;

        Gizmos.color = Color.cyan;
        for (var i = 0f; i < 1f; i += 0.01f)
        {
            var p1 = TweenParabola.GetPositionByFactor(JumpStart.position, JumpEnd.position, JumpHeight, i);
            var p2 = TweenParabola.GetPositionByFactor(JumpStart.position, JumpEnd.position, JumpHeight, i + 0.01f);
            Gizmos.DrawLine(p1, p2);
        }

    }
}
