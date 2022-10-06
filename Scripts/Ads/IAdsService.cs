namespace GameFoundation.Scripts.Ads
{
    using System;

    public interface IAdsService
    {
        Action OnRewardSucceed     { get; set; }
        bool   IsBannerReady       { get; }
        bool   IsInterstitialReady { get; }
        bool   IsRewardReady       { get; }
        void   InitializeAds();
        void   LoadAds();
        void   ShowBannerAds();
        void   HideBannerAds();
        void   ShowInterstitialAds();
        void   ShowRewardAds();
    }
}