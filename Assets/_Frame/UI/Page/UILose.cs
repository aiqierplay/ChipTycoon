using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UILose : UIPage<UILose>
{
    public void Retry()
    {
        Dispatch(GameEvent.RetryLevel);
        Level.LevelRetry();
    }
}
