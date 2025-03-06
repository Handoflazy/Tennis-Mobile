using System.Collections;
using UnityEngine;

namespace _Core._Scripts.Ads
{
    public class AdsManager : RegulatorSingleton<AdsManager>
    {
        public InitializeAds initializeAds;
        
        public BannerAds bannerAds;
        public InterstitialAds interstitialAds;
        public RewardedAds rewardedAds;
        
        private IEnumerator Start() {
            bannerAds.LoadBannerAd();
            interstitialAds.LoadInterstitialAd();
            yield return new WaitForSeconds(1);
            rewardedAds.LoadRewardAd();
        }
    }
}