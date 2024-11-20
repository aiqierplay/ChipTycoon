using System.Collections.Generic;
using UnityEngine;
using Aya.SDK;

namespace Aya.AD
{
    [CreateAssetMenu(menuName = "SDK/Setting/AD Setting")]
    public class ADSetting : SDKSettingBase<ADSetting>
    {
        [Header("App")]
        public string AppID;

        [Header("Banner")]
        public List<ADLocationParam> BannerList;

        [Header("Interstitial")]
        public List<ADLocationParam> InterstitialList;

        [Header("RewardedVideo")]
        public List<ADLocationParam> RewardedVideoList;
    }
}