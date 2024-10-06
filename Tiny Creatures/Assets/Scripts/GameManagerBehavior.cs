using System.Collections;
using System.Collections.Generic;
using System.Data;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManagerBehavior : MonoBehaviour
{
    public GameObject healthText;
    public Slider healthSlider;
    public Image healthImage;
    public Gradient healthGradient;

    [SerializeField]
    private GameObject _gameOverText;
    [SerializeField]
    private GameObject _resetButton;
    
    public AudioSource currentMusic;

    private PlayerBehavior _player;
    [SerializeField]
    private GameObject _spawner;

    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerBehavior>();

        // Setup Spawner
        _spawner.GetComponent<EnemySpawnerBehavior>().StartSpawning();

        if ( currentMusic != null )
        {
            currentMusic.Play();
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
    }

    public void ResetScene()
    {
        SceneManager.LoadScene("SampleScene");
    }

    public void UpdatePlayerHealth(int currHealth,int maxHealth)
    {
        healthText.GetComponent<TMP_Text>().text = currHealth + "/" + maxHealth;
        healthSlider.maxValue = maxHealth;
        healthSlider.value = currHealth;
        healthImage.color = healthGradient.Evaluate(healthSlider.normalizedValue);
    }
}
