using System.Collections;
using System.Collections.Generic;
using System.Data;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManagerBehavior : MonoBehaviour
{
    // Player Health Info
    public TextMeshProUGUI healthText;
    public Slider healthSlider;
    public Image healthImage;
    public Gradient healthGradient;

    // Player Experience Info
    public TextMeshProUGUI playerLevelText;
    public Slider xpSlider;

    [SerializeField]
    private GameObject _gameOverText;
    [SerializeField]
    private GameObject _resetButton;

    public List<AudioClip> musicSet;
    private AudioSource _musicPlayer;

    private PlayerBehavior _player;
    [SerializeField]
    private EnemySpawnerBehavior _spawner;

    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerBehavior>();
        _musicPlayer = transform.GetComponent<AudioSource>();

        // Setup Spawner
        _spawner.StartSpawning();

        if (musicSet.Count > 0)
        {
            _musicPlayer.clip = musicSet[0];
            _musicPlayer.Play();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPlayerDeath()
    {
        _spawner.GetComponent<EnemySpawnerBehavior>().OnPlayerDeath();
        _gameOverText.SetActive(true);
        _resetButton.SetActive(true);

        if (_musicPlayer.isPlaying)
        {
            transform.GetComponent<AudioSource>().Stop();
        }
    }

    public void ResetScene()
    {
        SceneManager.LoadScene("Level1");
    }

    public void UpdatePlayerHealth(int currHealth,int maxHealth)
    {
        healthText.text = currHealth + "/" + maxHealth;
        healthSlider.maxValue = maxHealth;
        healthSlider.value = currHealth;
        healthImage.color = healthGradient.Evaluate(healthSlider.normalizedValue);
    }

    public void UpdatePlayerExperience(int currXp,int xpToNextLevel, int playerLevel, bool increaseDiff)
    {
        playerLevelText.text = "Level " + playerLevel;
        xpSlider.maxValue = xpToNextLevel;
        xpSlider.value = currXp;
        if (increaseDiff)
        {
            _spawner.IncreaseDifficulty();
        }
    }
}
