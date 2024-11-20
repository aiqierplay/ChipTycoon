using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestTimer : EntityBase
{
    public Text Text;
    private float _timer;

    void Update()
    {
        _timer += DeltaTime;
        Text.text = _timer.ToString("F2");
    }
}
