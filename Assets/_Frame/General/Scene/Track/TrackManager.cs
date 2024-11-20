using System;
using System.Collections.Generic;

public class TrackManager : EntityBase
{
    [GetComponentInChildren, NonSerialized] public List<TrackLine> TrackLineList;

    public int Count => TrackLineList.Count;

    public void Init()
    {
        foreach (var trackLine in TrackLineList)
        {
            trackLine.Init();
        }
    }

    public void SetLine(TrackMover mover, int index)
    {
        var line = TrackLineList[index];
        mover.Init(line);
    }
}
