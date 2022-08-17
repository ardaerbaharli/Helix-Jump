using System;
using UnityEngine;

namespace Ads
{
    public class AdManager : MonoBehaviour
    {
        private AdsInitializer adsInitializer;
        private BannerAdController bannerAdController;
        private InterstitialAdController interstitialAdController;
        private RewardedAdController rewardedAdController;

        public bool IsBannerReady;
        public bool IsInterstitialReady;
        public bool IsRewardedReady;

        public static AdManager instance;

        public Action OnRewardedAdCompleted;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }

            adsInitializer = GetComponent<AdsInitializer>();
            bannerAdController = GetComponent<BannerAdController>();
            interstitialAdController = GetComponent<InterstitialAdController>();
            rewardedAdController = GetComponent<RewardedAdController>();

            adsInitializer.OnAdsInitialized += OnAdsInitialized;
        }

        private void OnAdsInitialized()
        {
            bannerAdController.LoadAd();
            bannerAdController.OnAdLoaded += OnBannerAdLoaded;

            interstitialAdController.LoadAd();
            interstitialAdController.OnAdLoaded += OnInterstitialAdLoaded;

            rewardedAdController.LoadAd();
            rewardedAdController.OnAdLoaded += OnRewardedAdLoaded;

            rewardedAdController.OnCompleted += OnRewardedAdComplete;
        }

        private void OnRewardedAdComplete()
        {
            OnRewardedAdCompleted?.Invoke();
        }

        private void OnRewardedAdLoaded()
        {
            IsRewardedReady = true;
            // rewardedAdController.ShowAd();
        }

        private void OnInterstitialAdLoaded()
        {
            IsInterstitialReady = true;
            // interstitialAdController.ShowAd();
        }

        private void OnBannerAdLoaded()
        {
            IsBannerReady = true;
            bannerAdController.ShowAd();
        }

        public void ShowRewardedAd()
        {
            if (IsRewardedReady)
            {
                rewardedAdController.ShowAd();
            }
        }

        public void ShowInterstitialAd()
        {
            if (IsInterstitialReady)
            {
                interstitialAdController.ShowAd();
            }
        }
    }
}