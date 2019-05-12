using GoogleMobileAds.Api;
using Azuma.Ids;
using Azuma.Utils;
using UnityEngine;
using UnityEngine.Events;

namespace Azuma.Manager
{
    /// <summary>
    /// Ad Mob Manager.
    /// </summary>
    public class AdMobManager : SingletonMonoBehaviour<AdMobManager>
    {
        /// <summary>
        /// The interstitial.
        /// </summary>
        private InterstitialAd interstitial;

        /// <summary>
        /// The request.
        /// </summary>
        private AdRequest request;

        /// <summary>
        /// The callback.
        /// </summary>
        private UnityAction callback;

        /// <summary>
        /// Start this instance.
        /// </summary>
        private void Start()
        {
            // 起動時にインタースティシャル広告をロードしておく
            this.RequestInterstitial();

            // バナー広告を表示
            this.RequestBanner();
        }

        /// <summary>
        /// Requests the banner.
        /// </summary>
        private void RequestBanner()
        {
#if UNITY_ANDROID
            string adUnitId = AdMobIds.Android_Banner;
#elif UNITY_IOS
            string adUnitId = AdMobIds.ios_Banner;
#else
            string adUnitId = "unexpected_platform";
#endif

            // Create a 320x50 banner at the top of the screen.
            BannerView bannerView = new BannerView(adUnitId, AdSize.Banner, AdPosition.Bottom);
            // Create an empty ad request.
            this.request = new AdRequest.Builder()
                // テスト完了後にコメント化
                .AddTestDevice(AdRequest.TestDeviceSimulator)
                .Build();

            // Load the banner with the request.
            bannerView.LoadAd(this.request);
        }

        /// <summary>
        /// Requests the interstitial.
        /// </summary>
        private void RequestInterstitial()
        {
#if UNITY_ANDROID
            string adUnitId = AdMobIds.Android_Interstitial;
#elif UNITY_IOS
            string adUnitId = AdMobIds.ios_Interstitial;
#else
            string adUnitId = "unexpected_platform";
#endif

            // Initialize an InterstitialAd.
            this.interstitial = new InterstitialAd(adUnitId);
            this.interstitial.OnAdClosed += (sender, args) =>
            {
                this.interstitial.Destroy();
                this.RequestInterstitial();
#if UNITY_IOS
                this.callback?.Invoke();
#endif
            };

            // Create an empty ad request.
            this.request = new AdRequest.Builder()
                // テスト完了後にコメント化
                .AddTestDevice(AdRequest.TestDeviceSimulator)
                .Build();

            // Load the interstitial with the request.
            this.interstitial.LoadAd(this.request);
        }

        /// <summary>
        /// Displaies the interstitial.
        /// </summary>
        /// <param name="callback">Cb.</param>
        public void DisplayInterstitial(UnityAction callback = null)
        {
            Time.timeScale = 0;
            this.callback = () =>
            {
                Time.timeScale = 1;
                callback();
            };

            // 広告表示
            if (this.interstitial.IsLoaded())
            {
                this.interstitial.Show();
#if !UNITY_IOS || UNITY_EDITOR
                this.callback?.Invoke();
#endif
            }
            else
            {
                this.callback?.Invoke();
            }
        }
    }
}
