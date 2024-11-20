using System;
using Sirenix.OdinInspector;
using UnityEngine;

public abstract class UIRvPopUpWindow<T> : UIPage<T> where T : UIRvPopUpWindow<T>
{
    [Title("RV Pop Up")]
    public float ShowInterval;

    [NonSerialized] public float LastShowTime = float.MinValue;

    public virtual bool CanShow
    {
        get
        {
            var enablePopUp = ABTestSetting.Ins.GetValue<bool>("RVPopUp");
            if (!enablePopUp) return false;
            var time = Time.realtimeSinceStartup;
            var c1 = time - LastShowTime >= ShowInterval;
            var c2 = !(UI.Current is T);
            var c3 = SDKUtil.IsRewardVideoReady();
            if (c1 && c2 && c3)
            {
                return true;
            }

            return false;
        }
    }

    public override void Show(params object[] args)
    {
        base.Show(args);
        LastShowTime = Time.realtimeSinceStartup;
    }
}
