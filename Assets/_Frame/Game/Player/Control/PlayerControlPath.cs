using Aya.Extension;
using UnityEngine;

public class PlayerControlPath : PlayerControl
{
    public override PlayerControlMode ControlMode => PlayerControlMode.Path;

    public bool AutoMove;
    public float RotateAngle = 30f;

    private bool _isMouseDown;
    private Vector3 _startMousePos;
    private Vector3 _lastMousePos;
    private float _startX;
    private float _rotateY;
    private float _targetRotateY;

    public override void InitComponent()
    {
        base.InitComponent();
        _rotateY = 0f;
        _targetRotateY = 0f;
        _isRun = false;
    }

    private bool _isRun;

    public override void UpdateImpl(float deltaTime)
    {
        var canInput = IsGaming && State.EnableInput;
        var turnX = Self.Render.RenderTrans.localPosition.x;
        if (canInput)
        {
            if (Input.GetMouseButtonDown(0) || (!_isMouseDown && Input.GetMouseButton(0)))
            {
                _isMouseDown = true;
                _startMousePos = _lastMousePos = Input.mousePosition;
                _startX = Self.Render.RenderTrans.localPosition.x;
                if (!AutoMove) Self.Play(AnimatorDefine.Walk);
            }

            if (Input.GetMouseButtonUp(0))
            {
                _isMouseDown = false;
                _rotateY = 0f;
                if (_isRun && !State.AutoMove)
                {
                    if (!AutoMove) Self.Play(AnimatorDefine.Idle);
                    _isRun = false;
                }
            }

            var canMove = AutoMove || (!AutoMove && _isMouseDown);
            var needMove = canMove || State.AutoMove;
            if (needMove)
            {
                _isRun = true;
                var nextPathPos = Move.MovePath(Move.MoveSpeedActually * deltaTime);
                var nextPos = nextPathPos;

                if (nextPos != transform.position)
                {
                    if (!State.KeepDirection)
                    {
                        var rotation = Quaternion.Lerp(Trans.rotation, Quaternion.LookRotation(nextPos - Trans.position), deltaTime * Move.RotateSpeed).eulerAngles;
                        if (Move.KeepUp)
                        {
                            rotation.x = 0f;
                        }

                        Trans.eulerAngles = rotation;
                    }
                }
            }

            if (_isMouseDown)
            {
                var currentMousePos = Input.mousePosition;
                var offset = currentMousePos - _lastMousePos;

                // Turn
                turnX = _startX + (currentMousePos - _startMousePos).x * Move.TurnSpeed / 200f;

                var offsetX = currentMousePos.x - _lastMousePos.x;
                var rotateDiff = 5;
                if (Mathf.Abs(offsetX) < rotateDiff)
                {
                    _targetRotateY = 0f;
                }
                else
                {
                    _targetRotateY = offsetX > rotateDiff ? RotateAngle : (offsetX < -rotateDiff ? -RotateAngle : 0f);
                }

                _lastMousePos = currentMousePos;
            }
            else
            {
                _targetRotateY = 0f;
            }
        }

        _rotateY = Mathf.LerpAngle(_rotateY, _targetRotateY, deltaTime * 10f);
        Render.RenderTrans.transform.SetLocalEulerAnglesY(_rotateY);

        State.TurnRange = Move.CurrentPath.TurnRange;
        turnX = Mathf.Clamp(turnX, State.TurnRange.x, State.TurnRange.y);
        var poxX = Mathf.Lerp(Self.Render.RenderTrans.localPosition.x, turnX, Move.TurnLerpSpeed * deltaTime);
        Self.Render.RenderTrans.SetLocalPositionX(poxX);
    }

    public override void ClearInput()
    {
        _isMouseDown = false;
        //    _startMousePos = Input.mousePosition;
        // _startX = Self.Render.RenderTrans.localPosition.x;
    }
}
