using Aya.UI;
using UnityEngine;

[RequireComponent(typeof(CanvasRenderer))]
public class UITouchArea : Touchable
{
    protected override void Awake()
    {
        base.Awake();
        var listener = UIEventListener.Get(gameObject);
        listener.onDown += (go, data) =>
        {
            var touchPoint = Input.mousePosition;
            TouchStart(touchPoint);
        };

        listener.onUp += (go, data) =>
        {
            var touchPoint = Input.mousePosition;
            TouchEnd(touchPoint);
        };
    }

    public virtual void Update()
    {
        if (!IsTouching) return;
        var touchPoint = Input.mousePosition;
        Touching(touchPoint);
    }
}
