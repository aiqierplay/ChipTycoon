using System;
using Aya.Render.Draw;
using Sirenix.OdinInspector;
using UnityEngine;

public class PaintArea : TouchArea
{
    [Title("Paint")]
    public Renderer TargetRenderer;
    public Color ClearColor = Color.white;
    public Color PaintColor = Color.black;
    public int PaintSize = 10;
    public Vector2Int TexSize = new Vector2Int(512, 512);

    [NonSerialized] public Texture2D TargetTexture;

    protected override void Awake()
    {
        base.Awake();
        TargetTexture = new Texture2D(TexSize.x, TexSize.y, TextureFormat.ARGB32, false);
        AutoFillGap = false;
        Init(PaintStart, PaintEnd, Paint);
    }

    public virtual void Init()
    {
        TargetTexture.Clear(ClearColor);
        TargetRenderer.material.mainTexture = TargetTexture;
    }

    private int _paintCount;
    private Vector2 _lastPaintUv;
    private Vector3 _lastTouchPos;

    public virtual void PaintStart(Vector3 touchPoint)
    {
        _paintCount = 0;
    }

    public virtual void PaintEnd(Vector3 touchPoint)
    {

    }

    public virtual void Paint(Vector3 touchPos)
    {
        TargetTexture.BeginDraw();

        var currentPaintUv = CurrentRayCastHit.textureCoord;
        var currentTouchPos = touchPos;
        if (_paintCount == 0)
        {
            _paintCount++;
            PaintImpl(currentTouchPos, currentPaintUv);
        }
        else
        {
            if (_lastTouchPos != currentTouchPos)
            {
                var startPos = _lastPaintUv * TexSize;
                var endPos = currentPaintUv * TexSize;
                var distance = (endPos - startPos).magnitude;
                var checkDis = PaintSize;
                if (distance > checkDis)
                {
                    var step = 1f / (distance / checkDis);
                    for (var i = 0f; i <= 1f; i += step)
                    {
                        var lerpTouchPos = Vector3.Lerp(_lastTouchPos, currentTouchPos, i);
                        var lerpUv = Vector2.Lerp(_lastPaintUv, currentPaintUv, i);
                        _paintCount++;
                        PaintImpl(lerpTouchPos, lerpUv);
                    }
                }

                _paintCount++;
                PaintImpl(currentTouchPos, currentPaintUv);
            }
            else _paintCount++;
        }

        _lastTouchPos = touchPos;
        _lastPaintUv = currentPaintUv;

        TargetTexture.EndDraw();
        TargetRenderer.material.mainTexture = TargetTexture;
    }

    public virtual void PaintImpl(Vector3 touchPos, Vector2 uvPos)
    {
        var paintPos = uvPos * TexSize;
        TargetTexture.FillCircle(paintPos, PaintSize, PaintColor);
    }
}
