using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagerBehavior : MonoBehaviour
{
    [SerializeField]
    private GameObject _healthText;
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
        _healthText.GetComponent<TMP_Text>().text = "Health: " + _player.GetHealth();
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
}
