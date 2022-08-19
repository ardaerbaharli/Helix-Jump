using System;
using Ads;
using Enums;
using TMPro;
using UnityEngine;

namespace UI
{
    public class GamePanel : MonoBehaviour
    {
        [SerializeField] private GameObject adPrompt;

        private void Start()
        {
            GameManager.instance.OnGameOver += OnGameOver;
        }

        private void OnDestroy()
        {
            GameManager.instance.OnGameOver -= OnGameOver;
        }

        private void OnGameOver()
        {
            if (AdManager.instance.isAdsActive)
                adPrompt.SetActive(true);
            else EndGame();
        }

        public void ShowAd()
        {
            adPrompt.SetActive(false);
            GameManager.instance.KeepPlaying();
        }

        public void EndGame()
        {
            adPrompt.SetActive(false);
            GameManager.instance.EndGame();
        }


        public void Pause()
        {
            SoundManager.instance.PlayUiClick();
            GameManager.instance.Pause();
            PageController.Instance.ShowPage(Pages.Pause);
        }
    }
}