using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private int maxHealth = 5;
    [SerializeField] private int currentHealth;
    [SerializeField] private Image[] healthIcons;
    [SerializeField] private Sprite fullHeart;
    [SerializeField] private Sprite emptyHeart;
    [SerializeField] private GameObject achievementsPanel;
    [SerializeField] private GameObject SoundToggleButton;
    [SerializeField] private Image[] achievementIcons;
    [SerializeField] private Sprite[] unlockedAchievements;
    [SerializeField] private Sprite[] lockedAchievements;
    [SerializeField] private Text totalKillsText;
    [SerializeField] private Button restartButton;
    [SerializeField] private AudioManager audioManager;
    [SerializeField] private FireBarManager fireBarManager;
    [SerializeField] private Button changeLanguageButton;
    [SerializeField] private Sprite[] flagIcons;

    private int totalKills = 0;
    private int language;

    void Start()
    {
        currentHealth = maxHealth;
        totalKills = PlayerPrefs.GetInt("TotalKills", 0);
        language = PlayerPrefs.GetInt("language", 0);
        achievementsPanel.SetActive(false);
        SoundToggleButton.SetActive(false);
        restartButton.onClick.AddListener(RestartGame);

        if (changeLanguageButton != null)
        {
            changeLanguageButton.onClick.AddListener(ChangeLanguage);
            UpdateFlagIcon();
        }

        UpdateHealthUI();

        audioManager.PlayBackgroundMusic();
    }
    void UpdateHealthUI()
    {
        for (int i = 0; i < healthIcons.Length; i++)
        {
            healthIcons[i].sprite = i < currentHealth ? fullHeart : emptyHeart;
            healthIcons[i].enabled = i < maxHealth;
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        UpdateHealthUI();
        if (audioManager != null)
        {
            audioManager.PlayDamageSound();
        }
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        if (audioManager != null)
        {
            audioManager.StopAllSounds();
        }
        ShowAchievements();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Weapon"))
        {
            TakeDamage(1);
        }
    }

    public void Heal(int amount)
    {
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
        UpdateHealthUI();
        if (audioManager != null)
        {
            audioManager.PlayHealSound();
        }
    }

    public void SetMaxHealth(int newMaxHealth)
    {
        maxHealth = newMaxHealth;
        currentHealth = Mathf.Min(currentHealth, maxHealth);
        UpdateHealthUI();
    }

    private void ShowAchievements()
    {
        achievementsPanel.SetActive(true);
        SoundToggleButton.SetActive(true);

        string killsText = GetLocalizedKillsText();
        totalKillsText.text = killsText + ": " + totalKills;

        UpdateAchievementIcons();
        Time.timeScale = 0;
    }

    private string GetLocalizedKillsText()
    {
        switch (language)
        {
            case 1:
                return "Total kills";
            case 2:
                return "Total Kills in Language 2";
            default:
                return "Всего убито";
        }
    }

    private void ChangeLanguage()
    {
        language = (language + 1) % flagIcons.Length;
        PlayerPrefs.SetInt("language", language);
        PlayerPrefs.Save();

        ShowAchievements();
        UpdateFlagIcon();
    }

    private void UpdateFlagIcon()
    {
        if (flagIcons != null && flagIcons.Length > language)
        {
            changeLanguageButton.image.sprite = flagIcons[language];
        }
    }

    private void UpdateAchievementIcons()
    {
        for (int i = 0; i < achievementIcons.Length; i++)
        {
            if (i < unlockedAchievements.Length && i < lockedAchievements.Length)
            {
                if (totalKills >= Mathf.Pow(10, i + 1) * 10)
                {
                    achievementIcons[i].sprite = unlockedAchievements[i];
                }
                else
                {
                    achievementIcons[i].sprite = lockedAchievements[i];
                }
            }
        }
    }

    private void RestartGame()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void IncrementTotalKills()
    {
        totalKills++;
        SaveAchievements();
    }

    private void SaveAchievements()
    {
        PlayerPrefs.SetInt("TotalKills", totalKills);
        PlayerPrefs.Save();
    }

    public void ActivatePowerUp()
    {
        if (fireBarManager != null)
        {
            fireBarManager.RestoreAllFirePoints();
            fireBarManager.ActivateInfiniteFire(8f);
        }
        if (audioManager != null)
        {
            audioManager.PlayPowerUpSound();
        }
    }
}