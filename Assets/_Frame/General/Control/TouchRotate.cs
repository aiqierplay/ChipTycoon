using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;

public class TouchRotate : EntityBase
{
    public Transform Target;
    public Space SpaceMode = Space.World;
    public Vector2 RotationSpeed = new Vector2(10, 10);
    public bool BlockWithUi = true;
    public bool StopInertia = true;
    [ShowIf(nameof(StopInertia))]
    public float DampingFactor = 0.9f;

    private bool _isDragging = false;
    private bool _isStopping = false;
    private Vector3 _lastTouchPosition;
    private Vector3 _lastRotateSpeed;
    
    public void Update()
    {
        if (Target == null) return;
        if (Input.GetMouseButtonDown(0))
        {
            if (BlockWithUi && EventSystem.current.IsPointerOverGameObject())
            {
                return;
            }

            _lastTouchPosition = Input.mousePosition;
            _isDragging = true;
            _isStopping = false;
        }
        
        if (_isDragging && Input.GetMouseButtonUp(0))
        {
            _isDragging = false;
            if (StopInertia)
            {
                _isStopping = true;
            }
        }

        if (_isDragging)
        {
            var currentTouchPosition = Input.mousePosition;
            var speed = UnscaledDeltaTime * RotationSpeed;

            var direction = (currentTouchPosition - _lastTouchPosition) * 0.5f;
            var rotationX = -speed.x * direction.x;
            var rotationY = speed.y * direction.y;

            Target.Rotate(rotationY, rotationX, 0, SpaceMode);

            _lastTouchPosition = currentTouchPosition;
            _lastRotateSpeed = direction;
        }

        if (_isStopping)
        {
            _lastRotateSpeed *= DampingFactor;
            var rotationX = -_lastRotateSpeed.x * RotationSpeed.x * UnscaledDeltaTime;
            var rotationY = _lastRotateSpeed.y * RotationSpeed.y * UnscaledDeltaTime;
            Target.Rotate(rotationY, rotationX, 0, SpaceMode);
            if (_lastRotateSpeed.sqrMagnitude <= 0.001f)
            {
                _isStopping = false;
                _lastRotateSpeed = Vector3.zero;
            }
        }
    }
}
