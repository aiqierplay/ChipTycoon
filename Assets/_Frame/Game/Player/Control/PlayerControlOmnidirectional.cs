using Aya.Extension;
using Aya.Maths;
using Aya.Physical;
using Sirenix.OdinInspector;
using UnityEngine;

public class PlayerControlOmnidirectional : PlayerControl
{
    public override PlayerControlMode ControlMode => PlayerControlMode.Omnidirectional;

    public bool FixedStartPoint = true;
    private readonly float _checkDistance = 100f;

    private Vector3 _lastMousePosition;
    private bool _isMouseDown;

    public bool ClampPosQuad;
    [ShowIf(nameof(ClampPosQuad))] public Vector3 ClampMinPos;
    [ShowIf(nameof(ClampPosQuad))] public Vector3 ClampMaxPos;

    public bool ClampPosWithLayer;
    [ShowIf(nameof(ClampPosWithLayer))] public LayerMask WalkLayer;

    public override void InitComponent()
    {
        base.InitComponent();
        _isMouseDown = false;
    }

    public override void Update()
    {
        if (!IsGaming) return;
        if (!State.EnableInput)
        {
            _isMouseDown = false;
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            _isMouseDown = true;
            _lastMousePosition = Input.mousePosition;
        }

        if (Input.GetMouseButtonUp(0))
        {
            _isMouseDown = false;
        }

        var deltaTime = DeltaTime;
        UpdateImpl(deltaTime);
    }

    public virtual void LateUpdate()
    {
        if (!IsGaming) return;
        if (!State.EnableInput) return;


    }

    public override void UpdateImpl(float deltaTime)
    {
        if (!State.EnableInput) return;

        if (_isMouseDown)
        {
            var currentPosition = Input.mousePosition;
            var direction = currentPosition - _lastMousePosition;
            if (!FixedStartPoint)
            {
                if (direction.magnitude > _checkDistance)
                {
                    _lastMousePosition = currentPosition + -direction.normalized * _checkDistance;
                }
            }

            // UIGame.Ins.TouchCanvas.SetPosition(_lastMousePosition);
            // UIGame.Ins.TouchCanvas.SetDirection(direction);

            direction.z = direction.y;
            direction.y = 0;
            direction = direction.normalized;

            if (direction == Vector3.zero) return;

            var distance = Move.MoveSpeedActually * deltaTime;
            var targetPos = Position + direction * distance;
            // var position = Move.MoveDirection(direction, distance);

            var forward = (targetPos - Position).normalized;
            if (forward != Vector3.zero)
            {
                Forward = Vector3.Lerp(Forward, forward, DeltaTime * Move.RotateSpeed);
            }

            if (ClampPosQuad)
            {
                targetPos = MathUtil.Clamp(Position, ClampMinPos, ClampMaxPos);
                Position = targetPos;
            }

            if (ClampPosWithLayer)
            {
                var (target, pos) = PhysicsUtil.Raycast<Transform>(targetPos + Vector3.up, Vector3.down, 10f, WalkLayer);
                if (target == null) return;
                Position = targetPos;
            }
        }
        else
        {
            Rigidbody.ClearMomentum();
        }
    }

    public override void ClearInput()
    {
        _isMouseDown = false;
    }
}
