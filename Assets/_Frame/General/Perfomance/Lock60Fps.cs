using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Lock60Fps : MonoBehaviour
{
    public void Awake()
    {
        // OnDemandRendering.renderFrameInterval = 60;
        Application.targetFrameRate = 60;
    }
}
