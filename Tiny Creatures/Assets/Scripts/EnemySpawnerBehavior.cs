using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnerBehavior : MonoBehaviour
{
    private int _difficultyLevel = 1;
    private int _difficultyMax = 1;

    [SerializeField]
    private float _timeBetweenSpawns = 1f;
    public float distFromPlayer = 10f;

    private GameObject _playerObject;
    private Dictionary<Enemies, GameObject> _enemies;
    public GameObject experienceGem;

    [SerializeField]
    private List<GameObject> _enemiesList;

    private IEnumerator _spawnRoutine;

    // Start is called before the first frame update
    void Start()
    {
        _playerObject = GameObject.FindGameObjectWithTag("Player");
        SetupEnemies();
        _spawnRoutine = SpawnEnemies();
    }

    // Update is called once per frame
    void Update()
    {

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

    public void StartSpawning()
    {
        StartCoroutine(_spawnRoutine);
    }

    void StopSpawning()
    {
        StopCoroutine(_spawnRoutine);
    }

    public void OnPlayerDeath()
    {
        StopSpawning();
        foreach(Transform child in transform)
        {
            Destroy(child.gameObject);
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

    public void IncreaseDifficulty()
    {
        _difficultyLevel += 1;
    }
}
