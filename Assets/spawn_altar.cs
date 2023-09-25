using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawn_altar : MonoBehaviour
{
    public GameObject enemyToSpawn;
    public GameObject spawnEffect;

    public bool hasSpawnedRecently = true;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(spawnCoRoutine());
    }

    public void spawn()
    {
        if (!hasSpawnedRecently) {
            Instantiate(enemyToSpawn, transform.position, transform.rotation);
            Instantiate(spawnEffect, transform.position, transform.rotation);
            StartCoroutine(spawnCoRoutine());
        }
    }

    IEnumerator spawnCoRoutine()
    {
        this.hasSpawnedRecently = true;
        yield return new WaitForSeconds(3f);
        this.hasSpawnedRecently = false;
    }
}
