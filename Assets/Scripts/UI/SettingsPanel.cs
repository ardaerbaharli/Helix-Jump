using System;
using UnityEngine;
using Utilities;

namespace UI
{
    public class SettingsPanel : MonoBehaviour
    {
        [SerializeField] private ToggleSwitch soundToggle, vibrationToggle;

        private void OnEnable()
        {
            soundToggle.Toggle(Config.IsSoundOn);
            vibrationToggle.Toggle(Config.IsVibrationOn);

            soundToggle.OnValueChanged += OnSoundToggleValueChanged;
            vibrationToggle.OnValueChanged += OnVibrationToggleValueChanged;
        }

        private void OnSoundToggleValueChanged(bool value)
        {
            SoundManager.instance.PlayUiClick();
            Config.IsSoundOn = value;
        }

        private void OnVibrationToggleValueChanged(bool value)
        {
            SoundManager.instance.PlayUiClick();
            Config.IsVibrationOn = value;
        }

        public void GoBack()
        {
            PageController.Instance.GoBack();
        }
    }
}