using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIGameProgress : UIBase
{
    public Image Progress;

    public Text Text1;
    public Text Text2;
    public Text Text3;
    public Text Text4;
    public Text Text5;

    public int GameProgressStartLevelIndex
    {
        get
        {
            var level = Save.LevelIndex.Value;
            var index = level / 5;
            var start = index * 5 + 1;
            if (level % 5 == 0)
            {
                start -= 5;
                index--;
            }

            return start;
        }
    }

    public override void Init()
    {
        base.Init();
        var level = Save.LevelIndex.Value;
        var start = GameProgressStartLevelIndex;
        var current = level - start + 1;

        Text1.text = (start).ToString();
        Text2.text = (start + 1).ToString();
        Text3.text = (start + 2).ToString();
        Text4.text = (start + 3).ToString();
        Text5.text = (start + 4).ToString();

        Progress.fillAmount = current * 1f / 5f;
    }
}
