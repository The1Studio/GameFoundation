namespace GameFoundation.Scripts.Ads
{
    using System;
    using GoogleMobileAds.Api;
    using UnityEngine.Advertisements;

    public class AdsConfig
    {
        public bool UnityAdsEnable;
        public bool AdMobEnable;

        public UnityAdsConfig UnityAdsConfig;
        public AdmobConfig    AdmobConfig;
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

    [Serializable]
    public class AdmobConfig
    {
        public string     iosAdsID;
        public string     androidAdsID;
        public AdPosition bannerPosition;
    }
}