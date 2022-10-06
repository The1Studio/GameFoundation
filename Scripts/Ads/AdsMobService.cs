namespace GameFoundation.Scripts.Ads
{
    using System;
    using Cysharp.Threading.Tasks;
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

        private const float TimeOutRequestAds = 1f;


        public Action OnRewardSucceed { get; set; }

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

        public void ShowBannerAds() { this.bannerView?.Show(); }

        public void HideBannerAds() { this.bannerView?.Hide(); }

        public async void ShowInterstitialAds()
        {
            if (!this.interstitialAd.IsLoaded())
            {
                this.interstitialAd.LoadAd(this.adRequest);
                await UniTask.Delay(TimeSpan.FromSeconds(TimeOutRequestAds));
            }

            this.interstitialAd.Show();
        }

        public async void ShowRewardAds()
        {
            if (!this.rewardedAd.IsLoaded())
            {
                this.rewardedAd.LoadAd(this.adRequest);
                await UniTask.Delay(TimeSpan.FromSeconds(TimeOutRequestAds));
            }

            this.rewardedAd.Show();
        }
    }
}