namespace GameFoundation.Scripts.Ads
{
    using System;
    using Cysharp.Threading.Tasks;
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

        public void InitializeAds()
        {
            Advertisement.Initialize(this.gameID);
            Advertisement.AddListener(this);
        }

        public bool IsAdsReady()
        {
            return Advertisement.IsReady(this.bannerAdsID)
                   || Advertisement.IsReady(this.interstitialAdsID)
                   || Advertisement.IsReady(this.rewardAdsID);
        }


        public void LoadAds()
        {
            Advertisement.Load(this.bannerAdsID);
            Advertisement.Load(this.interstitialAdsID);
            Advertisement.Load(this.rewardAdsID);
        }

        public async void ShowBannerAds()
        {
            if (!Advertisement.IsReady(this.bannerAdsID))
            {
                await UniTask.Delay(TimeSpan.FromSeconds(TimeOutAdsRequest));
            }

            Advertisement.Banner.SetPosition(this.bannerPos);
            Advertisement.Banner.Show(this.bannerAdsID);
        }
        public void HideBannerAds() { Advertisement.Banner.Hide(); }

        public async void ShowInterstitialAds()
        {
            if (!Advertisement.IsReady(this.interstitialAdsID))
            {
                await UniTask.Delay(TimeSpan.FromSeconds(TimeOutAdsRequest));
            }

            Advertisement.Show(this.interstitialAdsID);
        }

        public async void ShowRewardAds()
        {
            if (!Advertisement.IsReady(this.rewardAdsID))
            {
                await UniTask.Delay(TimeSpan.FromSeconds(TimeOutAdsRequest));
            }

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