using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnerBehavior : MonoBehaviour
{
    private int _difficultyLevel = 1;
    private int _difficultyMax = 1;
    private int _timeBetweenSpawns = 1;
    public float distFromPlayer = 9f;

    private GameObject _playerObject;
    private Dictionary<Enemies, GameObject> _enemies;

    [SerializeField]
    private List<GameObject> _enemiesList;

    private float timeToStop = 10f;
    private bool _spawningActive = true;
    private IEnumerator _spawningRoutine;

    // Start is called before the first frame update
    void Start()
    {
        _playerObject = GameObject.FindGameObjectWithTag("Player");
        _spawningRoutine = SpawnEnemies();
        SetupEnemies();
        StartCoroutine(_spawningRoutine);
    }

    // Update is called once per frame
    void Update()
    {
        timeToStop -= Time.deltaTime;
        if (timeToStop <= 0 && _spawningActive)
        {
            StopCoroutine(_spawningRoutine);
            _spawningActive = false;
        }
    }

    IEnumerator SpawnEnemies()
    {
        while (true)
        {
            
            int difficultyTotal = _difficultyLevel;
            int currentDifficulty = Random.Range(1, _difficultyMax);
            
            while (difficultyTotal > 0)
            {
                GameObject enemyToSpawn = null;

                switch (currentDifficulty)
                {
                    case 1:
                        enemyToSpawn = _enemies[Enemies.Enemy1];
                        break;
                }
                if (enemyToSpawn != null)
                {
                    Vector3 newLocation = _playerObject.transform.position + distFromPlayer * new Vector3(Random.Range(-100, 100), Random.Range(-100, 100), 0).normalized;
                    GameObject newEnemy = Instantiate(enemyToSpawn, newLocation, Quaternion.identity);
                    newEnemy.transform.parent = transform;

                    difficultyTotal -= currentDifficulty;
                }
            }

            yield return new WaitForSeconds(_timeBetweenSpawns);
        }
    }

    void SetupEnemies()
    {
        _enemies = new Dictionary<Enemies, GameObject>()
        {
            { Enemies.Enemy1, _enemiesList[0]}
        };
    }

    public enum Enemies
    {
        Enemy1
    }
}
