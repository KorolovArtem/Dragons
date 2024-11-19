using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    public bool isSoundOn = true;
    [SerializeField] private Sprite soundOnSprite;
    [SerializeField] private Sprite soundOffSprite;
    [SerializeField] private Button soundToggleButton;
    [SerializeField] private AudioClip healSound;
    [SerializeField] private AudioClip damageSound;
    [SerializeField] private AudioClip powerUpSound;

    private List<AudioSource> audioSources = new List<AudioSource>();
    private AudioSource effectSource;
    private bool isPaused = false;

    void Awake()
    {
        if (PlayerPrefs.HasKey("IsSoundOn"))
        {
            isSoundOn = PlayerPrefs.GetInt("IsSoundOn") == 1;
        }

        UpdateAudioSources();
        UpdateButtonSprite();
        ApplySoundSettings();

        effectSource = gameObject.AddComponent<AudioSource>();

        if (soundToggleButton != null)
        {
            soundToggleButton.onClick.AddListener(ToggleSound);
        }
    }

    void OnApplicationPause(bool pauseStatus)
    {
        isPaused = pauseStatus;
        ApplySoundSettings();
    }

    void OnApplicationFocus(bool hasFocus)
    {
        isPaused = !hasFocus;
        ApplySoundSettings();
    }

    public void UpdateAudioSources()
    {
        audioSources.Clear();
        audioSources.AddRange(FindObjectsOfType<AudioSource>());
    }

    public void ToggleSound()
    {
        isSoundOn = !isSoundOn;
        PlayerPrefs.SetInt("IsSoundOn", isSoundOn ? 1 : 0);
        PlayerPrefs.Save();
        UpdateAudioSources();
        ApplySoundSettings();
    }

    public void ApplySoundSettings()
    {
        bool muteState = !isSoundOn || isPaused;
        foreach (AudioSource source in audioSources)
        {
            source.mute = muteState;
        }
        UpdateButtonSprite();
    }

    private void UpdateButtonSprite()
    {
        if (soundToggleButton != null)
        {
            soundToggleButton.image.sprite = isSoundOn ? soundOnSprite : soundOffSprite;
        }
    }

    public void StopAllSounds()
    {
        foreach (AudioSource source in audioSources)
        {
            if (source != null && source.isPlaying)
            {
                source.Stop();
            }
        }
    }

    public void PlayBackgroundMusic()
    {
        AudioSource backgroundMusic = audioSources.Find(source => source.CompareTag("BackgroundMusic"));
        if (backgroundMusic != null && !backgroundMusic.isPlaying)
        {
            backgroundMusic.Play();
        }
    }

    public void PlayHealSound()
    {
        if (healSound != null && effectSource != null)
        {
            effectSource.PlayOneShot(healSound);
        }
    }

    public void PlayDamageSound()
    {
        if (damageSound != null && effectSource != null)
        {
            effectSource.PlayOneShot(damageSound);
        }
    }

    public void PlayPowerUpSound()
    {
        if (powerUpSound != null && effectSource != null)
        {
            effectSource.PlayOneShot(powerUpSound);
        }
    }
}