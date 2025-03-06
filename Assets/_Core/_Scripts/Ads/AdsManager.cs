using UnityEngine.Serialization;

namespace _Core._Scripts.Ads
{
    public class AdsManager : PersistentSingleton<AdsManager>
    {
        public InitializeAds initializeAds;
        
        public BannerAds bannerAds;
        public InterstitialAds interstitialAds;
        public RewardedAds rewardedAds;

        protected override void Awake() {
            base.Awake();
            
            bannerAds.LoadBannerAd();
            interstitialAds.LoadInterstitialAd();
            rewardedAds.LoadRewardAd();
        }
    }
}