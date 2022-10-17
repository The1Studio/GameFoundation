namespace GameFoundation.Scripts.Ads
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;

    public class AdsManager
    {
        #region Zenject

        private readonly AdsConfig adsConfig;

        #endregion

        private List<IAdsService> adsServices = new();

        public AdsManager(AdsConfig adsConfig) { this.adsConfig = adsConfig; }

        public bool IsConnection => Application.internetReachability != NetworkReachability.NotReachable;

        public void InitializeAds()
        {
            if (!this.IsConnection) return;

            this.InitAdsServices();

            foreach (var adsService in this.adsServices)
            {
                adsService.InitializeAds();
                adsService.LoadAds();
            }
        }

        private void InitAdsServices()
        {
            var    unityAdsConfig = this.adsConfig.UnityAdsConfig;
            var    admobConfig    = this.adsConfig.AdmobConfig;
            string adsUnitID;
            string gameID;

#if UNITY_IOS
            gameID = unityAdsConfig.iosGameID;
            adsUnitID = admobConfig.iosAdsID;
#else
            adsUnitID = admobConfig.androidAdsID;
            gameID    = unityAdsConfig.androidGameID;
#endif

            if (this.adsConfig.UnityAdsEnable)
            {
                this.adsServices.Add(new UnityAdsService(gameID, unityAdsConfig.bannerID, unityAdsConfig.interstitialID, unityAdsConfig.rewardID, unityAdsConfig.bannerPos));
            }

            if (this.adsConfig.AdMobEnable)
            {
                this.adsServices.Add(new AdsMobService(adsUnitID, admobConfig.bannerPosition));
            }
        }

        public void ShowBannerAds()
        {
            if (!this.IsConnection) return;

            this.adsServices.FirstOrDefault(adService => adService.IsBannerReady)?.ShowBannerAds();
        }

        public void HideBannerAds()
        {
            foreach (var adsService in this.adsServices)
            {
                adsService.HideBannerAds();
            }
        }

        public void ShowInterstitialAds()
        {
            if (!this.IsConnection) return;

            this.adsServices.FirstOrDefault(adService => adService.IsInterstitialReady)?.ShowInterstitialAds();
        }

        public void ShowRewardAds(Action onRewardSucceed)
        {
            if (!this.IsConnection) return;

            var adsShow = this.adsServices.FirstOrDefault(adService => adService.IsRewardReady);
            if (adsShow == null) return;

            adsShow.ShowRewardAds();
            adsShow.OnRewardSucceed = onRewardSucceed;
        }
    }
}