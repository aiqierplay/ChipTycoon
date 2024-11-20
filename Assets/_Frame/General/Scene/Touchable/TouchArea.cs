using System;
using UnityEngine;

public class TouchArea : Touchable
{
    public GameObject TargetObject;
    public LayerMask TouchLayer;
    public float CheckDistance = 100f;
    public Vector3 TouchOffset;
    public bool BlockWithUI = true;

    [NonSerialized] public Vector3 CurrentTouchPos;
    [NonSerialized] public RaycastHit CurrentRayCastHit;

    public virtual void Update()
    {
        if (Input.GetMouseButton(0))
        {
            var inputPos = Input.mousePosition + TouchOffset;
            var rayOriginalPos = Camera.MainCamera.ScreenPointToRay(inputPos);
            var isOverUI = BlockWithUI && UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject();
            var hit = Physics.Raycast(rayOriginalPos, out var hitInfo, CheckDistance, TouchLayer.value);

            var touching = hit && hitInfo.collider.gameObject == TargetObject && !isOverUI;
            if (touching)
            {
                CurrentRayCastHit = hitInfo;
                CurrentTouchPos = hitInfo.point;
            }

            if (touching && !IsTouching)
            {
                TouchStart(hitInfo.point);
            }
            else if (!touching && IsTouching)
            {
                TouchEnd(hitInfo.point);
            }

            if (IsTouching)
            {
                Touching(hitInfo.point);
            }
        }
        else
        {
            if (IsTouching)
            {
                TouchEnd(Vector3.zero);
            }
        }
    }
}