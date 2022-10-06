namespace GameFoundation.Scripts.Ads
{
    using System;
    using UnityEngine.Advertisements;

    public class UnityAdsService : IAdsService, IUnityAdsListener
    {
        private readonly string         gameID;
        private readonly string         bannerAdsID;
        private readonly string         interstitialAdsID;
        private readonly string         rewardAdsID;
        private readonly BannerPosition bannerPos;

        private const float TimeOutAdsRequest = 1f;

        public UnityAdsService(string gameID, string bannerAdsID, string interstitialAdsID, string rewardAdsID, BannerPosition bannerPos)
        {
            this.gameID            = gameID;
            this.bannerAdsID       = bannerAdsID;
            this.interstitialAdsID = interstitialAdsID;
            this.rewardAdsID       = rewardAdsID;
            this.bannerPos         = bannerPos;
        }


        #region Inplement IAdsService

        public Action OnRewardSucceed { get; set; }
        public bool   IsBannerReady   => Advertisement.IsReady(this.bannerAdsID);

        public bool IsInterstitialReady => Advertisement.IsReady(this.interstitialAdsID);

        public bool IsRewardReady => Advertisement.IsReady(this.rewardAdsID);

        public void InitializeAds()
        {
            Advertisement.Initialize(this.gameID);
            Advertisement.AddListener(this);
        }

        public void LoadAds()
        {
            Advertisement.Load(this.bannerAdsID);
            Advertisement.Load(this.interstitialAdsID);
            Advertisement.Load(this.rewardAdsID);
        }

        public void ShowBannerAds()
        {
            Advertisement.Load(this.bannerAdsID);
            Advertisement.Banner.SetPosition(this.bannerPos);
            Advertisement.Banner.Show(this.bannerAdsID);
        }
        public void HideBannerAds() { Advertisement.Banner.Hide(); }

        public void ShowInterstitialAds()
        {
            Advertisement.Load(this.interstitialAdsID);
            Advertisement.Show(this.interstitialAdsID);
        }

        public void ShowRewardAds()
        {
            Advertisement.Load(this.rewardAdsID);
            Advertisement.Show(this.rewardAdsID);
        }

        #endregion

        #region Inplement IUnityAdsListener

        public void OnUnityAdsReady(string placementId) { }

        public void OnUnityAdsDidError(string message) { }

        public void OnUnityAdsDidStart(string placementId) { }

        public void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
        {
            if (placementId == this.rewardAdsID && showResult == ShowResult.Finished)
            {
                this.OnRewardSucceed.Invoke();
            }
        }

        #endregion
    }
}