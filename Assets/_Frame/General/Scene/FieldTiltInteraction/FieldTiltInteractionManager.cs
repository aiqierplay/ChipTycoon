using System;
using System.Collections.Generic;
using UnityEngine;

public class FieldTiltInteractionManager : EntityBase<FieldTiltInteractionManager>
{
    public float TriggerDistance = 1.0f; // 触发旋转的距离
    public float MaxTiltAngle = 30.0f; // 最大倾斜的角度
    public float TiltTime = 0.5f; // 倾斜所需时间
    public float RecoveryTime = 1.0f; // 恢复到原始角度的时间

    private Vector3 _previousPosition; // 记录玩家的前一个位置
    private float _tiltSpeed; // 倾斜速度
    private float _recoverySpeed; // 恢复速度

    [NonSerialized] public FieldTiltInteractionPlaceholder Placeholder;
    [NonSerialized] public List<FieldTiltInteractionNode> NodeList = new List<FieldTiltInteractionNode>();

    public void SetPlaceHolder(FieldTiltInteractionPlaceholder placeholder)
    {
        Placeholder = placeholder;
        _previousPosition = Placeholder.Position;
    }

    public void LateUpdate()
    {
        if (Placeholder == null)
        {
            return;
        }

        _tiltSpeed = MaxTiltAngle / TiltTime;
        _recoverySpeed = MaxTiltAngle / RecoveryTime;

        var playerMovementDirection = (Placeholder.Position - _previousPosition).normalized;
        if (playerMovementDirection == Vector3.zero) return;
        foreach (var node in NodeList)
        {
            if (node == null) continue;
            var distance = Vector3.Distance(node.Position, Placeholder.Position);
            if (distance < TriggerDistance)
            {
                var targetRotation = Quaternion.LookRotation(playerMovementDirection) * Quaternion.Euler(MaxTiltAngle, 0, 0);
                node.Rotation = Quaternion.RotateTowards(node.Rotation, targetRotation, _tiltSpeed * Time.deltaTime);
            }
            else
            {
                node.Rotation = Quaternion.RotateTowards(node.Rotation, Quaternion.identity, _recoverySpeed * Time.deltaTime);
            }
        }

        _previousPosition = Placeholder.Position;
    }
}
