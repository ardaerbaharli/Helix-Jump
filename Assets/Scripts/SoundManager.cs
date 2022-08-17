using UnityEngine;
using UnityEngine.Audio;
using Utilities;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioMixer mixer;

    [SerializeField] private AudioSource ballJump;
    [SerializeField] private AudioSource helixBreak;
    [SerializeField] private AudioSource uiClick;

    public static SoundManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            // DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        SetSound(Config.IsSoundOn);
    }

    public void SetSound(bool value)
    {
        Config.IsSoundOn = value;
        mixer.SetFloat("Master", value ? 0 : -80);
    }

    public void PlayBallJump()
    {
        ballJump.Play();
    }

    public void PlayHelixBreak()
    {
        helixBreak.Play();
    }

    public void PlayUiClick()
    {
        uiClick.Play();
    }
}