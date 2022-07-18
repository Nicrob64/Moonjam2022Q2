using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PissBottleSpawner : MonoBehaviour
{
    public GameObject PissBottlePrefab;
    public float SpawnDelay = 10f;
    
    float _elapsedDelay = 0;

    bool _shouldSpawnPissBottle = true;
    GameObject[] _spawnLocations;
    System.Random _rand = new();

    public void OnBottlePissedIn()
    {
        _shouldSpawnPissBottle = true;
        _elapsedDelay = 0;
    }

    void Awake()
    {
        _spawnLocations = GameObject.FindGameObjectsWithTag("PissBottleSpawn");

        EventManager.Instance.OnPissInBottle += OnBottlePissedIn;
    }

    // Update is called once per frame
    void Update()
    {
        if(_shouldSpawnPissBottle && _elapsedDelay > SpawnDelay)
        {
            GameObject spawn = _spawnLocations[_rand.Next(_spawnLocations.Length)];
            GameObject newBottle = Instantiate(PissBottlePrefab, spawn.transform.position, spawn.transform.rotation);
            newBottle.transform.SetParent(spawn.transform);

            _shouldSpawnPissBottle = false;
            _elapsedDelay = 0;
        }
        else if(_shouldSpawnPissBottle)
        {
            _elapsedDelay += Time.deltaTime;
        }
    }
}
