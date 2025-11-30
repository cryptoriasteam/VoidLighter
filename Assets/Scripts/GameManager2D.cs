using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager2D : MonoBehaviour
{
    public static GameManager2D Instance;

    public bool IsPingActive { get; private set; }
    public bool IsGameOver { get; private set; }

    public float pingDuration = 0.5f;
    public float pingCooldown = 3f;

    private float cooldownTimer;    
    public UnityEngine.UI.Slider volumeSlider;
    public AudioSource musicSource;


    public int Score { get; private set; } = 0;
    [Header("UI")]
    public TextMeshProUGUI scoreText;

    [Header("Health")]
    public int PlayerHealth { get; private set; } = 3;

    public GameObject heart1;
    public GameObject heart2;
    public GameObject heart3;
    


    [Header("Sounds")]
    public AudioClip enemyKillSound;
    public AudioClip playerDeathSound;
    public AudioClip pingSound;

    public GameObject restartPopup;

    private AudioSource audioSource;


    void Start()
{
    if (musicToggle != null)
        musicToggle.SetIsOnWithoutNotify(true); 

    if (musicSource != null)
        musicSource.mute = false;

    if (sfxToggle != null)
        sfxToggle.isOn = sfxEnabled;
}

    void Awake()
    {
        Instance = this;
        audioSource = GetComponent<AudioSource>();
        
    }

    void Update()
    {
        if (!IsGameOver)
        {
            if (cooldownTimer > 0)
                cooldownTimer -= Time.deltaTime;
        }


        if (scoreText != null)
            scoreText.text = "Score: " + Score;

            if (volumeSlider != null && musicSource != null)
        musicSource.volume = volumeSlider.value;
    }


    public void AddScore(int amount)
    {
        Score += amount;
    }


    public void PlayKillSound()
    {
        if (enemyKillSound != null)
            audioSource.PlayOneShot(enemyKillSound);
    }

    public void PlayPlayerDeathSound()
    {
        if (playerDeathSound != null)
            audioSource.PlayOneShot(playerDeathSound);
    }

    public void PlayPingSound()
    {
        if (pingSound != null)
            audioSource.PlayOneShot(pingSound);
    }


    public void StartPing()
    {
        if (cooldownTimer > 0 || IsPingActive) return;

        PlayPingSound();
        StartCoroutine(PingRoutine());
    }

    private System.Collections.IEnumerator PingRoutine()
    {
        IsPingActive = true;
        cooldownTimer = pingCooldown;

        yield return new WaitForSeconds(pingDuration);

        IsPingActive = false;
    }


    public void GameOver()
    {
        if (IsGameOver) return;

        IsGameOver = true;

        PlayPlayerDeathSound();

        if (restartPopup != null)
            restartPopup.SetActive(true);

    }


    public void Restart()
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.buildIndex);
    }
    public void GoToMainMenu()
{
    SceneManager.LoadScene("MainMenu");
}



public void DamagePlayer(int amount)
{
    if (IsGameOver) return;

    PlayerHealth -= amount;
    UpdateHearts();

    if (PlayerHealth <= 0)
        GameOver();
}



    private void UpdateHearts()
    {
        if (PlayerHealth <= 2 && heart3 != null) heart3.SetActive(false);
        if (PlayerHealth <= 1 && heart2 != null) heart2.SetActive(false);
        if (PlayerHealth <= 0 && heart1 != null) heart1.SetActive(false);
    }

    public void StartGame()
{
    SceneManager.LoadScene("SampleScene");
}

public void QuitGame()
{
    Application.Quit();
}


public GameObject optionsPanel;
public AudioSource musicAudioSource;
public bool sfxEnabled = true;

public void OpenOptions()
{
    optionsPanel.SetActive(true);
}

public void CloseOptions()
{
    optionsPanel.SetActive(false);
}

public void SetMasterVolume(float value)
{
    musicSource.volume = value;
}

public UnityEngine.UI.Toggle musicToggle;
public UnityEngine.UI.Toggle sfxToggle;


public void ToggleMusic(bool enabled)
{
    Debug.Log("ToggleMusic called, enabled = " + enabled);

    if (musicSource == null)
    {
        Debug.LogError("MusicSource NOT assigned!");
        return;
    }

    musicSource.mute = !enabled;


    Debug.Log("musicSource.mute = " + musicSource.mute);
}


public void ToggleSFX(bool enabled)
{
    sfxEnabled = enabled;
}

}
