using System;
using UnityEngine;
using UnityEngine.Advertisements;

namespace Ads
{
    public class AdsInitializer : MonoBehaviour, IUnityAdsInitializationListener
    {
        [SerializeField] string _androidGameId;
        [SerializeField] string _iOSGameId;
        [SerializeField] bool _testMode = true;
        private string _gameId;

        public Action OnUnityAdsInitialized;

       

        public void InitializeAds()
        {
            _gameId = (Application.platform == RuntimePlatform.Android)
                ? _androidGameId
                : _iOSGameId;
            Advertisement.Initialize(_gameId, _testMode, this);
        }

        public void OnInitializationComplete()
        {
            // Debug.Log("Unity Ads initialization complete.");
            OnUnityAdsInitialized?.Invoke();
        }

        public void OnInitializationFailed(UnityAdsInitializationError error, string message)
        {
            // Debug.Log($"Unity Ads Initialization Failed: {error.ToString()} - {message}");
        }
    }
}