namespace GameFoundation.Scripts.Ads
{
    public interface IAdsService
    {
        bool IsAdsReady();
        void InitializeAds();
        void LoadAds();
        void ShowBannerAds();
        void ShowInterstitialAds();
        void ShowRewardAds();
    }
}