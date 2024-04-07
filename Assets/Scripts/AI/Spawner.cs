using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Spawner : MonoBehaviour
{

    [SerializeField] Vector2 spawnZoneSize;
    [SerializeField] Vector2 interval;
    [SerializeField] Vector2Int customersPerSpawn;

    // [SerializeField] GameObject customerPrefab;
    [SerializeField] List<CustomerSO> customerVariants;

    [SerializeField] private List<AICore> customersPool;

    private void Start()
    {
        StartCoroutine(SpawnCoroutine());    
    }

    IEnumerator SpawnCoroutine()
    {
        yield return new WaitForSeconds(interval.RandomRange());
        for (int i = 0; i < customersPerSpawn.RandomRange(); i++)
        {
            SpawnCustomer();
        }
        StartCoroutine(SpawnCoroutine());
    }

    private void SpawnCustomer()
    {
        AICore pulledCustomer = customersPool.Where(core => !core.isActive).FirstOrDefault();
        if (pulledCustomer == null) 
            return;

        Vector3 spawnPoint = RandomSpawnPoint(transform.position, spawnZoneSize);
        pulledCustomer.transform.position = spawnPoint;
        pulledCustomer.Initialize(customerVariants.RandomChoice());


    }

    private Vector3 RandomSpawnPoint(Vector3 zonePos, Vector2 zoneSize)
    {
        Vector3 offset = new Vector3(Random.Range(-zoneSize.x / 2, zoneSize.x / 2), transform.position.y, Random.Range(-zoneSize.y / 2, zoneSize.y / 2));
        return zonePos + offset;
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(transform.position, new Vector3(spawnZoneSize.x, 0, spawnZoneSize.y));
    }
}
