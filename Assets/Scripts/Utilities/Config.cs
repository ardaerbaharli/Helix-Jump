namespace Utilities
{
    public class Config
    {
        private static readonly string VibrationPref = "Vibration";
        private static readonly string SoundPref = "Sound";
        public static readonly string HighScorePref = "HighScore";

        public static int DeathCount;
        public static bool IsVibrationOn
        {
            get => PlayerPrefsX.GetBool(VibrationPref, true);
            set => PlayerPrefsX.SetBool(VibrationPref, value);
        }

        public static bool IsSoundOn
        {
            get => PlayerPrefsX.GetBool(SoundPref, true);
            set => PlayerPrefsX.SetBool(SoundPref, value);
        }
        
    }
}