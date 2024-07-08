using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject prefabToSpawn;  
    private GameObject currentPrefab; 
    public float respawnTime = 5f; 

    private bool isSpawning = false; 

    void Start()
    {
        SpawnPrefab();
    }

    void Update()
    {
        if (currentPrefab == null && !isSpawning)
        {
            StartCoroutine(RespawnPrefab());
        }
    }

    void SpawnPrefab()
    {
        currentPrefab = Instantiate(prefabToSpawn, transform.position, transform.rotation);
    }

    IEnumerator RespawnPrefab()
    {
        isSpawning = true;
        yield return new WaitForSeconds(respawnTime);
        SpawnPrefab();
        isSpawning = false;
    }
}
