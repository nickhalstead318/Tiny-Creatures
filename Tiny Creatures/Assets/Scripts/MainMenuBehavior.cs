using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuBehavior : MonoBehaviour
{

    private AudioSource _musicPlayer;

    // Start is called before the first frame update
    void Start()
    {
        _musicPlayer = transform.GetComponent<AudioSource>();
        _musicPlayer.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartGame()
    {
        SceneManager.LoadScene("SampleScene");
    }

    public void QuitGame()
    { 
        Application.Quit();
    }
}
