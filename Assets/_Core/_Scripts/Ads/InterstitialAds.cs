using System;
using UnityEngine;
using UnityEngine.Advertisements;

namespace _Core._Scripts.Ads
{
    public class InterstitialAds : MonoBehaviour, IUnityAdsLoadListener, IUnityAdsShowListener
    {
        [SerializeField] private string androidAdUnitID ;
        [SerializeField] private string iosAdUnitID;
        private string adUnitID;

        private void Awake() {
#if UNITY_IOS
            adUnitID = iosAdUnitID;
#elif UNITY_ANDROID
            adUnitID = androidAdUnitID;
#elif UNITY_EDITOR
            adUnitID = androidAdUnitID;
#endif
        }
        
        public void LoadInterstitialAd() {
            Advertisement.Load(adUnitID, this);
        }
        public void ShowInterstitialAd() {
            Advertisement.Show(adUnitID, this);
            LoadInterstitialAd();
        }

        #region LoadCallBacks
        public void OnUnityAdsAdLoaded(string placementId) {
            
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
        }
        #endregion

    }
}