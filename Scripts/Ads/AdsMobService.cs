namespace GameFoundation.Scripts.Ads
{
    using System;

    public class AdsMobService : IAdsService
    {
        public Action OnRewardSucceed { get; set; }

        public bool IsAdsReady() { throw new NotImplementedException(); }

        public void InitializeAds() { throw new NotImplementedException(); }

        public void LoadAds() { throw new NotImplementedException(); }

        public void ShowBannerAds() { throw new NotImplementedException(); }
        public void HideBannerAds() { throw new NotImplementedException(); }

        public void ShowInterstitialAds() { throw new NotImplementedException(); }

        public void ShowRewardAds() { throw new NotImplementedException(); }
    }
}