using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIWin : UIPage<UIWin>
{
    public void NextLevel()
    {
        Dispatch(GameEvent.NextLevel);
        Level.NextLevel();
    }
}
