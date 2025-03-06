using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.Events;

namespace _Core._Scripts.Ads
{
    public class RewardedAds : MonoBehaviour, IUnityAdsLoadListener, IUnityAdsShowListener
    {
        [SerializeField] private string androidAdUnitID;
        [SerializeField] private string iosAdUnitID;
        private string adUnitID;
        
        public UnityAction OnCompleteAds = delegate {  };
        public Observer<bool> AdsAvailable = new(true);
        

        private void Awake() {
#if UNITY_IOS
            adUnitID = iosAdUnitID;
#elif UNITY_ANDROID
            adUnitID = androidAdUnitID;
#elif UNITY_EDITOR
            adUnitID = androidAdUnitID;
#endif
        }
        
        public void LoadRewardAd() {
            Advertisement.Load(adUnitID, this);
        }
        public void ShowRewardAd() {
            Advertisement.Show(adUnitID, this);
            AdsAvailable.Value = false;
            LoadRewardAd();
        }

        #region LoadCallBacks
        public void OnUnityAdsAdLoaded(string placementId) {
            AdsAvailable.Value = true;
        }

        public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message) {
            
        }
        #endregion

        #region ShowCallBacks
        public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message) {
           
        }

        public void OnUnityAdsShowStart(string placementId) {
           
        }
        public void OnUnityAdsShowClick(string placementId) {
           
        }

        public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState) {
            if (adUnitID.Equals(placementId) && showCompletionState.Equals(UnityAdsShowCompletionState.COMPLETED))
            {
                OnCompleteAds.Invoke();
            }
        }
        #endregion
    }
}