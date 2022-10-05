namespace GameFoundation.Scripts.Ads
{
    using System;

    public interface IAdsService
    {
        Action OnRewardSucceed { get; set; }
        void   InitializeAds();
        void   LoadAds();
        void   ShowBannerAds();
        void   HideBannerAds();
        void   ShowInterstitialAds();
        void   ShowRewardAds();
    }
}