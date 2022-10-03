namespace GameFoundation.Scripts.Ads
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Zenject;

    public class AdsManager : IInitializable
    {
        private List<IAdsService> adsServices = new();

        public void Initialize()
        {
            foreach (var adsService in this.adsServices)
            {
                adsService.InitializeAds();
            }
        }

        public void ShowAds(AdsType adsType)
        {
            switch (adsType)
            {
                case AdsType.Banner:
                    this.ShowBannerAds();
                    break;
                case AdsType.Interstitial:
                    this.ShowInterstitialAds();
                    break;
                case AdsType.Reward:
                    this.ShowRewardAds();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(adsType), adsType, null);
            }
        }

        private void ShowBannerAds()
        {
            foreach (var adsService in this.adsServices.Where(adsService => adsService.IsAdsReady()))
            {
                adsService.ShowBannerAds();
            }
        }

        private void ShowInterstitialAds()
        {
            foreach (var adsService in this.adsServices.Where(adsService => adsService.IsAdsReady()))
            {
                adsService.ShowInterstitialAds();
            }
        }

        private void ShowRewardAds()
        {
            foreach (var adsService in this.adsServices.Where(adsService => adsService.IsAdsReady()))
            {
                adsService.ShowRewardAds();
            }
        }
    }

    public enum AdsType
    {
        Banner,
        Interstitial,
        Reward
    }
}