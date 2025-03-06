using UnityEngine;
using UnityEngine.Advertisements;

namespace _Core._Scripts.Ads
{
    public class BannerAds : MonoBehaviour
    {
        [SerializeField] private string androidAdUnitID;
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
            Advertisement.Banner.SetPosition(BannerPosition.TOP_CENTER);
        }
        
        public void LoadBannerAd() {
            BannerLoadOptions options = new() {
                loadCallback = BannerLoaded,
                errorCallback = BannerLoadedError
            };
            Advertisement.Banner.Load(adUnitID, options);
        }
        public void ShowBannerAds() {
            BannerOptions options = new BannerOptions() {
                showCallback = BannerShown,
                clickCallback = BannerClicked,
                hideCallback = BannerHidden
            };
            Advertisement.Banner.Show(adUnitID, options);
            LoadBannerAd();
        }
        public void HideBannerAds() => Advertisement.Banner.Hide();

        private void BannerClicked() {
           
        }

        private void BannerHidden() {
            
        }

        private void BannerShown() {
          
        }

        private void BannerLoadedError(string message) {
        }

        private void BannerLoaded() {
        }

        
    }
}