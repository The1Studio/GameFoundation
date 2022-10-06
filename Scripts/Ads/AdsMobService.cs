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
        private AdRequest      adRequest;

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
            this.adRequest      = new AdRequest.Builder().Build();
        }

        public void LoadAds()
        {
            this.bannerView.LoadAd(this.adRequest);
            this.interstitialAd.LoadAd(this.adRequest);
            this.LoadReward();
        }

        private void LoadReward()
        {
            this.rewardedAd.OnUserEarnedReward += this.OnUserEarnedReward;
            this.rewardedAd.LoadAd(this.adRequest);
        }
        private void OnUserEarnedReward(object sender, Reward e) { this.OnRewardSucceed.Invoke(); }

        public void ShowBannerAds()
        {
            this.bannerView.LoadAd(this.adRequest);
            this.bannerView.Show();
        }

        public void HideBannerAds() { this.bannerView.Hide(); }

        public void ShowInterstitialAds()
        {
            this.rewardedAd = new RewardedAd(this.adUnitID);
            this.interstitialAd.Show();
        }

        public void ShowRewardAds()
        {
            this.LoadReward();
            this.rewardedAd.Show();
        }
    }
}