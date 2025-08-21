using System;
using System.Collections;
using Unity.Cinemachine;
using UnityEngine;
using Random = UnityEngine.Random;

public class ZombieSpawner : MonoBehaviour
{
    [SerializeField] private Transform Player;
    
    [SerializeField] private GameObject zombiePrefab;
    [SerializeField] private float zombieSpawnRate;
    [SerializeField] private float maxSpawnedZombies;
    [SerializeField] private float spawnDistance;

    private int zombieCount;

    private void Start()
    {
        StartCoroutine(SpawnZombies());
    }

    private IEnumerator SpawnZombies()
    {
        while (true)
        {
            var randomAngle = Random.Range(0, 360);
            var direction= Player.transform.up;
            var spawnPosition = Quaternion.Euler(0, 0, randomAngle) * direction.normalized * spawnDistance;
            if (zombieCount < maxSpawnedZombies)
            {
                var zombie = Instantiate(zombiePrefab, spawnPosition + Player.transform.position, Quaternion.identity);
                zombie.GetComponent<Health>().Death += OnZombieDeath;
                zombieCount++;
            }

            yield return new WaitForSeconds(zombieSpawnRate);
        }
    }

    private void OnZombieDeath(string obj)
    {
        zombieCount--;
    }
}