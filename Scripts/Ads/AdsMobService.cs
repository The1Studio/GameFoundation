namespace GameFoundation.Scripts.Ads
{
    using System;
    using GoogleMobileAds.Api;

    public class AdsMobService : IAdsService
    {
        private readonly string     adUnitID;
        private readonly AdPosition bannerPosition;

        public AdsMobService(string adUnitID, AdPosition bannerPosition)
        {
            this.adUnitID       = adUnitID;
            this.bannerPosition = bannerPosition;
        }

        private BannerView     bannerView;
        private InterstitialAd interstitialAd;
        private RewardedAd     rewardedAd;

        public Action OnRewardSucceed { get; set; }

        public bool IsBannerReady => this.bannerView != null;

        public bool IsInterstitialReady => this.interstitialAd.IsLoaded();

        public bool IsRewardReady => this.rewardedAd.IsLoaded();
        public void InitializeAds()
        {
            MobileAds.Initialize(_ => { });

            this.bannerView     = new BannerView(this.adUnitID, AdSize.Banner, this.bannerPosition);
            this.interstitialAd = new InterstitialAd(this.adUnitID);
            this.rewardedAd     = new RewardedAd(this.adUnitID);
        }

        public void LoadAds()
        {
            this.LoadBanner();
            this.LoadInterstitial();
            this.LoadReward();
        }

        private void LoadBanner()
        {
            var adRequest = new AdRequest.Builder().Build();
            this.bannerView.OnAdClosed += this.ReloadBannerAds;
            this.bannerView.LoadAd(adRequest);
        }

        private void LoadInterstitial()
        {
            var adRequest = new AdRequest.Builder().Build();
            this.interstitialAd.OnAdClosed += this.ReloadInterstitialAds;
            this.interstitialAd.LoadAd(adRequest);
        }

        private void LoadReward()
        {
            var adRequest = new AdRequest.Builder().Build();
            this.rewardedAd.OnUserEarnedReward += this.OnUserEarnedReward;
            this.rewardedAd.OnAdClosed         += this.ReloadRewardAds;
            this.rewardedAd.LoadAd(adRequest);
        }

        private void ReloadBannerAds(object sender, EventArgs e) { this.LoadBanner(); }

        private void ReloadInterstitialAds(object sender, EventArgs e) { this.LoadInterstitial(); }

        private void ReloadRewardAds(object sender, EventArgs e) { this.LoadReward(); }

        private void OnUserEarnedReward(object sender, Reward e) { this.OnRewardSucceed.Invoke(); }

        public void ShowBannerAds() { this.bannerView.Show(); }

        public void HideBannerAds() { this.bannerView.Hide(); }

        public void ShowInterstitialAds() { this.interstitialAd.Show(); }

        public void ShowRewardAds() { this.rewardedAd.Show(); }
    }
}