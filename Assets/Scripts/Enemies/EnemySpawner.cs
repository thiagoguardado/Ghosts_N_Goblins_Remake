using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemySpawner : MonoBehaviour {

    public GameObject objectPrefab;

    [Header("Spawning Area and Time")]
    public Collider2D collider;
    public float minBetweenSpawnTime = 2f;
    public float maxBetweenSpawnTime = 4f;
    private float timer = 0f;
    private float nextTime;

    void Start()
    {
        SetupNextTime();
    }

    protected virtual void Update()
    {
        if (collider.OverlapPoint(PlayerController.Instance.transform.position))
        {
            UpdateTimer();
        }

        if (timer >= nextTime)
        {
            Spawn(SelectAreaToSpawn());
            SetupNextTime();
            timer = 0f;
        }
    }

    private void UpdateTimer()
    {
        timer += Time.deltaTime;
    }

    private void SetupNextTime()
    {
        nextTime = UnityEngine.Random.Range(minBetweenSpawnTime, maxBetweenSpawnTime);
    }

    private void Spawn(Vector3 position)
    {
        Instantiate(objectPrefab, position, objectPrefab.transform.rotation);
    }

    protected abstract Vector3 SelectAreaToSpawn();
}


