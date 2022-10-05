namespace GameFoundation.Scripts.Ads
{
    using System;
    using UnityEngine.Advertisements;

    public class AdsConfig
    {
        public bool UnityAdsEnable;
        public bool AdMobEnable;

        public UnityAdsConfig UnityAdsConfig;
    }

    [Serializable]
    public class UnityAdsConfig
    {
        public string         iosGameID;
        public string         androidGameID;
        public string         bannerID;
        public string         interstitialID;
        public string         rewardID;
        public BannerPosition bannerPos;
    }
}