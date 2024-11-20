using System;
using Aya.Extension;
using UnityEngine;
using UnityEngine.UI;

public class UIIndicator : UIBase
{
    public EntityBase Target;

    [GetComponentInChildren, NonSerialized]public Image Image;
    [NonSerialized] public Vector2 Size;
    [NonSerialized] public float Diagonal;
    [NonSerialized] public Vector3 Center;

    protected override void Awake()
    {
        base.Awake();
        Size = Rect.sizeDelta;
        Center = new Vector3(Screen.width / 2f, Screen.height / 2f, 0f);
        Diagonal = Mathf.Sqrt(Mathf.Pow(Screen.height / 2f, 2) + Mathf.Pow(Screen.width / 2f, 2));
    }

    public void SetTarget(EntityBase target)
    {
        Target = target;
        gameObject.name = "Indicator_" + target.name;
    }

    public virtual Vector3 GetCenterPos()
    {
        return Player.Position;
    }

    public void LateUpdate()
    {
        // if (Target == null) return;
        var isInView = Target.IsInView();
        Image.gameObject.SetActive(!isInView);

        if (!isInView)
        {
            var direction = Target.Position - GetCenterPos();
            direction.y = direction.z;
            direction.z = 0f;

            var angle = Angle360(Vector3.up, direction);
            Trans.SetEulerAnglesZ(angle);

            var screenPos = Vector3.zero;
            var radian = (360f - angle) * Mathf.PI / 180f;
            var offsetX = Mathf.Sin(radian) * Diagonal;
            var offsetY = Mathf.Cos(radian) * Diagonal;
            screenPos.x = Screen.width / 2f + offsetX;
            screenPos.y = Screen.height / 2f + offsetY;

            if (screenPos.x > Screen.width)
            {
                screenPos.x = Screen.width;
            }

            if (screenPos.x < 0f)
            {
                screenPos.x = 0f;
            }

            if (screenPos.y > Screen.height)
            {
                screenPos.y = Screen.height;
            }

            if (screenPos.y < 0f)
            {
                screenPos.y = 0f;
            }

            var worldPos = UICamera.ScreenToWorldPoint(screenPos);
            worldPos.z = 0f;
            Position = worldPos;
        }
    }

    public float Angle360(Vector3 from, Vector3 to)
    {
        var v3 = Vector3.Cross(from, to);
        if (v3.z > 0f)
        {
            return Vector3.Angle(from, to);
        }
        else
        {
            return 360f - Vector3.Angle(from, to);
        }
    }
}
