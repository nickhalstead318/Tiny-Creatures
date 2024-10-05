using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManagerBehavior : MonoBehaviour
{
    [SerializeField]
    private GameObject _healthText;
    private PlayerBehavior _player;

    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerBehavior>();
    }

    // Update is called once per frame
    void Update()
    {
        _healthText.GetComponent<TMP_Text>().text = "Health: " + _player.GetHealth();
    }
}
