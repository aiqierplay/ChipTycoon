using System.Collections.Generic;
using UnityEngine;

public class DetectorGroup
{
    public EntityBase Handler;
    public Vector3 Direction;
    public List<Detector> DetectorList;

    public void Init(EntityBase handler, Vector3 direction, List<Detector> detectorList)
    {
        Handler = handler;
        Direction = direction;
        DetectorList = detectorList;
    }

    public float GetValue()
    {
        var count = DetectorList.Count;
        var value = 0f;
        for(var i =0 ; i < count; i++)
        {
            var detector = DetectorList[i];
            value += detector.GetValueImpl(Handler, Direction);
        }

        return value;
    }
}
